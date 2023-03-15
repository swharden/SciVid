using FluentAssertions;

internal class SampleData
{
    public static string RepoRootPath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../"));

    public static string Video16bitPath = Path.GetFullPath(Path.Combine(RepoRootPath, "data/images/video-16-bit.tif"));

    [Test]
    public void Test_RepoRoot_CanBeFound()
    {
        Directory.Exists(RepoRootPath).Should().BeTrue();
        File.Exists(Path.Combine(RepoRootPath, "LICENSE")).Should().BeTrue();
    }

    [Test]
    public void Test_DemoFiles_CanBeFound()
    {
        File.Exists(Video16bitPath).Should().BeTrue();
    }
}
