namespace Vestigium.Logging.Tests;

[Collection("VestigiumLogger")]
public sealed class ArchiveThrownCoverageTests : IDisposable
{
    private readonly string _logs = Path.Combine(Path.GetTempPath(), "vestigium-at-" + Guid.NewGuid().ToString("N"));
    private readonly string _arc = Path.Combine(Path.GetTempPath(), "vestigium-ata-" + Guid.NewGuid().ToString("N"));
    private readonly string _key = Path.Combine(Path.GetTempPath(), "vestigium-atk", Guid.NewGuid().ToString("N"), "ring.jsonl");
    private readonly string _custom = Path.Combine(Path.GetTempPath(), "vestigium-atc-" + Guid.NewGuid().ToString("N"));

    public ArchiveThrownCoverageTests()
    {
        Directory.CreateDirectory(_logs);
        Directory.CreateDirectory(_arc);
        Directory.CreateDirectory(_custom);
    }

    [Fact]
    public void AcceptSealAllowsNoTrailerAndValid()
    {
        VestigiumLogger.Initialize(cfg =>
        {
            cfg.AppId = "PingIQ";
            cfg.LogDirectory = _logs;
            cfg.LogSealEnabled = true;
            cfg.LogSealKeyPath = _key;
            cfg.OperationsLogEnabled = false;
        });
        var plain = Path.Combine(_logs, "PingIQ.001.jsonl");
        File.WriteAllText(plain, "{\"EVENTID\":1}\n");
        File.SetLastWriteTimeUtc(plain, DateTime.UtcNow.AddDays(-30));
        var sealedFile = Path.Combine(_logs, "PingIQ.002.jsonl");
        File.WriteAllText(sealedFile, "{\"EVENTID\":1}\n");
        File.SetLastWriteTimeUtc(sealedFile, DateTime.UtcNow.AddDays(-30));
        var ring = VestigiumSealKeyRing.Open(_key);
        var bytes = File.ReadAllBytes(sealedFile);
        var sha = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(bytes));
        var sig = Convert.ToBase64String(ring.Sign(bytes));
        File.AppendAllText(sealedFile,
            "{\"VESTIGIUM_TRAILER\":1,\"KeyId\":\"" + ring.KeyId + "\",\"ContentSha256\":\"" + sha + "\",\"Sig\":\"" + sig + "\",\"LineCount\":1}\n");
        File.SetLastWriteTimeUtc(sealedFile, DateTime.UtcNow.AddDays(-30));
        var result = VestigiumLogArchive.ArchiveOlderThan(TimeSpan.FromDays(1), _arc, _logs, "PingIQ");
        Assert.True(result.Archived >= 2);
        VestigiumLogger.Shutdown();
    }

    [Fact]
    public void ArchiveIoExceptionCountsFailed()
    {
        var file = Path.Combine(_logs, "PingIQ.001.jsonl");
        File.WriteAllText(file, "{}\n");
        File.SetLastWriteTimeUtc(file, DateTime.UtcNow.AddDays(-30));
        Directory.CreateDirectory(Path.Combine(_arc, "PingIQ.001.jsonl"));
        var result = VestigiumLogArchive.ArchiveOlderThan(TimeSpan.FromDays(1), _arc, _logs, "PingIQ");
        Assert.True(result.Failed >= 1);
    }

    [Fact]
    public void ArchiveHashMismatchReturnsFalse()
    {
        var file = Path.Combine(_logs, "PingIQ.001.jsonl");
        File.WriteAllText(file, "{}\n");
        File.SetLastWriteTimeUtc(file, DateTime.UtcNow.AddDays(-30));
        VestigiumLogArchive.HashOverride = path =>
            path.StartsWith(_arc, StringComparison.OrdinalIgnoreCase) ? "DEADBEEF" : "OKHASH";
        try
        {
            var result = VestigiumLogArchive.ArchiveOlderThan(TimeSpan.FromDays(1), _arc, _logs, "PingIQ");
            Assert.True(result.Failed >= 1);
        }
        finally
        {
            VestigiumLogArchive.HashOverride = null;
        }
    }

    [Fact]
    public void ThrownUnknownSeverityFallsBackToError()
    {
        var catalog = VestigiumCustomCatalog.Open(_custom);
        catalog.Add("App.Odd", typeof(OddEx).FullName!, "System", "Lifecycle", 10_000, "NotALevel", "x");
        catalog.Save();
        VestigiumLogger.Initialize(cfg =>
        {
            cfg.AppId = "PingIQ";
            cfg.LogDirectory = _logs;
            cfg.OperationsLogEnabled = false;
        });
        VestigiumLogger.LoadCustomCatalog(_custom);
        VestigiumLog.Thrown(new OddEx("odd"), VestigiumStatus.Failed, 10_000);
        VestigiumLog.Thrown(new OddEx("odd2"), VestigiumStatus.Failed);
        VestigiumLogger.Flush();
        VestigiumLogger.Shutdown();
    }

    public void Dispose()
    {
        VestigiumLogArchive.HashOverride = null;
        VestigiumLogger.Shutdown();
        try { Directory.Delete(_logs, true); } catch { }
        try { Directory.Delete(_arc, true); } catch { }
        try { Directory.Delete(_custom, true); } catch { }
    }

    private sealed class OddEx : Exception
    {
        public OddEx(string message) : base(message) { }
    }
}
