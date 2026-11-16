using System.Windows;

namespace Vestigium.Themes.Tests;

public sealed class PaletteContractTests
{
    public static IEnumerable<object[]> Palettes() =>
        ThemeDefinitions.SuiteV1.Select(t => new object[] { t });

    [Theory]
    [MemberData(nameof(Palettes))]
    public void Each_shipped_palette_satisfies_the_key_contract(ThemeDefinition theme)
    {
        PackScheme.OnApplication(() =>
        {
            var dictionary = new ResourceDictionary { Source = theme.ResourceUri };
            ThemeContractValidator.Validate(dictionary);

            Assert.Equal(theme.Id, dictionary[ThemeResourceKeys.ThemeId]);
            Assert.Equal(theme.DisplayName, dictionary[ThemeResourceKeys.ThemeDisplayName]);
            Assert.Equal(theme.IsDark, dictionary[ThemeResourceKeys.ThemeIsDark]);
            Assert.Equal(ThemeResourceKeys.ContractVersion, dictionary[ThemeResourceKeys.ThemeVersion]);
        });
    }
}
