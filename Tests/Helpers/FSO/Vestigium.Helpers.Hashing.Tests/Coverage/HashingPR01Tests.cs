using System.Security.Cryptography;
using Vestigium.Helpers.Hashing;
using Vestigium.Logging;

namespace Vestigium.Helpers.Hashing.Tests;

[Collection("Logger")]
public sealed class HashingPr01Tests
{
    public HashingPr01Tests() => VestigiumLogger.Shutdown();

    private static string Init()
    {
        var dir = Path.Combine(Path.GetTempPath(), "VestigiumHashPR01", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        VestigiumLogger.Initialize(cfg =>
        {
            cfg.AppId = HashingCatalog.AppId;
            cfg.LogDirectory = dir;
            cfg.MinimumDiskLevel = VestigiumLogLevel.Debug;
            cfg.OperationsLogEnabled = false;
            HashingCatalog.Register(cfg);
        });
        return dir;
    }
    [Fact]
    public void HmacKey_ToString_is_not_hex()
    {
        using var key = HmacKey.Generate();
        var hex = key.ToHexLower();
        var printed = key.ToString();
        Assert.Equal("HmacKey(32 bytes)", printed);
        Assert.DoesNotContain(hex, printed);
        Assert.DoesNotContain(key.ToBase64(), printed);
        key.Dispose();
        Assert.Equal("HmacKey(disposed)", key.ToString());
    }

    [Fact]
    public void Hmac_jsonl_has_keyBytes_not_key_hex()
    {
        try
        {
            Init();
            var material = new byte[32];
            for (var i = 0; i < material.Length; i++)
                material[i] = (byte)(0xA0 + i);
            using var key = HmacKey.FromBytes(material);
            var keyHex = key.ToHexLower();
            var keyB64 = key.ToBase64();
            _ = HashingHelper.HmacString("gallery-demo-only", key);
            VestigiumLogger.Flush();
            Assert.Contains(VestigiumLogger.RecentJsonLines, line => line.Contains("keyBytes=32"));
            Assert.DoesNotContain(VestigiumLogger.RecentJsonLines, line => line.Contains(keyHex, StringComparison.OrdinalIgnoreCase));
            Assert.DoesNotContain(VestigiumLogger.RecentJsonLines, line => line.Contains(keyB64));
        }
        finally
        {
            VestigiumLogger.Shutdown();
        }
    }

    [Fact]
    public void HashFile_jsonl_may_contain_digest_hex()
    {
        try
        {
            Init();
            var dir = Path.Combine(Path.GetTempPath(), "VestigiumHashPR01", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            var path = Path.Combine(dir, "payload.bin");
            File.WriteAllText(path, "abc");
            var digest = HashingHelper.HashFile(path);
            VestigiumLogger.Flush();
            Assert.Contains(VestigiumLogger.RecentJsonLines, line => line.Contains("digest=" + digest));
        }
        finally
        {
            VestigiumLogger.Shutdown();
        }
    }
}
