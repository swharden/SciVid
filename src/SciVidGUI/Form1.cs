namespace SciVidGUI;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        AllowDrop = true;
        DragEnter += Form1_DragEnter; ;
        DragDrop += Form1_DragDrop; ;
    }

    private void Form1_DragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data!.GetDataPresent(DataFormats.FileDrop))
            e.Effect = DragDropEffects.Copy;
    }

    private void Form1_DragDrop(object? sender, DragEventArgs e)
    {
        string[] paths = (string[])e.Data!.GetData(DataFormats.FileDrop)!;
        if (paths.Length == 1 && paths[0].EndsWith(".tif"))
        {
            LoadTifStack(paths[0]);
        }
        else
        {
            throw new NotImplementedException("file(s) not supported");
        }
    }

    private void LoadTifStack(string tifFilePath)
    {

    }
}
