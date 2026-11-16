using Vestigium.Helpers.Kql;

namespace Vestigium.Helpers.Kql.Tests.Coverage;

public sealed class KqlMethodsTests
{
    [Theory]
    [InlineData("contains", "contains")]
    [InlineData("EQUALS", "equals")]
    [InlineData("startsWith", "startsWith")]
    [InlineData("endsWith", "endsWith")]
    [InlineData("isEmpty", "isEmpty")]
    [InlineData("like", "like")]
    [InlineData("isTrue", "isTrue")]
    [InlineData("isFalse", "isFalse")]
    [InlineData("greaterThan", "greaterThan")]
    [InlineData("greaterThanOrEqual", "greaterThanOrEqual")]
    [InlineData("lessThan", "lessThan")]
    [InlineData("lessThanOrEqual", "lessThanOrEqual")]
    [InlineData("between", "between")]
    [InlineData("after", "after")]
    [InlineData("before", "before")]
    [InlineData("on", "on")]
    public void Canonical_accepts_each_method(string name, string canonical)
    {
        Assert.Equal(canonical, KqlMethods.Canonical(name));
        Assert.True(KqlMethods.IsMethod(name));
    }

    [Fact]
    public void Canonical_rejects_unknown()
    {
        Assert.Null(KqlMethods.Canonical("nope"));
        Assert.False(KqlMethods.IsMethod("nope"));
    }

    [Theory]
    [InlineData("Name.contains", true, "Name", "contains")]
    [InlineData("Name.nope", false, "Name.nope", "")]
    [InlineData("contains", false, "contains", "")]
    [InlineData(".contains", false, ".contains", "")]
    public void TrySplit_splits_only_a_real_method(string ident, bool ok, string field, string method)
    {
        var split = KqlMethods.TrySplit(ident, out var gotField, out var gotMethod);
        Assert.Equal(ok, split);
        Assert.Equal(field, gotField);
        Assert.Equal(method, gotMethod);
    }

    [Theory]
    [InlineData("contains", KqlType.String, true)]
    [InlineData("like", KqlType.String, true)]
    [InlineData("contains", KqlType.Integer, false)]
    [InlineData("isEmpty", KqlType.String, true)]
    [InlineData("isEmpty", KqlType.Char, true)]
    [InlineData("isEmpty", KqlType.DateTime, true)]
    [InlineData("isEmpty", KqlType.Integer, false)]
    [InlineData("isTrue", KqlType.Boolean, true)]
    [InlineData("isFalse", KqlType.Boolean, true)]
    [InlineData("isTrue", KqlType.String, false)]
    [InlineData("after", KqlType.DateTime, true)]
    [InlineData("before", KqlType.DateTime, true)]
    [InlineData("on", KqlType.DateTime, true)]
    [InlineData("after", KqlType.String, false)]
    [InlineData("between", KqlType.Integer, true)]
    [InlineData("between", KqlType.Long, true)]
    [InlineData("between", KqlType.Number, true)]
    [InlineData("between", KqlType.Decimal, true)]
    [InlineData("between", KqlType.TimeSpan, true)]
    [InlineData("between", KqlType.DateTime, true)]
    [InlineData("between", KqlType.String, false)]
    [InlineData("greaterThan", KqlType.Integer, true)]
    [InlineData("greaterThanOrEqual", KqlType.Long, true)]
    [InlineData("lessThan", KqlType.Number, true)]
    [InlineData("lessThanOrEqual", KqlType.Decimal, true)]
    [InlineData("greaterThan", KqlType.TimeSpan, true)]
    [InlineData("greaterThan", KqlType.String, false)]
    [InlineData("equals", KqlType.Char, true)]
    [InlineData("nope", KqlType.String, false)]
    public void Allows_follows_the_type(string method, KqlType type, bool allowed)
    {
        Assert.Equal(allowed, KqlMethods.Allows(method, type, KqlCompareAs.Stored));
    }

    [Theory]
    [InlineData(KqlType.IpAddress)]
    [InlineData(KqlType.MacAddress)]
    public void Allows_limits_address_types(KqlType type)
    {
        Assert.True(KqlMethods.Allows("equals", type, KqlCompareAs.Stored));
        Assert.True(KqlMethods.Allows("contains", type, KqlCompareAs.Stored));
        Assert.False(KqlMethods.Allows("like", type, KqlCompareAs.Stored));
        Assert.True(KqlMethods.Allows("equals", KqlType.String, KqlCompareAs.IpAddress));
        Assert.False(KqlMethods.Allows("like", KqlType.String, KqlCompareAs.MacAddress));
    }

    [Fact]
    public void For_lists_each_type()
    {
        Assert.Contains("like", KqlMethods.For(KqlType.String, KqlCompareAs.Stored));
        Assert.Equal(new[] { "equals", "isEmpty" }, KqlMethods.For(KqlType.Char, KqlCompareAs.Stored));
        Assert.Contains("isTrue", KqlMethods.For(KqlType.Boolean, KqlCompareAs.Stored));
        Assert.Contains("between", KqlMethods.For(KqlType.Integer, KqlCompareAs.Stored));
        Assert.Contains("between", KqlMethods.For(KqlType.Long, KqlCompareAs.Stored));
        Assert.Contains("between", KqlMethods.For(KqlType.Number, KqlCompareAs.Stored));
        Assert.Contains("between", KqlMethods.For(KqlType.Decimal, KqlCompareAs.Stored));
        Assert.Contains("between", KqlMethods.For(KqlType.TimeSpan, KqlCompareAs.Stored));
        Assert.Contains("after", KqlMethods.For(KqlType.DateTime, KqlCompareAs.Stored));
        Assert.Contains("startsWith", KqlMethods.For(KqlType.IpAddress, KqlCompareAs.Stored));
        Assert.Contains("startsWith", KqlMethods.For(KqlType.String, KqlCompareAs.IpAddress));
        Assert.Contains("startsWith", KqlMethods.For(KqlType.String, KqlCompareAs.MacAddress));
    }

    [Theory]
    [InlineData("isEmpty", true)]
    [InlineData("isTrue", true)]
    [InlineData("isFalse", true)]
    [InlineData("equals", false)]
    public void IsNullary_is_the_three_flags(string method, bool nullary)
    {
        Assert.Equal(nullary, KqlMethods.IsNullary(method));
    }

    [Fact]
    public void IsBetween_is_only_between()
    {
        Assert.True(KqlMethods.IsBetween("between"));
        Assert.False(KqlMethods.IsBetween("equals"));
    }

    [Theory]
    [InlineData("contains", KqlCompareOp.Contains)]
    [InlineData("startsWith", KqlCompareOp.BeginsWith)]
    [InlineData("endsWith", KqlCompareOp.EndsWith)]
    [InlineData("like", KqlCompareOp.Like)]
    [InlineData("greaterThan", KqlCompareOp.Gt)]
    [InlineData("after", KqlCompareOp.Gt)]
    [InlineData("greaterThanOrEqual", KqlCompareOp.Ge)]
    [InlineData("lessThan", KqlCompareOp.Lt)]
    [InlineData("before", KqlCompareOp.Lt)]
    [InlineData("lessThanOrEqual", KqlCompareOp.Le)]
    [InlineData("isFalse", KqlCompareOp.Eq)]
    [InlineData("equals", KqlCompareOp.Eq)]
    [InlineData("nope", KqlCompareOp.Eq)]
    public void ToOp_maps_each_method(string method, KqlCompareOp op)
    {
        Assert.Equal(op, KqlMethods.ToOp(method));
    }
}
