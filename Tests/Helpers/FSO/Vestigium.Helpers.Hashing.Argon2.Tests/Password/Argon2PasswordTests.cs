using System.Security.Cryptography;
using Vestigium.Helpers.Hashing;
using Vestigium.Logging;

namespace Vestigium.Helpers.Hashing.Argon2.Tests;

[Collection("Logger")]
[Trait("Category", "Argon2")]
public sealed class Argon2PasswordTests
{
    public Argon2PasswordTests() => VestigiumLogger.Shutdown();

    [Fact]
    public void HashPassword_is_not_hashstring_and_verify_roundtrips()
    {
        const string password = "gallery-demo-only";
        var phc = HashingHelper.HashPassword(password);
        Assert.StartsWith("$argon2id$", phc);
        Assert.NotEqual(HashingHelper.HashString(password), phc);
        Assert.True(HashingHelper.VerifyPassword(password, phc));
        Assert.False(HashingHelper.VerifyPassword("gallery-demo-xxxx", phc));
        Assert.False(HashingHelper.VerifyPassword(password, "not-a-phc"));
    }

    [Fact]
    public void Password_argon2id_phc_round_trips_and_rejects_wrong()
    {
        var stored = HashingHelper.HashPassword("gallery-demo-only");
        Assert.StartsWith("$argon2id$v=19$m=19456,t=2,p=1$", stored);
        Assert.True(HashingHelper.VerifyPassword("gallery-demo-only", stored));
        Assert.False(HashingHelper.VerifyPassword("gallery-demo-xx", stored));
        Assert.Throws<ArgumentException>(() => HashingHelper.HashPassword("short"));
        Assert.False(HashingHelper.VerifyPassword("gallery-demo-only", "not-a-phc"));
    }

    [Fact]
    public void Password_verifier_rejects_each_phc_failure_arm()
    {
        const string pwd = "gallery-demo-only";
        Assert.False(HashingHelper.VerifyPassword(pwd, "$argon2i$v=19$m=19456,t=2,p=1$c2FsdHNhbHQ$c2FsdHNhbHRzYWx0c2FsdHNhbHRzYWx0c2FsdA"));
        Assert.False(HashingHelper.VerifyPassword(pwd, "$argon2id$v=16$m=19456,t=2,p=1$c2FsdHNhbHQ$c2FsdHNhbHRzYWx0c2FsdHNhbHRzYWx0c2FsdA"));
        Assert.False(HashingHelper.VerifyPassword(pwd, "$argon2id$v=19$m19456$c2FsdHNhbHQ$c2FsdHNhbHRzYWx0c2FsdHNhbHRzYWx0c2FsdA"));
        Assert.False(HashingHelper.VerifyPassword(pwd, "$argon2id$v=19$m=0,t=2,p=1$c2FsdHNhbHQ$c2FsdHNhbHRzYWx0c2FsdHNhbHRzYWx0c2FsdA"));
        Assert.False(HashingHelper.VerifyPassword(pwd, "$argon2id$v=19$m=19456,t=2,p=1,q=1$c2FsdHNhbHQ$c2FsdHNhbHRzYWx0c2FsdHNhbHRzYWx0c2FsdA"));
        Assert.False(HashingHelper.VerifyPassword(pwd, "$argon2id$v=19$m=19456,p=1$c2FsdHNhbHQ$c2FsdHNhbHRzYWx0c2FsdHNhbHRzYWx0c2FsdA"));
        Assert.False(HashingHelper.VerifyPassword(pwd, "$argon2id$v=19$t=2,p=1$c2FsdHNhbHQ$c2FsdHNhbHRzYWx0c2FsdHNhbHRzYWx0c2FsdA"));
        Assert.False(HashingHelper.VerifyPassword(pwd, "$argon2id$v=19$m=19456,t=2$c2FsdHNhbHQ$c2FsdHNhbHRzYWx0c2FsdHNhbHRzYWx0c2FsdA"));
        Assert.False(HashingHelper.VerifyPassword(pwd, "$argon2id$v=19$m=19456,t=2,p=1$%%%$c2FsdHNhbHRzYWx0c2FsdHNhbHRzYWx0c2FsdA"));

        var salt8 = Convert.ToBase64String(new byte[8]).TrimEnd('=');
        var salt4 = Convert.ToBase64String(new byte[4]).TrimEnd('=');
        var hash32 = Convert.ToBase64String(new byte[32]).TrimEnd('=');
        var hash16 = Convert.ToBase64String(new byte[16]).TrimEnd('=');
        Assert.False(HashingHelper.VerifyPassword(pwd, $"$argon2id$v=19$m=19456,t=2,p=1${salt4}${hash32}"));
        Assert.False(HashingHelper.VerifyPassword(pwd, $"$argon2id$v=19$m=19456,t=2,p=1${salt8}${hash16}"));

        Assert.Throws<CryptographicException>(() => HashingHelper.VerifyPassword(pwd, $"$argon2id$v=19$m=65537,t=2,p=1${salt8}${hash32}"));
        Assert.Throws<CryptographicException>(() => HashingHelper.VerifyPassword(pwd, $"$argon2id$v=19$m=19456,t=11,p=1${salt8}${hash32}"));
        Assert.Throws<CryptographicException>(() => HashingHelper.VerifyPassword(pwd, $"$argon2id$v=19$m=19456,t=2,p=5${salt8}${hash32}"));
        Assert.Throws<CryptographicException>(() => HashingHelper.VerifyPassword(pwd, $"$argon2id$v=19$m=7,t=2,p=1${salt8}${hash32}"));

        Assert.Throws<ArgumentException>(() => HashingHelper.HashPassword(new string('x', 129)));
        Assert.Throws<ArgumentException>(() => HashingHelper.VerifyPassword("short", $"$argon2id$v=19$m=19456,t=2,p=1${salt8}${hash32}"));
    }

