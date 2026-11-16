using Vestigium.Helpers.Kql;
using Vestigium.Helpers.Kql.ActiveSetup;

namespace Vestigium.Helpers.Kql.ActiveSetup.Tests;

public sealed class ActiveSetupKqlTests
{
    [Fact]
    public void Components_Opens_A_Session_With_The_Catalog()
    {
        using var session = ActiveSetupKql.Components();
        Assert.Equal(ActiveSetupKql.Identity, "Vestigium.Helpers.Kql.ActiveSetup");
        Assert.Equal(10, session.Fields.Count);
        Assert.Contains(session.Fields, f => f.Canonical == "setup.displayname");
        Assert.Contains(session.Fields, f => f.Canonical == "setup.pending");
    }

    [Fact]
    public void ToRow_Maps_All_Facts_Onto_The_Session()
    {
        using var session = ActiveSetupKql.Components();
        var facts = new ActiveSetupFacts
        {
            DisplayName = "Test Component",
            Component = "{12345678-1234-1234-1234-1234567890AB}",
            StubPath = @"C:\Windows\System32\stub.exe",
            Version = "1.0.0.0",
            Locale = "en-US",
            ComponentId = "Test.Component",
            Installed = true,
            IsGlobal = true,
            IsX64 = false,
            Pending = true
        };

        var row = ActiveSetupAdapter.ToRow(session, facts);

        Assert.Equal("Test Component", row.Get("setup.displayname").Raw);
        Assert.Equal("{12345678-1234-1234-1234-1234567890AB}", row.Get("setup.component").Raw);
        Assert.Equal(@"C:\Windows\System32\stub.exe", row.Get("setup.stubpath").Raw);
        Assert.Equal("1.0.0.0", row.Get("setup.version").Raw);
        Assert.Equal("en-US", row.Get("setup.locale").Raw);
        Assert.Equal("Test.Component", row.Get("setup.componentId").Raw);
        Assert.Equal(true, row.Get("setup.installed").Raw);
        Assert.Equal(true, row.Get("setup.isGlobal").Raw);
        Assert.Equal(false, row.Get("setup.isX64").Raw);
        Assert.Equal(true, row.Get("setup.pending").Raw);
    }

    [Fact]
    public void ToRow_Rejects_Null_Arguments()
    {
        using var session = ActiveSetupKql.Components();
        Assert.Throws<ArgumentNullException>(() => ActiveSetupAdapter.ToRow(null!, new ActiveSetupFacts()));
        Assert.Throws<ArgumentNullException>(() => ActiveSetupAdapter.ToRow(session, null!));
    }

    [Theory]
    [InlineData("setup.pending == true")]
    [InlineData("setup.displayname LIKE '%Test%'")]
    [InlineData("setup.isX64 == false")]
    [InlineData("setup.stubpath.contains(\"powershell\")")]
    public void Compile_Accepts_Known_Fields(string text)
    {
        using var session = ActiveSetupKql.Components();
        var compiled = KqlHelper.Compile(text, session);
        Assert.True(compiled.Ok, compiled.Error?.Message);
    }
}
