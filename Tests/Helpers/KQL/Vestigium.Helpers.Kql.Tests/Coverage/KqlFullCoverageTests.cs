using Vestigium.Helpers.Kql;

namespace Vestigium.Helpers.Kql.Tests.Coverage;

public sealed class KqlFullCoverageTests
{
    [Fact]
    public void Parser_rejects_empty_and_whitespace()
    {
        Assert.False(KqlHelper.Compile("", Session()).Ok);
        Assert.False(KqlHelper.Compile("   ", Session()).Ok);
    }

    [Theory]
    [InlineData("== 1")]
    [InlineData("(Name")]
    [InlineData("Name ==")]
    [InlineData("Name NOT")]
    [InlineData("Name NOT FOO")]
    [InlineData("Name IN 1")]
    [InlineData("Name IN (1")]
    [InlineData("Name BETWEEN 1")]
    [InlineData("Name BETWEEN 1 OR 2")]
    [InlineData("Name BEGINS FOO")]
    [InlineData("Name(")]
    [InlineData("Name(open")]
    [InlineData("Name.isTrue")]
    [InlineData("Name.isTrue(1)")]
    [InlineData("Count.between(1)")]
    [InlineData("Name.contains(")]
    [InlineData("NOT LIKE")]
    [InlineData("Name AND")]
    public void Parser_error_paths(string text)
    {
        using var session = Session();
        Assert.False(KqlHelper.Compile(text, session).Ok);
    }

    [Fact]
    public void Parser_bare_true_and_loose_and_word_ops()
    {
        using var session = Session();
        Assert.True(KqlHelper.Compile("Flag AND Name == \"a\"", session).Ok);
        Assert.True(KqlHelper.Compile("Flag OR Name == \"a\"", session).Ok);
        Assert.True(KqlHelper.Compile("Name \"hello\"", session).Ok);
        Assert.True(KqlHelper.Compile("Count 5", session).Ok);
        Assert.True(KqlHelper.Compile("Flag true", session).Ok);
        Assert.True(KqlHelper.Compile("Name BEGINS WITH \"a\"", session).Ok);
        Assert.True(KqlHelper.Compile("Name ENDS WITH \"a\"", session).Ok);
        Assert.True(KqlHelper.Compile("Name NOT LIKE \"a\"", session).Ok);
        Assert.True(KqlHelper.Compile("Name NOT IN (\"a\")", session).Ok);
    }

    [Fact]
    public void Parser_closed_call_null_forms()
    {
        using var session = Session();
        Assert.True(KqlHelper.Compile("Name()", session).Ok);
        Assert.True(KqlHelper.Compile("Name(NULL)", session).Ok);
        Assert.True(KqlHelper.Compile("Name(IS NULL)", session).Ok);
        Assert.True(KqlHelper.Compile("Name(IS NOT NULL)", session).Ok);
        Assert.True(KqlHelper.Compile("Name(NOT NULL)", session).Ok);
        Assert.True(KqlHelper.Compile("Name(\"\")", session).Ok);
    }

    [Fact]
    public void Parser_literals_and_constructors()
    {
        using var session = Session();
        Assert.True(KqlHelper.Compile("Count == 1.5", session).Ok);
        Assert.True(KqlHelper.Compile("Count == 1kb", session).Ok);
        Assert.True(KqlHelper.Compile("When == date(2026-11-15)", session).Ok);
        Assert.True(KqlHelper.Compile("Span == 5s", session).Ok);
        Assert.True(KqlHelper.Compile("Addr == ipaddress(10.1.2.3)", session).Ok);
        Assert.True(KqlHelper.Compile("Mac == macaddress(00:11:22:33:44:55)", session).Ok);
        Assert.False(KqlHelper.Compile("Name == string(", session).Ok);
        Assert.False(KqlHelper.Compile("When == date(not-a-date)", session).Ok);
        Assert.False(KqlHelper.Compile("Count == 1x", session).Ok);
    }

    [Fact]
    public void Evaluator_logical_unknown_and_null_ops()
    {
        using var session = Session();
        var empty = new KqlFixtureRow(session);
        var full = new KqlFixtureRow(session).Set("Name", "hello").Set("Count", 5L).Set("Flag", true);

        AssertTri(session, empty, "Name == \"hello\" AND Count == 5", KqlTriState.Unknown);
        AssertTri(session, empty, "Name == \"hello\" OR Count == 5", KqlTriState.Unknown);
        AssertTri(session, full, "Name == \"hello\" AND Count == 0", KqlTriState.False);
        AssertTri(session, full, "Name == \"x\" OR Count == 5", KqlTriState.True);
        AssertTri(session, empty, "NOT Name == \"hello\"", KqlTriState.Unknown);
        AssertTri(session, full, "NOT Flag == true", KqlTriState.False);
        AssertTri(session, full, "NOT Flag == false", KqlTriState.True);

        AssertTri(session, empty, "Name()", KqlTriState.True);
        AssertTri(session, empty, "Name(NULL)", KqlTriState.True);
        AssertTri(session, full, "Name()", KqlTriState.False);
        AssertTri(session, full, "Name(NULL)", KqlTriState.False);
    }

