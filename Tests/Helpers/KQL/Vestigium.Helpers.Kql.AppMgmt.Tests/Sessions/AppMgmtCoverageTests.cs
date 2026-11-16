using Vestigium.Helpers.Kql;
using Vestigium.Helpers.Kql.AppMgmt;

namespace Vestigium.Helpers.Kql.AppMgmt.Tests;

public sealed class AppMgmtCoverageTests
{
    [Fact]
    public void Bloatware_Session_And_Adapter_Cover_The_Catalog()
    {
        using var session = AppMgmtKql.Bloatware();
        Assert.Equal(6, session.Fields.Count);
        Assert.Contains(session.Fields, f => f.Canonical == "bloat.name");
        Assert.Contains(session.Fields, f => f.Canonical == "bloat.package");

        var facts = new BloatwareFacts
        {
            Name = "Candy Crush",
            Family = "Microsoft.Gaming",
            Category = "Games",
            Installed = true,
            Provisioned = false,
            Package = "Microsoft.CandyCrush_1.0"
        };

        var row = BloatwareAdapter.ToRow(session, facts);
        Assert.Equal("Candy Crush", row.Get("bloat.name").Raw);
        Assert.Equal("Microsoft.Gaming", row.Get("bloat.family").Raw);
        Assert.Equal("Games", row.Get("bloat.category").Raw);
        Assert.Equal(true, row.Get("bloat.installed").Raw);
        Assert.Equal(false, row.Get("bloat.provisioned").Raw);
        Assert.Equal("Microsoft.CandyCrush_1.0", row.Get("bloat.package").Raw);
    }

    [Fact]
    public void Feature_Adapter_Maps_Facts()
    {
        using var session = AppMgmtKql.Features();
        var facts = new FeatureFacts
        {
            Caption = "Hyper-V",
            Name = "Microsoft-Hyper-V",
            State = "Enabled",
            IsEnabled = true,
            Parent = "NetFx4-AdvSrvs"
        };

        var row = FeatureAdapter.ToRow(session, facts);
        Assert.Equal("Hyper-V", row.Get("feature.caption").Raw);
        Assert.Equal("Microsoft-Hyper-V", row.Get("feature.name").Raw);
        Assert.Equal(true, row.Get("feature.state").Raw);          // mapped from IsEnabled
        Assert.Equal(true, row.Get("feature.IsEnabled").Raw);
        Assert.Equal("NetFx4-AdvSrvs", row.Get("feature.parent").Raw);
    }

    [Fact]
    public void Footprints_Adapter_Covers_RefCount_Present_And_Absent()
    {
        using var session = AppMgmtKql.Footprints();

        var withCount = new FootprintsFacts
        {
            Kind = "Registry",
            Name = "HKCU\\Software\\App",
            Path = "Software\\App",
            Scope = "User",
            RefCount = 3,
            Package = "Pkg.1"
        };

        var rowWith = FootprintsAdapter.ToRow(session, withCount);
        Assert.Equal("Registry", rowWith.Get("footprint.kind").Raw);
        Assert.Equal(3L, rowWith.Get("footprint.refcount").Raw);

        var withoutCount = new FootprintsFacts
        {
            Kind = "File",
            Name = "app.exe",
            Path = "C:\\app.exe",
            Scope = "Machine",
            RefCount = null,
            Package = "Pkg.2"
        };

        var rowWithout = FootprintsAdapter.ToRow(session, withoutCount);
        Assert.Equal("File", rowWithout.Get("footprint.kind").Raw);
        Assert.True(rowWithout.Get("footprint.refcount").IsUnknown);
    }

    [Fact]
    public void Bloatware_And_Feature_Reject_Nulls()
    {
        using var bloat = AppMgmtKql.Bloatware();
        using var feat = AppMgmtKql.Features();
        using var foot = AppMgmtKql.Footprints();

        Assert.Throws<ArgumentNullException>(() => BloatwareAdapter.ToRow(null!, new BloatwareFacts()));
        Assert.Throws<ArgumentNullException>(() => BloatwareAdapter.ToRow(bloat, null!));
        Assert.Throws<ArgumentNullException>(() => FeatureAdapter.ToRow(null!, new FeatureFacts()));
        Assert.Throws<ArgumentNullException>(() => FeatureAdapter.ToRow(feat, null!));
        Assert.Throws<ArgumentNullException>(() => FootprintsAdapter.ToRow(null!, new FootprintsFacts()));
        Assert.Throws<ArgumentNullException>(() => FootprintsAdapter.ToRow(foot, null!));
    }

    [Theory]
    [InlineData("bloat.installed == true")]
    [InlineData("bloat.name LIKE '%Candy%'")]
    [InlineData("feature.IsEnabled == true")]
    [InlineData("feature.caption LIKE '%Hyper%'")]
    [InlineData("footprint.kind == \"Registry\"")]
    public void Bloatware_And_Feature_Compile(string text)
    {
        using var bloat = AppMgmtKql.Bloatware();
        using var feat = AppMgmtKql.Features();
        using var foot = AppMgmtKql.Footprints();

        if (text.StartsWith("bloat."))
            Assert.True(KqlHelper.Compile(text, bloat).Ok);
        else if (text.StartsWith("feature."))
            Assert.True(KqlHelper.Compile(text, feat).Ok);
        else
            Assert.True(KqlHelper.Compile(text, foot).Ok);
    }
}
