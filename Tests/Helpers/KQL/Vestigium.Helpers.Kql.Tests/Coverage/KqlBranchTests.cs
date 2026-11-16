using Vestigium.Helpers.Kql;

namespace Vestigium.Helpers.Kql.Tests.Coverage;

public sealed class KqlBranchTests
{
    [Theory]
    [InlineData("Name()")]
    [InlineData("Name(NULL)")]
    [InlineData("Name(IS NULL)")]
    [InlineData("Name(IS NOT NULL)")]
    [InlineData("Name(NOT NULL)")]
    [InlineData("Name(\"\")")]
    public void Closed_call_forms_parse(string text)
    {
        using var session = Session();
        var result = KqlHelper.Compile(text, session);
        Assert.True(result.Ok, result.Error?.Message);
    }

    [Theory]
    [InlineData("Name \"hello\"")]
    [InlineData("Count 5")]
    [InlineData("Flag true")]
    [InlineData("Flag yes")]
    [InlineData("Flag AND Name == \"a\"")]
    public void Loose_and_bare_forms_parse(string text)
    {
        using var session = Session();
        Assert.True(KqlHelper.Compile(text, session).Ok);
    }

    [Theory]
    [InlineData("Name == \"a\"")]
    [InlineData("Name != \"a\"")]
    [InlineData("Count > 1")]
    [InlineData("Count < 1")]
    [InlineData("Count >= 1")]
    [InlineData("Count <= 1")]
    [InlineData("Count GT 1")]
    [InlineData("Count LT 1")]
    [InlineData("Count GTE 1")]
    [InlineData("Count LTE 1")]
    [InlineData("Name CONTAINS \"a\"")]
    [InlineData("Name BEGINS WITH \"a\"")]
    [InlineData("Name ENDS WITH \"a\"")]
    [InlineData("Name LIKE \"a*\"")]
    [InlineData("Name NOT LIKE \"a*\"")]
    [InlineData("Name IN (\"a\", \"b\")")]
    [InlineData("Name NOT IN (\"a\")")]
    [InlineData("Count BETWEEN 1 AND 9")]
    [InlineData("Flag.isTrue()")]
    [InlineData("Flag.isFalse()")]
    [InlineData("Name.isEmpty()")]
    [InlineData("Name.contains(\"a\")")]
    [InlineData("Name.startsWith(\"a\")")]
    [InlineData("Name.endsWith(\"a\")")]
    [InlineData("Name.equals(\"a\")")]
    [InlineData("Name.like(\"a*\")")]
    [InlineData("Count.greaterThan(1)")]
    [InlineData("Count.greaterThanOrEqual(1)")]
    [InlineData("Count.lessThan(1)")]
    [InlineData("Count.lessThanOrEqual(1)")]
    [InlineData("Count.between(1, 9)")]
    [InlineData("When.after(\"2026-11-15\")")]
    [InlineData("When.before(\"2026-11-15\")")]
    [InlineData("When.on(\"2026-11-15\")")]
    [InlineData("Count == 1kb")]
    [InlineData("Count == 2mb")]
    [InlineData("Count == 3gb")]
    [InlineData("Count == 1tb")]
    [InlineData("Count == 8b")]
    [InlineData("Span == 5ms")]
    [InlineData("Span == 5s")]
    [InlineData("Span == 5m")]
    [InlineData("Span == 5h")]
    [InlineData("Span == 5d")]
    [InlineData("Addr == ipaddress(10.1.2.3)")]
    [InlineData("Mac == macaddress(00:11:22:33:44:55)")]
    public void Every_operator_and_literal_form_parses(string text)
    {
        using var session = Session();
        var result = KqlHelper.Compile(text, session);
        Assert.True(result.Ok, result.Error?.Message);
    }

    [Theory]
    [InlineData("2026-11-15")]
    [InlineData("2026/11/15")]
    [InlineData("20261115")]
    [InlineData("11/15/2026")]
    [InlineData("11/5/2026")]
    [InlineData("15-Nov-2026")]
    [InlineData("5-Nov-2026")]
    [InlineData("15Nov2026")]
    [InlineData("5Nov2026")]
    [InlineData("15 Nov 2026")]
    [InlineData("15 November 2026")]
    [InlineData("Nov 15 2026")]
    [InlineData("November 15 2026")]
    [InlineData("Nov 15, 2026")]
    [InlineData("November 15, 2026")]
    [InlineData("Nov 2026")]
    [InlineData("November 2026")]
    [InlineData("2026-11")]
    [InlineData("2026/11")]
    public void Every_date_format_parses(string text)
    {
        Assert.True(KqlDates.TryParse(text, out _));
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("t", true)]
    [InlineData("yes", true)]
    [InlineData("y", true)]
    [InlineData("1", true)]
    [InlineData("enabled", true)]
    [InlineData("false", false)]
    [InlineData("f", false)]
    [InlineData("no", false)]
    [InlineData("n", false)]
    [InlineData("0", false)]
    [InlineData("disabled", false)]
    [InlineData("", false)]
    [InlineData("maybe", false)]
    public void Boolean_words(string text, bool expected)
    {
        var ok = KqlBooleans.TryParse(text, out var value);
        Assert.Equal(expected || text is "false" or "f" or "no" or "n" or "0" or "disabled", ok);
        if (ok)
            Assert.Equal(expected, value);
    }