    [Fact]
    public void Evaluator_ip_and_mac_edges()
    {
        using var session = Session();
        var row = new KqlFixtureRow(session)
            .Set("Addr", "10.1.2.3")
            .Set("Mac", "00:11:22:33:44:55");

        AssertTri(session, row, "Addr == ipaddress(10.1.2.3)", KqlTriState.True);
        AssertTri(session, row, "Addr != ipaddress(10.1.2.4)", KqlTriState.True);
        AssertTri(session, row, "Addr BEGINS WITH ipaddress(10.1)", KqlTriState.True);
        AssertTri(session, row, "Addr ENDS WITH ipaddress(2.3)", KqlTriState.True);
        AssertTri(session, row, "Addr CONTAINS ipaddress(1.2)", KqlTriState.True);

        AssertTri(session, row, "Mac == macaddress(00:11:22:33:44:55)", KqlTriState.True);
        AssertTri(session, row, "Mac != macaddress(00:11:22:33:44:56)", KqlTriState.True);
        AssertTri(session, row, "Mac BEGINS WITH macaddress(00:11)", KqlTriState.True);
        AssertTri(session, row, "Mac ENDS WITH macaddress(44:55)", KqlTriState.True);
        AssertTri(session, row, "Mac CONTAINS macaddress(22:33)", KqlTriState.True);
    }

    [Fact]
    public void Evaluator_in_between_and_conversions()
    {
        using var session = Session();
        var row = new KqlFixtureRow(session)
            .Set("Name", "hello")
            .Set("Count", 5L)
            .Set("Flag", true)
            .Set("When", new DateTime(2026, 11, 15))
            .Set("Span", TimeSpan.FromSeconds(5));

        AssertTri(session, row, "Name IN (\"hello\", \"x\")", KqlTriState.True);
        AssertTri(session, row, "Name IN (\"a\", \"b\")", KqlTriState.False);
        AssertTri(session, row, "Name NOT IN (\"hello\")", KqlTriState.False);
        AssertTri(session, row, "Name NOT IN (\"a\")", KqlTriState.True);
        AssertTri(session, row, "Count BETWEEN 1 AND 9", KqlTriState.True);
        AssertTri(session, row, "Count BETWEEN 10 AND 20", KqlTriState.False);

        var empty = new KqlFixtureRow(session);
        AssertTri(session, empty, "Name IN (\"hello\")", KqlTriState.Unknown);
        AssertTri(session, empty, "Count BETWEEN 1 AND 9", KqlTriState.Unknown);

        AssertTri(session, row, "Flag == true", KqlTriState.True);
        AssertTri(session, row, "Span == 5s", KqlTriState.True);
        AssertTri(session, row, "When == date(2026-11-15)", KqlTriState.True);
    }

    [Fact]
    public void Evaluator_like_and_text_span()
    {
        using var session = Session();
        var row = new KqlFixtureRow(session).Set("Name", "Hello World");
        AssertTri(session, row, "Name LIKE \"Hello*\"", KqlTriState.True);
        AssertTri(session, row, "Name NOT LIKE \"Hello*\"", KqlTriState.False);
        AssertTri(session, row, "Name BEGINS WITH \"hello\"", KqlTriState.True);
        AssertTri(session, row, "Name ENDS WITH \"world\"", KqlTriState.True);
        AssertTri(session, row, "Name CONTAINS \"lo Wo\"", KqlTriState.True);
        AssertTri(session, row, "Name CONTAINS \"xyz\"", KqlTriState.False);
    }

    [Fact]
    public void Binder_closed_require_and_range()
    {
        using var session = KqlHelper.Create(
        [
            KqlField.Create("Status", KqlType.String, closed: ["open", "closed"]),
            KqlField.Create("Port", KqlType.Integer, minimum: 1, maximum: 65535)
        ]);

        Assert.True(KqlHelper.Compile("Status == open", session).Ok);
        Assert.True(KqlHelper.Compile("Status(open)", session).Ok);
        Assert.False(KqlHelper.Compile("Status == other", session).Ok);
        Assert.False(KqlHelper.Compile("Status(other)", session).Ok);
        Assert.True(KqlHelper.Compile("Port == 80", session).Ok);
        Assert.False(KqlHelper.Compile("Port == 0", session).Ok);
        Assert.False(KqlHelper.Compile("Port == 70000", session).Ok);
        Assert.False(KqlHelper.Compile("Unknown == 1", session).Ok);
    }

