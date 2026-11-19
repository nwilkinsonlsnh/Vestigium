namespace Vestigium.Logging;

public static class LogId
{
    public const int MinimumLetters = 4;

    public static string Validate(string? id, string paramName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id, paramName);
        if (id.IndexOfAny(['.', '/', '\\', ':', '*', '?', '"', '<', '>', '|']) >= 0)
            throw new ArgumentException($"'{id}' contains a separator or reserved character.", paramName);
        var letters = 0;
        foreach (var c in id)
        {
            if (char.IsLetter(c)) letters++;
        }
        if (letters < MinimumLetters)
            throw new ArgumentException($"'{id}' must contain at least {MinimumLetters} letters.", paramName);
        return id;
    }
}