    [Fact]
    public void Closed_values_are_enforced()
    {
        using var session = KqlHelper.Create(
        [
            KqlField.Create("Status", KqlType.String, closed: ["open", "closed"])
        ]);
        Assert.True(KqlHelper.Compile("Status == open", session).Ok);
        Assert.False(KqlHelper.Compile("Status == other", session).Ok);
        Assert.True(KqlHelper.Compile("Status(open)", session).Ok);
        Assert.False(KqlHelper.Compile("Status(other)", session).Ok);
    }

    [Fact]
    public void Evaluator_hits_type_branches()
    {
        using var session = Session();
        var row = new KqlFixtureRow(session)
            .Set("Name", "hello")
            .Set("Count", 5L)
            .Set("Flag", true)
            .Set("When", new DateTime(2026, 11, 15))
            .Set("Span", TimeSpan.FromSeconds(5))
            .Set("Addr", "10.1.2.3")
            .Set("Mac", "00:11:22:33:44:55");

        AssertMatch(session, row, "Name CONTAINS \"ell\"");
        AssertMatch(session, row, "Name BEGINS WITH \"he\"");
        AssertMatch(session, row, "Name ENDS WITH \"lo\"");
        AssertMatch(session, row, "Count == 5");
        AssertMatch(session, row, "Flag == true");
        AssertMatch(session, row, "When == date(2026-11-15)");
        AssertMatch(session, row, "Span == 5s");
        AssertMatch(session, row, "Addr == ipaddress(10.1.2.3)");
        AssertMatch(session, row, "Addr CONTAINS ipaddress(1.2)");
        AssertMatch(session, row, "Addr BEGINS WITH ipaddress(10)");
        AssertMatch(session, row, "Addr ENDS WITH ipaddress(2.3)");
        AssertMatch(session, row, "Mac == macaddress(00:11:22:33:44:55)");
        AssertMatch(session, row, "Mac CONTAINS macaddress(22:33)");
        AssertMatch(session, row, "Mac ENDS WITH macaddress(44:55)");
        AssertMatch(session, row, "Name IN (\"hello\", \"x\")");
        AssertMatch(session, row, "Count BETWEEN 1 AND 9");
        AssertMatch(session, row, "NOT (Count == 0)");
        AssertMatch(session, row, "Count == 5 AND Name CONTAINS \"he\"");
        AssertMatch(session, row, "Count == 0 OR Name == \"hello\"");
    }

    [Fact]
    public void Evaluator_hits_conversion_and_null_branches()
    {
        using var session = Session();
        var row = new KqlFixtureRow(session)
            .Set("Name", "hello")
            .Set("Count", 5)
            .Set("Flag", true)
            .Set("When", "2026-11-15")
            .Set("Span", "5s")
            .Set("Addr", "10.1.2.3")
            .Set("Mac", "001122334455");

        AssertMatch(session, row, "Flag == true");
        AssertMatch(session, row, "Flag == yes");
        AssertMatch(session, row, "When == \"2026-11-15\"");
        AssertMatch(session, row, "Span == 5s");
        AssertMatch(session, row, "Count == 5");
        AssertMatch(session, row, "Name.isEmpty()");
        AssertMatch(session, row, "Flag.isTrue()");
        AssertMatch(session, row, "Flag.isFalse()");
    }

    [Fact]
    public void Completer_covers_slots()
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
        Assert.Empty(KqlHelper.Complete("x", 5, session).Rows);
        Assert.Empty(KqlHelper.Complete("Name == ipaddress(", 19, session).Rows);
    }

    [Theory]
    [InlineData(typeof(char), KqlType.Char)]
    [InlineData(typeof(DateOnly), KqlType.DateTime)]
    [InlineData(typeof(KqlIpAddress), KqlType.IpAddress)]
    [InlineData(typeof(KqlMacAddress), KqlType.MacAddress)]
    public void Type_map_covers_remaining(Type type, KqlType expected)
    {
        Assert.Equal(expected, KqlTypes.From(type));
        Assert.Equal(KqlType.String, KqlTypes.Infer(null));
    }

    private static void AssertMatch(KqlSession session, KqlFixtureRow row, string text)
    {
        var compiled = KqlHelper.Compile(text, session);
        Assert.True(compiled.Ok, compiled.Error?.Message);
        Assert.Equal(KqlTriState.True, compiled.Query!.Evaluate(row));
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