    [Fact]
    public void Binder_ip_mac_validation_and_type_checks()
    {
        using var session = Session();
        Assert.False(KqlHelper.Compile("Addr == ipaddress(10.1)", session).Ok);
        Assert.False(KqlHelper.Compile("Addr == ipaddress(10.1.2.3.4)", session).Ok);
        Assert.False(KqlHelper.Compile("Addr == ipaddress(999.1.2.3)", session).Ok);
        Assert.False(KqlHelper.Compile("Addr == ipaddress()", session).Ok);
        Assert.True(KqlHelper.Compile("Addr CONTAINS ipaddress(10.1)", session).Ok);

        Assert.False(KqlHelper.Compile("Mac == macaddress(00:11)", session).Ok);
        Assert.False(KqlHelper.Compile("Mac == macaddress(zz)", session).Ok);
        Assert.False(KqlHelper.Compile("Mac == macaddress()", session).Ok);
        Assert.True(KqlHelper.Compile("Mac CONTAINS macaddress(22:33)", session).Ok);
        Assert.False(KqlHelper.Compile("Mac CONTAINS macaddress(z)", session).Ok);

        Assert.False(KqlHelper.Compile("Count LIKE \"a\"", session).Ok);
        Assert.False(KqlHelper.Compile("Name == 1", session).Ok);
    }

    [Fact]
    public void Binder_wildcard_warning()
    {
        using var session = Session();
        var result = KqlHelper.Compile("Name == \"hello*\"", session);
        Assert.True(result.Ok);
        Assert.NotEmpty(result.Diagnostics);
    }

    [Fact]
    public void Completer_slots_and_edges()
    {
        using var session = Session();
        Assert.Empty(KqlHelper.Complete("x", -1, session).Rows);
        Assert.NotEmpty(KqlHelper.Complete("", 0, session).Rows);
        Assert.NotEmpty(KqlHelper.Complete("Na", 2, session).Rows);
        Assert.NotEmpty(KqlHelper.Complete("Name ", 5, session).Rows);
        Assert.NotEmpty(KqlHelper.Complete("Name == ", 8, session).Rows);
        Assert.NotEmpty(KqlHelper.Complete("Name.", 5, session).Rows);
        Assert.NotEmpty(KqlHelper.Complete("Name.c", 6, session).Rows);
        Assert.NotEmpty(KqlHelper.Complete("Flag ", 5, session).Rows);
        Assert.NotEmpty(KqlHelper.Complete("Addr ", 5, session).Rows);
        Assert.NotEmpty(KqlHelper.Complete("Count ", 6, session).Rows);
        Assert.NotEmpty(KqlHelper.Complete("When ", 5, session).Rows);
        Assert.NotEmpty(KqlHelper.Complete("Span ", 5, session).Rows);
        Assert.Empty(KqlHelper.Complete("Name == ipaddress(", 19, session).Rows);
        Assert.Empty(KqlHelper.Complete("x", 5, session).Rows);
    }

    [Fact]
    public void Methods_allows_and_for()
    {
        Assert.True(KqlMethods.Allows("contains", KqlType.String, KqlCompareAs.Stored));
        Assert.True(KqlMethods.Allows("isEmpty", KqlType.String, KqlCompareAs.Stored));
        Assert.True(KqlMethods.Allows("isTrue", KqlType.Boolean, KqlCompareAs.Stored));
        Assert.True(KqlMethods.Allows("after", KqlType.DateTime, KqlCompareAs.Stored));
        Assert.True(KqlMethods.Allows("between", KqlType.Integer, KqlCompareAs.Stored));
        Assert.True(KqlMethods.Allows("greaterThan", KqlType.Number, KqlCompareAs.Stored));
        Assert.True(KqlMethods.Allows("equals", KqlType.String, KqlCompareAs.Stored));
        Assert.False(KqlMethods.Allows("nope", KqlType.String, KqlCompareAs.Stored));
        Assert.True(KqlMethods.Allows("contains", KqlType.String, KqlCompareAs.IpAddress));

        Assert.Contains("contains", KqlMethods.For(KqlType.String, KqlCompareAs.Stored));
        Assert.Contains("equals", KqlMethods.For(KqlType.Char, KqlCompareAs.Stored));
        Assert.Contains("isTrue", KqlMethods.For(KqlType.Boolean, KqlCompareAs.Stored));
        Assert.Contains("greaterThan", KqlMethods.For(KqlType.Integer, KqlCompareAs.Stored));
        Assert.Contains("after", KqlMethods.For(KqlType.DateTime, KqlCompareAs.Stored));
        Assert.Contains("equals", KqlMethods.For(KqlType.String, KqlCompareAs.IpAddress));
    }

