using System.Windows;

namespace Vestigium.Themes.Tests;

public sealed class ThemeContractTests
{
    [Fact]
    public void FromPack_builds_the_component_uri()
    {
        var theme = ThemeDefinition.FromPack("LightBlue", "Light Blue", "Vestigium.Themes.LightBlue", false, "light");
        Assert.Equal("LightBlue", theme.Id);
        Assert.Equal("Light Blue", theme.DisplayName);
        Assert.False(theme.IsDark);
        Assert.Equal("light", theme.Description);
        Assert.Equal(
            "pack://application:,,,/Vestigium.Themes.LightBlue;component/Themes/Theme.xaml",
            theme.ResourceUri.AbsoluteUri);
    }

    [Fact]
    public void FromPack_rejects_blank_id_and_assembly()
    {
        Assert.Throws<ArgumentException>(() => ThemeDefinition.FromPack(" ", "Name", "Asm", false));
        Assert.Throws<ArgumentException>(() => ThemeDefinition.FromPack("Id", "Name", "", false));
    }

    [Fact]
    public void SuiteV1_is_the_nine_shipped_palettes()
    {
        var ids = ThemeDefinitions.SuiteV1.Select(t => t.Id).ToArray();
        Assert.Equal(
            ["LightBlue", "DarkMode", "Terminal", "SolarizedDark", "StandardWPF", "Monokai", "Sublime", "Dracula", "Nord"],
            ids);
        Assert.Equal(ids.Length, ids.Distinct(StringComparer.OrdinalIgnoreCase).Count());
        Assert.Contains(ThemeDefinitions.SuiteV1, t => t.Id == "LightBlue" && !t.IsDark);
        Assert.Contains(ThemeDefinitions.SuiteV1, t => t.Id == "Dracula" && t.IsDark);
    }

    [Fact]
    public void Contract_lists_are_nonempty_and_unique()
    {
        Assert.Equal("1.0", ThemeResourceKeys.ContractVersion);
        Assert.NotEmpty(ThemeResourceKeys.RequiredMetadata);
        Assert.NotEmpty(ThemeResourceKeys.RequiredColors);
        Assert.NotEmpty(ThemeResourceKeys.RequiredBrushes);
        var all = ThemeResourceKeys.RequiredMetadata
            .Concat(ThemeResourceKeys.RequiredColors)
            .Concat(ThemeResourceKeys.RequiredBrushes)
            .ToArray();
        Assert.Equal(all.Length, all.Distinct(StringComparer.Ordinal).Count());
    }

    [Fact]
    public void Validator_rejects_null_and_lists_missing_keys()
    {
        Assert.Throws<ArgumentNullException>(() => ThemeContractValidator.Validate(null!));

        var sparse = new ResourceDictionary();
        sparse[ThemeResourceKeys.ThemeId] = "x";
        var ex = Assert.Throws<ThemeLoadException>(() => ThemeContractValidator.Validate(sparse));
        Assert.Contains(ThemeResourceKeys.ThemeDisplayName, ex.MissingKeys);
        Assert.DoesNotContain(ThemeResourceKeys.ThemeId, ex.MissingKeys);
    }

    [Fact]
    public void Validator_accepts_a_complete_dictionary()
    {
        var full = new ResourceDictionary();
        foreach (var key in ThemeResourceKeys.RequiredMetadata
                     .Concat(ThemeResourceKeys.RequiredColors)
                     .Concat(ThemeResourceKeys.RequiredBrushes))
        {
            full[key] = key;
        }

        ThemeContractValidator.Validate(full);
    }

    [Fact]
    public void Changed_args_keep_previous_and_current()
    {
        var previous = ThemeDefinition.FromPack("A", "A", "A", false);
        var current = ThemeDefinition.FromPack("B", "B", "B", true);
        var args = new ThemeChangedEventArgs(previous, current);
        Assert.Same(previous, args.Previous);
        Assert.Same(current, args.Current);
    }
}
