public class AnnotatorTests
{
    [Test]
    public void Test_Annotator_TifStack()
    {
        SciVid.Annotator an = new(fps: 5);
        an.AnnotateTifStackWebm(SampleData.Video16bitPath, "stack.webm");
    }
}