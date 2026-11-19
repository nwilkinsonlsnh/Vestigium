namespace Vestigium.Logging.Tests;

public sealed class LogIdTests
{
    [Theory]
    [InlineData("PingIQ")]
    [InlineData("pingiq")]
    [InlineData("0AB1C2D")]
    [InlineData("PathPing")]
    public void Accepts_four_or_more_letters(string id) =>
        Assert.Equal(id, LogId.Validate(id, nameof(id)));

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("001")]
    [InlineData("AB1")]
    [InlineData("abc")]
    [InlineData("PingIQ.PathPing")]
    [InlineData("Ping/IQ")]
    public void Rejects_short_pure_digit_and_separator(string id) =>
        Assert.Throws<ArgumentException>(() => LogId.Validate(id, nameof(id)));

    [Fact]
    public void Rejects_null() =>
        Assert.Throws<ArgumentNullException>(() => LogId.Validate(null, "id"));
}
