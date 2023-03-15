using FFMpegCore;
using FFMpegCore.Pipes;
using SkiaSharp;

namespace SciVid;

public class Annotator
{
    public double FrameRate { get; }
    public double FramePeriod { get; }

    public Annotator(double fps)
    {
        FrameRate = fps;
        FramePeriod = 1.0 / fps;
    }

    public void AnnotateTifStackWebm(string tifFilePath, string saveAs, double divideBy = 16)
    {
        SciTIF.TifFile tif = new(tifFilePath);
        SciTIF.Image[] images = tif.GetAllImages();
        foreach (SciTIF.Image image in images)
            image.ScaleBy(0, 1.0 / divideBy);

        IEnumerable<IVideoFrame> FrameGenerator()
        {
            using SKFont textFont = new(SKTypeface.FromFamilyName("consolas"), size: 14);
            using SKPaint textPaint = new(textFont) { Color = SKColors.Yellow, TextAlign = SKTextAlign.Left };
            for (int i = 0; i < images.Length; i++)
            {
                TimeSpan elapsed = TimeSpan.FromSeconds(i * FramePeriod);

                byte[] bytes = images[i].GetBitmapBytes();
                SKBitmap bmp = SKBitmap.Decode(bytes);
                using SKCanvas canvas = new(bmp);

                canvas.DrawText($"Frame {i}", 10, textPaint.FontSpacing, textPaint);
                canvas.DrawText($"{elapsed.Minutes}:{elapsed.Seconds}.{elapsed.Milliseconds / 10:00}", 10, 2 * textPaint.FontSpacing, textPaint);

                using SKBitmapFrame frame = new(bmp);
                yield return frame;
            }
        }

        saveAs = Path.GetFullPath(saveAs);
        var frameGen = FrameGenerator();
        RawVideoPipeSource videoFramesSource = new(frameGen) { FrameRate = FrameRate };
        bool success = FFMpegArguments
            .FromPipeInput(videoFramesSource)
            .OutputToFile(saveAs, overwrite: true, options => options.WithVideoCodec("libvpx-vp9"))
            .ProcessSynchronously();
        Console.WriteLine($"Wrote: {saveAs}");
    }

}
