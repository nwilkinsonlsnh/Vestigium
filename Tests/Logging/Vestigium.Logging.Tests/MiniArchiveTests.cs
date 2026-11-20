namespace Vestigium.Logging.Tests;

[Collection("VestigiumLogger")]
public sealed class MiniArchiveTests : IDisposable
{
    private readonly string _src = Path.Combine(Path.GetTempPath(), "vestigium-mini-arch-src", Guid.NewGuid().ToString("N"));
    private readonly string _dst = Path.Combine(Path.GetTempPath(), "vestigium-mini-arch-dst", Guid.NewGuid().ToString("N"));

    public MiniArchiveTests()
    {
        Directory.CreateDirectory(_src);
        Directory.CreateDirectory(_dst);
    }

    [Fact]
    public void Archives_only_aged_mini_parts()
    {
        var old = DateTime.UtcNow.AddDays(-30);
        Touch(Path.Combine(_src, "PingIQ.jsonl"), old);
        Touch(Path.Combine(_src, "PingIQ.PathPing.jsonl"), old);
        Touch(Path.Combine(_src, "PingIQ.PathPing.001.jsonl"), old);
        Touch(Path.Combine(_src, "PingIQ.TraceRt.jsonl"), old);

        var result = VestigiumLogArchive.ArchiveMiniLogs(TimeSpan.FromDays(7), _dst, _src, "PingIQ", "PathPing");
        Assert.Equal(2, result.Archived);
        Assert.Equal(2, result.Deleted);
        Assert.True(File.Exists(Path.Combine(_src, "PingIQ.jsonl")));
        Assert.True(File.Exists(Path.Combine(_src, "PingIQ.TraceRt.jsonl")));
        Assert.False(File.Exists(Path.Combine(_src, "PingIQ.PathPing.jsonl")));
        Assert.True(File.Exists(Path.Combine(_dst, "PingIQ.PathPing.jsonl")));
        Assert.True(File.Exists(Path.Combine(_dst, "PingIQ.PathPing.jsonl.sha256")));
        var sidecar = File.ReadAllText(Path.Combine(_dst, "PingIQ.PathPing.jsonl.sha256"));
        Assert.Matches("^[0-9A-F]{64}  PingIQ.PathPing.jsonl$", sidecar.Trim());
    }

    [Fact]
    public void Disabled_by_default_leaves_files()
    {
        Touch(Path.Combine(_src, "PingIQ.PathPing.jsonl"), DateTime.UtcNow.AddDays(-30));
        Assert.True(File.Exists(Path.Combine(_src, "PingIQ.PathPing.jsonl")));
    }

    public void Dispose()
    {
        try { Directory.Delete(_src, true); } catch { }
        try { Directory.Delete(_dst, true); } catch { }
    }

    private static void Touch(string path, DateTime utc)
    {
        File.WriteAllText(path, "{}\n");
        File.SetLastWriteTimeUtc(path, utc);
    }
}
