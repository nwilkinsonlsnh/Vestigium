namespace Vestigium.Logging.Tests;

[Collection("VestigiumLogger")]
public sealed class MiniMultiInstanceTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), "vestigium-mini-multi", Guid.NewGuid().ToString("N"));

    public MiniMultiInstanceTests() => Directory.CreateDirectory(_dir);

    [Fact]
    public void Two_minis_against_one_app_do_not_share_state()
    {
        using var a = new VestigiumLoggerMini("PingIQ", "PathPing", _dir, floodThreshold: 1, miniCorrelationId: "corr-a");
        using var b = new VestigiumLoggerMini("PingIQ", "TraceRt", _dir, floodThreshold: 5, miniCorrelationId: "corr-b");

        for (var i = 0; i < 10; i++)
            a.Information(1, "Network", "Probe", "same");
        b.Information(1, "Network", "Probe", "same");
        Assert.True(a.Flush(TimeSpan.FromSeconds(2)));
        Assert.True(b.Flush(TimeSpan.FromSeconds(2)));

        var aPath = Path.Combine(_dir, "PingIQ.PathPing.jsonl");
        var bPath = Path.Combine(_dir, "PingIQ.TraceRt.jsonl");
        Assert.Equal(aPath, a.ActivePath);
        Assert.Equal(bPath, b.ActivePath);
        Assert.NotEqual(a.MiniCorrelationId, b.MiniCorrelationId);

        var aText = SharedRead.Text(aPath);
        var bText = SharedRead.Text(bPath);
        Assert.Contains("corr-a", aText);
        Assert.DoesNotContain("corr-b", aText);
        Assert.Contains("corr-b", bText);
        Assert.DoesNotContain("corr-a", bText);
        Assert.Contains("same", bText);
    }

    [Fact]
    public void Janitor_for_one_mini_leaves_the_other()
    {
        var old = DateTime.UtcNow.AddDays(-30);
        TestFiles.Touch(Path.Combine(_dir, "PingIQ.PathPing.jsonl"), old);
        TestFiles.Touch(Path.Combine(_dir, "PingIQ.TraceRt.jsonl"), old);

        var result = VestigiumLogJanitor.DeleteMiniLogs(_dir, "PingIQ", "PathPing", TimeSpan.FromDays(7));
        Assert.Equal(1, result.Deleted);
        Assert.False(File.Exists(Path.Combine(_dir, "PingIQ.PathPing.jsonl")));
        Assert.True(File.Exists(Path.Combine(_dir, "PingIQ.TraceRt.jsonl")));
    }

    public void Dispose()
    {
        try { Directory.Delete(_dir, true); } catch { }
    }

    private static void TestFiles.Touch(string path, DateTime utc)
    {
        File.WriteAllText(path, "{}\n");
        File.SetLastWriteTimeUtc(path, utc);
    }
}