    [Fact]
    public void HashPassword_too_short_is_logged_as_failed()
    {
        Assert.Throws<ArgumentException>(() => HashingHelper.HashPassword(""));
        Assert.False(HashingHelper.VerifyPassword("gallery-demo-only", ""));
    }

    [Fact]
    public void VerifyPassword_garbage_phc_returns_false()
    {
        try
        {
            Init();
            const string pwd = "gallery-demo-only";
            Assert.False(HashingHelper.VerifyPassword(pwd, "not-a-phc"));
            Assert.False(HashingHelper.VerifyPassword(pwd, ""));
            Assert.False(HashingHelper.VerifyPassword(pwd, "$argon2i$v=19$m=19456,t=2,p=1$c2FsdHNhbHQ$c2FsdHNhbHRzYWx0c2FsdHNhbHRzYWx0c2FsdA"));
            Assert.False(HashingHelper.VerifyPassword(pwd, "$argon2id$v=16$m=19456,t=2,p=1$c2FsdHNhbHQ$c2FsdHNhbHRzYWx0c2FsdHNhbHRzYWx0c2FsdA"));
            VestigiumLogger.Flush();
            Assert.Contains(VestigiumLogger.RecentJsonLines, line => line.Contains("\"EVENTID\":13020"));
        }
        finally
        {
            VestigiumLogger.Shutdown();
        }
    }

    [Fact]
    public void VerifyPassword_parameter_bomb_throws()
    {
        var salt8 = Convert.ToBase64String(new byte[8]).TrimEnd('=');
        var hash32 = Convert.ToBase64String(new byte[32]).TrimEnd('=');
        Assert.Throws<CryptographicException>(() => HashingHelper.VerifyPassword("correct horse", $"$argon2id$v=19$m=65537,t=2,p=1${salt8}${hash32}"));
        Assert.Throws<CryptographicException>(() => HashingHelper.VerifyPassword("correct horse", $"$argon2id$v=19$m=19456,t=11,p=1${salt8}${hash32}"));
        Assert.Throws<CryptographicException>(() => HashingHelper.VerifyPassword("correct horse", $"$argon2id$v=19$m=19456,t=2,p=5${salt8}${hash32}"));
        Assert.Throws<CryptographicException>(() => HashingHelper.VerifyPassword("correct horse", $"$argon2id$v=19$m=7,t=2,p=1${salt8}${hash32}"));
    }

    [Fact]
    public void HashPassword_length_still_throws()
    {
        var salt8 = Convert.ToBase64String(new byte[8]).TrimEnd('=');
        var hash32 = Convert.ToBase64String(new byte[32]).TrimEnd('=');
        Assert.Throws<ArgumentException>(() => HashingHelper.HashPassword(""));
        Assert.Throws<ArgumentException>(() => HashingHelper.HashPassword(new string('x', 129)));
        Assert.Throws<ArgumentException>(() => HashingHelper.VerifyPassword("short", $"$argon2id$v=19$m=19456,t=2,p=1${salt8}${hash32}"));
    }

    [Fact]
    public void Logs_never_contain_password_phc_or_hmac_key()
    {
        var dir = Path.Combine(Path.GetTempPath(), "VestigiumHashArgon2", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        VestigiumLogger.Initialize(cfg =>
        {
            cfg.AppId = HashingCatalog.AppId;
            cfg.LogDirectory = dir;
            cfg.MinimumDiskLevel = VestigiumLogLevel.Debug;
            cfg.OperationsLogEnabled = false;
            HashingCatalog.Register(cfg);
        });
        try
        {
            var stored = HashingHelper.HashPassword("gallery-demo-only");
            using var key = HmacKey.Generate();
            var keyHex = key.ToHexLower();
            var keyB64 = key.ToBase64();
            _ = HashingHelper.HmacString("probe-message-16", key);
            _ = HashingHelper.HashString("abc");
            var blob = string.Join('\n', VestigiumLogger.RecentJsonLines);
            Assert.DoesNotContain("gallery-demo-only", blob, StringComparison.Ordinal);
            Assert.DoesNotContain(stored, blob, StringComparison.Ordinal);
            Assert.DoesNotContain("$argon2id$", blob, StringComparison.Ordinal);
            Assert.DoesNotContain(keyHex, blob, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(keyB64, blob, StringComparison.Ordinal);
            Assert.Contains("password hashed", blob, StringComparison.Ordinal);
            Assert.Contains("\"APPID\":\"Hashing\"", blob, StringComparison.Ordinal);
        }
        finally
        {
            VestigiumLogger.Shutdown();
            Directory.Delete(dir, recursive: true);
        }
    }

    private static void Init()
    {
        var dir = Path.Combine(Path.GetTempPath(), "VestigiumHashArgon2", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        VestigiumLogger.Initialize(cfg =>
        {
            cfg.AppId = HashingCatalog.AppId;
            cfg.LogDirectory = dir;
            cfg.MinimumDiskLevel = VestigiumLogLevel.Debug;
            cfg.OperationsLogEnabled = false;
            HashingCatalog.Register(cfg);
        });
    }
}
