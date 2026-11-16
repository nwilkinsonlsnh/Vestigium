using System.Windows;

namespace Vestigium.Themes.Tests;

public sealed class ThemeManagerTests
{
    [Fact]
    public void Register_rejects_null_and_duplicates()
    {
        var manager = new ThemeManager();
        Assert.Throws<ArgumentNullException>(() => manager.Register(null!));
        manager.Register(ThemeDefinition.FromPack("LightBlue", "Light Blue", "Vestigium.Themes.LightBlue", false));
        Assert.Throws<InvalidOperationException>(() =>
            manager.Register(ThemeDefinition.FromPack("lightblue", "Again", "Vestigium.Themes.LightBlue", false)));
        Assert.Single(manager.AvailableThemes);
    }

    [Fact]
    public void RegisterSuiteV1_registers_each_shipped_palette_once()
    {
        var manager = new ThemeManager();
        manager.RegisterSuiteV1();
        Assert.Equal(9, manager.AvailableThemes.Count);
        Assert.Throws<InvalidOperationException>(() => manager.RegisterSuiteV1());
    }

    [Fact]
    public void Initialize_requires_an_application_and_a_registration()
    {
        var manager = new ThemeManager();
        Assert.Throws<ArgumentNullException>(() => manager.Initialize(null!));
    }

    [Fact]
    public void Switch_before_initialize_has_no_application()
    {
        var manager = new ThemeManager();
        Assert.Throws<ArgumentException>(() => manager.SwitchTheme(" "));
        Assert.Throws<InvalidOperationException>(() => manager.SwitchTheme("LightBlue"));
        Assert.Throws<InvalidOperationException>(() => manager.Unload());
    }

    [Fact]
    public void Initialize_switch_and_unload_round_trip()
    {
        PackScheme.OnApplication(() =>
        {
            var app = Application.Current!;
            var manager = new ThemeManager();
            Assert.Throws<ThemeLoadException>(() => manager.Initialize(app));

            manager.Register(ThemeDefinitions.SuiteV1[0]);
            manager.Register(ThemeDefinitions.SuiteV1[1]);
            ThemeDefinition? seen = null;
            manager.ThemeChanged += (_, e) => seen = e.Current;

            manager.Initialize(app, "DarkMode");
            Assert.Equal("DarkMode", manager.Current?.Id);
            Assert.Equal("DarkMode", seen?.Id);
            Assert.Contains(app.Resources.MergedDictionaries, d => d.Contains(ThemeResourceKeys.ThemeId));

            manager.SwitchTheme("LightBlue");
            Assert.Equal("LightBlue", manager.Current?.Id);

            Assert.Throws<ThemeLoadException>(() => manager.SwitchTheme("missing"));

            manager.Unload();
            Assert.Null(manager.Current);
            Assert.DoesNotContain(app.Resources.MergedDictionaries, d => d.Contains(ThemeResourceKeys.ThemeId));
        });
    }
}
