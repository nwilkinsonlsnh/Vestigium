namespace Vestigium.Logging.Tests;

[Collection("VestigiumLogger")]
public sealed class MiniJanitorTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), "vestigium-mini-jan", Guid.NewGuid().ToString("N"));

    public MiniJanitorTests() => Directory.CreateDirectory(_dir);

    [Fact]
    public void Mini_janitor_deletes_only_its_own_aged_parts()
    {
        var old = DateTime.UtcNow.AddDays(-30);
        TestFiles.Touch(Path.Combine(_dir, "PingIQ.jsonl"), old);
        TestFiles.Touch(Path.Combine(_dir, "PingIQ.001.jsonl"), old);
        TestFiles.Touch(Path.Combine(_dir, "PingIQ.PathPing.jsonl"), old);
        TestFiles.Touch(Path.Combine(_dir, "PingIQ.PathPing.001.jsonl"), old);
        TestFiles.Touch(Path.Combine(_dir, "PingIQ.TraceRt.jsonl"), old);

        var result = VestigiumLogJanitor.DeleteMiniLogs(_dir, "PingIQ", "PathPing", TimeSpan.FromDays(7));
        Assert.Equal(2, result.Deleted);
        Assert.True(File.Exists(Path.Combine(_dir, "PingIQ.jsonl")));
        Assert.True(File.Exists(Path.Combine(_dir, "PingIQ.001.jsonl")));
        Assert.True(File.Exists(Path.Combine(_dir, "PingIQ.TraceRt.jsonl")));
        Assert.False(File.Exists(Path.Combine(_dir, "PingIQ.PathPing.jsonl")));
        Assert.False(File.Exists(Path.Combine(_dir, "PingIQ.PathPing.001.jsonl")));
    }

    [Fact]
    public void Host_janitor_does_not_delete_mini_files()
    {
        VestigiumLogger.Initialize(o =>
        {
            o.AppId = "PingIQ";
            o.LogDirectory = _dir;
            o.JanitorEnabled = true;
            o.JanitorMaxAge = TimeSpan.FromDays(7);
            o.OperationsLogEnabled = false;
        });
        var old = DateTime.UtcNow.AddDays(-30);
        TestFiles.Touch(Path.Combine(_dir, "PingIQ.001.jsonl"), old);
        TestFiles.Touch(Path.Combine(_dir, "PingIQ.PathPing.jsonl"), old);

        var result = VestigiumLogJanitor.DeleteHostLogs();
        Assert.True(result.Deleted >= 1);
        Assert.False(File.Exists(Path.Combine(_dir, "PingIQ.001.jsonl")));
        Assert.True(File.Exists(Path.Combine(_dir, "PingIQ.PathPing.jsonl")));
        VestigiumLogger.Shutdown();
    }

    public void Dispose()
    {
        VestigiumLogger.Shutdown();
        try { Directory.Delete(_dir, true); } catch { }
    }

    private static void TestFiles.Touch(string path, DateTime utc)
    {
        File.WriteAllText(path, "{}\n");
        File.SetLastWriteTimeUtc(path, utc);
    }
}
