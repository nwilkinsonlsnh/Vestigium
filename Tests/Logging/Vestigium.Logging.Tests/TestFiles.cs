namespace Vestigium.Logging.Tests;

internal static class TestFiles
{
    public static string ReadShared(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static void Touch(string path, DateTime utc)
    {
        File.WriteAllText(path, "{}\n");
        File.SetLastWriteTimeUtc(path, utc);
    }
}
