namespace Vestigium.Logging.Tests;

public sealed class LogJanitorTests
{
    [Fact]
    public void DeletesOldFilesAndKeepsRecent()
    {
        var dir = Path.Combine(Path.GetTempPath(), "vestigium-janitor-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        var oldFile = Path.Combine(dir, "PingIQ.001.jsonl");
        var newFile = Path.Combine(dir, "PingIQ.jsonl");
        File.WriteAllText(oldFile, "{}\n");
        File.WriteAllText(newFile, "{}\n");
        var result = VestigiumLogJanitor.DeleteOlderThan(TimeSpan.FromDays(14), dir, "PingIQ");
        Assert.Equal(1, result.Deleted);
        Assert.False(File.Exists(oldFile));
        Assert.True(File.Exists(newFile));
    }

    [Fact]
    public void RejectsNonPositiveAge()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            VestigiumLogJanitor.DeleteOlderThan(TimeSpan.Zero, "x", "PingIQ"));
    }
}
