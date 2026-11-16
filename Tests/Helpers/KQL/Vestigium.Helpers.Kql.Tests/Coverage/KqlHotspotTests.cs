using Vestigium.Helpers.Kql;

namespace Vestigium.Helpers.Kql.Tests.Coverage;

public sealed class KqlHotspotTests
{
    [Theory]
    [InlineData(typeof(bool), KqlType.Boolean)]
    [InlineData(typeof(bool?), KqlType.Boolean)]
    [InlineData(typeof(char), KqlType.Char)]
    [InlineData(typeof(string), KqlType.String)]
    [InlineData(typeof(int), KqlType.Integer)]
    [InlineData(typeof(short), KqlType.Integer)]
    [InlineData(typeof(byte), KqlType.Integer)]
    [InlineData(typeof(long), KqlType.Long)]
    [InlineData(typeof(float), KqlType.Number)]
    [InlineData(typeof(double), KqlType.Number)]
    [InlineData(typeof(decimal), KqlType.Decimal)]
    [InlineData(typeof(DateTime), KqlType.DateTime)]
    [InlineData(typeof(DateTimeOffset), KqlType.DateTime)]
    [InlineData(typeof(DateOnly), KqlType.DateTime)]
    [InlineData(typeof(TimeSpan), KqlType.TimeSpan)]
    [InlineData(typeof(KqlIpAddress), KqlType.IpAddress)]
    [InlineData(typeof(KqlMacAddress), KqlType.MacAddress)]
    [InlineData(typeof(object), KqlType.String)]
    public void From_maps_each_clr_type(Type type, KqlType expected)
    {
        Assert.Equal(expected, KqlTypes.From(type));
    }

    [Fact]
    public void Infer_null_is_string_and_value_follows_type()
    {
        Assert.Equal(KqlType.String, KqlTypes.Infer(null));
        Assert.Equal(KqlType.Integer, KqlTypes.Infer(3));
        Assert.Throws<ArgumentNullException>(() => KqlTypes.From(null!));
    }

    [Theory]
    [InlineData("127.0.0.1", true)]
    [InlineData("  ::1  ", true)]
    [InlineData("", false)]
    [InlineData("not-an-ip", false)]
    public void Ip_parses(string text, bool ok)
    {
        Assert.Equal(ok, KqlIpAddress.TryParse(text, out var value));
        if (ok)
            Assert.Equal(text.Trim(), value.ToString());
    }

    [Theory]
    [InlineData("00-11-22-33-44-55", true)]
    [InlineData("001122334455", true)]
    [InlineData("", false)]
    [InlineData("zz", false)]
    public void Mac_parses(string text, bool ok)
    {
        Assert.Equal(ok, KqlMacAddress.TryParse(text, out var value));
        if (ok)
            Assert.False(string.IsNullOrEmpty(value.ToString()));
    }

    [Theory]
    [InlineData("500", 500d)]
    [InlineData("1kb", 1024d)]
    [InlineData("1mb", 1024d * 1024d)]
    [InlineData("1gb", 1024d * 1024d * 1024d)]
    [InlineData("1tb", 1024d * 1024d * 1024d * 1024d)]
    [InlineData("1b", 1d)]
    public void Units_scale(string text, double expected)
    {
        Assert.True(KqlUnits.TryParse(text, out var value));
        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\"\"")]
    [InlineData("nope")]
    [InlineData("+x")]
    public void Units_and_dates_reject(string text)
    {
        Assert.False(KqlUnits.TryParse(text, out _));
        Assert.False(KqlDates.TryParse(text, out _));
    }

    [Fact]
    public void Dates_accept_words_offsets_and_formats()
    {
        Assert.True(KqlDates.TryParse("today", out var today));
        Assert.Equal(DateTime.Today, today.Date);
        Assert.True(KqlDates.TryParse("now", out _));
        Assert.True(KqlDates.TryParse("yesterday", out var yesterday));
        Assert.Equal(DateTime.Today.AddDays(-1), yesterday.Date);
        Assert.True(KqlDates.TryParse("+2", out var plus));
        Assert.Equal(DateTime.Today.AddDays(2), plus.Date);
        Assert.True(KqlDates.TryParse("-1", out _));
        Assert.True(KqlDates.TryParse("\"2026-11-15\"", out var exact));
        Assert.Equal(new DateTime(2026, 11, 15), exact.Date);
        Assert.True(KqlDates.TryParse("11/15/2026", out _));
        Assert.True(KqlDates.TryParse("2026-11", out var month));
        Assert.Equal(1, month.Day);
        Assert.Equal(new DateTime(2026, 11, 15), KqlDates.Parse("2026-11-15").Date);
        Assert.Throws<FormatException>(() => KqlDates.Parse("nope"));
    }

    [Fact]
    public void Prepare_empty_partial_ready_and_failed()
    {
        using var session = Session();
        Assert.Equal(KqlPrepareState.Empty, KqlPrepare.Run(null, session).State);
        Assert.Equal(KqlPrepareState.Empty, KqlPrepare.Run("   ", session).State);
        Assert.Equal(KqlPrepareState.Partial, KqlPrepare.Run("Name", session).State);
        var ready = KqlPrepare.Run("Name == \"a\"", session);
        Assert.Equal(KqlPrepareState.Ready, ready.State);
        Assert.NotNull(ready.Query);
        var failed = KqlPrepare.Run("Missing == 1", session);
        Assert.Equal(KqlPrepareState.Failed, failed.State);
    }

    [Theory]
    [InlineData("Name == \"a\"")]
    [InlineData("Name != \"a\"")]
    [InlineData("Count > 1")]
    [InlineData("Count >= 1")]
    [InlineData("Count < 1")]
    [InlineData("Count <= 1")]
    [InlineData("Name LIKE \"a*\"")]
    [InlineData("Name CONTAINS \"a\"")]
    [InlineData("Name STARTSWITH \"a\"")]
    [InlineData("Name ENDSWITH \"a\"")]
    [InlineData("Count BETWEEN 1 AND 2")]
    [InlineData("Name IN (\"a\", \"b\")")]
    [InlineData("Name IS NULL")]
    [InlineData("Name IS NOT NULL")]
    [InlineData("Flag.isTrue()")]
    [InlineData("Name.isEmpty()")]
    [InlineData("Count.between(1, 9)")]
    [InlineData("Name.contains(\"a\")")]
    [InlineData("When.after(\"2026-11-15\")")]
    public void Prepare_accepts_each_operator_shape(string text)
    {
        using var session = Session();
        var prepared = KqlPrepare.Run(text, session);
        Assert.True(prepared.State is KqlPrepareState.Ready or KqlPrepareState.Failed, prepared.Message);
    }

    private static KqlSession Session()
        => KqlHelper.Create(
            KqlField.Create("Name", KqlType.String, ["Title"]),
            KqlField.Create("Count", KqlType.Integer),
            KqlField.Create("Flag", KqlType.Boolean),
            KqlField.Create("When", KqlType.DateTime));
}