    [Fact]
    public void Lexer_escapes_and_errors()
    {
        using var session = Session();
        Assert.True(KqlHelper.Compile("Name == \"hello\\nworld\"", session).Ok);
        Assert.True(KqlHelper.Compile("Name == \"hello\\tworld\"", session).Ok);
        Assert.True(KqlHelper.Compile("Name == \"hello\\\"world\"", session).Ok);
        Assert.False(KqlHelper.Compile("Name == \"unterminated", session).Ok);
        Assert.False(KqlHelper.Compile("Name == @bad", session).Ok);
        Assert.True(KqlHelper.Compile("Name == 'single'", session).Ok);
    }

    [Fact]
    public void Value_from_all_types()
    {
        Assert.True(KqlValue.From(null).IsUnknown);
        Assert.True(KqlValue.From("").IsUnknown);
        Assert.True(KqlValue.From("   ").IsUnknown);
        Assert.Equal(KqlType.String, KqlValue.From("hello").Type);
        Assert.Equal(KqlType.Boolean, KqlValue.From(true).Type);
        Assert.Equal(KqlType.TimeSpan, KqlValue.From(TimeSpan.FromSeconds(1)).Type);
        Assert.Equal(KqlType.DateTime, KqlValue.From(DateTime.Now).Type);
        Assert.Equal(KqlType.DateTime, KqlValue.From(DateTimeOffset.Now).Type);
        Assert.Equal(KqlType.Integer, KqlValue.From(1).Type);
        Assert.Equal(KqlType.Integer, KqlValue.From(1L).Type);
        Assert.Equal(KqlType.Integer, KqlValue.From((byte)1).Type);
        Assert.Equal(KqlType.Number, KqlValue.From(1.5).Type);
        Assert.Equal(KqlType.Number, KqlValue.From(1.5f).Type);
        Assert.Equal(KqlType.Number, KqlValue.From(1.5m).Type);
        Assert.Equal(KqlType.String, KqlValue.From(Guid.NewGuid()).Type);
    }

    [Fact]
    public void Units_and_dates_and_booleans_edges()
    {
        Assert.True(KqlUnits.TryParse("1kb", out _));
        Assert.True(KqlUnits.TryParse("2mb", out _));
        Assert.True(KqlUnits.TryParse("3gb", out _));
        Assert.True(KqlUnits.TryParse("1tb", out _));
        Assert.True(KqlUnits.TryParse("8b", out _));
        Assert.False(KqlUnits.TryParse("", out _));
        Assert.False(KqlUnits.TryParse("1x", out _));
        Assert.False(KqlUnits.TryParse("kb", out _));

        Assert.True(KqlDates.TryParse("2026-11-15", out _));
        Assert.False(KqlDates.TryParse("", out _));
        Assert.False(KqlDates.TryParse("not-a-date", out _));

        Assert.True(KqlBooleans.TryParse("true", out var b) && b);
        Assert.True(KqlBooleans.TryParse("false", out b) && !b);
        Assert.False(KqlBooleans.TryParse("", out _));
        Assert.False(KqlBooleans.TryParse("maybe", out _));
    }

    private static void AssertTri(KqlSession session, KqlFixtureRow row, string text, KqlTriState expected)
    {
        var compiled = KqlHelper.Compile(text, session);
        Assert.True(compiled.Ok, compiled.Error?.Message);
        Assert.Equal(expected, compiled.Query!.Evaluate(row));
    }

    private static KqlSession Session()
        => KqlHelper.Create(
        [
            KqlField.Create("Name", KqlType.String),
            KqlField.Create("Count", KqlType.Integer),
            KqlField.Create("Flag", KqlType.Boolean),
            KqlField.Create("When", KqlType.DateTime),
            KqlField.Create("Span", KqlType.TimeSpan),
            KqlField.Create("Addr", KqlType.String, compareAs: KqlCompareAs.IpAddress),
            KqlField.Create("Mac", KqlType.String, compareAs: KqlCompareAs.MacAddress)
        ]);
}
