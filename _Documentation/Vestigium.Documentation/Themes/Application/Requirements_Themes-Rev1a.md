# Vestigium.Themes — Requirements Specification

**Document ID:** VEST-THM-SRS-000
**Version:** 1a
**Status:** Staging — monorepo release
**Date:** 15 November 2026
**Target:** .NET 10 LTS / `net10.0-windows` / WPF
**Companions:** `DesignSpec_Themes_Rev-1a.md`
**Projects:** `Themes/Vestigium.Themes/`, `Themes/Vestigium.Themes.Controls/`, nine palette projects, `Themes/Vestigium.Themes.Pack/`
**Package:** `Vestigium.Themes` (core), `Vestigium.Themes.Controls`, palette assemblies, `Vestigium.Themes.Pack`

Binding contract for the theme system as staged in the Vestigium monorepo. Source material is the Vestigium.Themes 1.0 requirements, rewritten to this library format.

---

## 0. Purpose

One visual language for Vestigium WPF hosts. A host registers the palettes it ships, initializes once, and styles views with named catalog keys. The system answers:

- which palettes are available (`ThemeManager.AvailableThemes`);
- which palette is current (`ThemeManager.Current`);
- how to swap a palette at runtime without rebuilding the host (`SwitchTheme`);
- whether a palette is complete (`ThemeContractValidator`);
- which explicit styles a view may use (`Vestigium.Themes.Controls`).

It does not draw charts. It does not persist a user preference. It does not replace Fluent or MahApps. The host owns the window and the saved theme id.

```csharp
protected override void OnStartup(StartupEventArgs e)
{
    var manager = new ThemeManager();
    manager.RegisterSuiteV1();
    manager.Initialize(this, "LightBlue");
    base.OnStartup(e);
}
```

---

## 1. Binding language

| Term | Meaning |
|---|---|
| **Palette** | A `ResourceDictionary` that supplies the color and brush keys in `ThemeResourceKeys`. |
| **Catalog** | The explicit named styles in `Vestigium.Themes.Controls`. |
| **Core** | Metrics and converters merged once. Never replaced on a switch. |
| **Slot** | The merged dictionary that contains `Vestigium.Theme.Id`. `SwitchTheme` replaces only this slot. |
| **Contract** | Every required key in `ThemeResourceKeys` must resolve. Missing keys fail the load. |
| **Suite v1** | The nine palettes in `ThemeDefinitions.SuiteV1`. |

---

## 2. Scope

### 2.1 In scope (1a)

| ID | Capability |
|---|---|
| S-01 | Core contracts, `ThemeManager`, metrics, converters (`Vestigium.Themes`) |
| S-02 | Explicit control catalog (`Vestigium.Themes.Controls`) |
| S-03 | Nine palettes with the same resource-key contract |
| S-04 | Runtime switch that replaces only the palette dictionary |
| S-05 | Contract validation on every palette load |
| S-06 | Standard WPF palette (platform-like hues, same keys) |
| S-07 | `Unload()` drops Vestigium resources |
| S-08 | `Vestigium.Themes.Pack` references the suite for a host that wants all of it |

### 2.2 Out of scope (1a)

- Implicit `TargetType` styles. A view that omits `Style=` keeps the stock WPF look.
- Skia, WinUI, MAUI, or web ports.
- A theme-authoring UI.
- Localization of style keys beyond `en-US` metadata.
- Custom control assemblies beyond restyling native WPF types.
- Persisting the selected theme id. That is the host.

---

## 3. Projects

| Project | Role |
|---|---|
| `Vestigium.Themes` | Contracts, `ThemeManager`, `ThemeDefinitions`, metrics, converters |
| `Vestigium.Themes.Controls` | Explicit styles. References the core. |
| `Vestigium.Themes.LightBlue` | Palette. Light. |
| `Vestigium.Themes.DarkMode` | Palette. Dark. |
| `Vestigium.Themes.Terminal` | Palette. Dark. |
| `Vestigium.Themes.SolarizedDark` | Palette. Dark. |
| `Vestigium.Themes.StandardWPF` | Palette. Light. Platform-like hues. |
| `Vestigium.Themes.Monokai` | Palette. Dark. |
| `Vestigium.Themes.Sublime` | Palette. Dark. Mariana. |
| `Vestigium.Themes.Dracula` | Palette. Dark. |
| `Vestigium.Themes.Nord` | Palette. Dark. |
| `Vestigium.Themes.Pack` | Aggregates core, controls, and every palette. |

A host references the core, the controls, and only the palettes it ships. Unused palettes are not loaded.

---

## 4. Host contract

| Rule | Behavior |
|---|---|
| Register before initialize | `Initialize` throws `ThemeLoadException` when nothing is registered. |
| Initialize before the first window | `{StaticResource Button.Primary}` must resolve before a view is parsed. `OnStartup` before `base.OnStartup` is the place. |
| Register is explicit | `RegisterSuiteV1` does not run on package import. |
| Duplicate id fails | A second `Register` of the same id throws `InvalidOperationException`. |
| Unknown id fails | `SwitchTheme` of an unregistered id throws `ThemeLoadException`. |
| Incomplete palette fails | `ThemeContractValidator` throws `ThemeLoadException` and lists `MissingKeys`. |
| Switch replaces the slot only | Core metrics and the control catalog stay. |
| Unload is total | `Unload` removes Vestigium dictionaries from the application. |

---

## 5. Palette contract

Every `Theme.xaml` supplies the keys in `ThemeResourceKeys`.

| Group | Examples |
|---|---|
| Metadata | `Vestigium.Theme.Id`, `DisplayName`, `Base`, `IsDark`, `Version`, `Showcase` |
| Surfaces | `Vestigium.Colors.Surface.Window`, `Layer`, `Card`, `Chrome`, `Sidebar` |
| Controls | `Vestigium.Colors.Control.Fill`, `FillHover`, `FillPressed`, `FillInput` |
| Text | `Vestigium.Colors.Text.Primary` and the rest of the text ramp |
| Brushes | `Vestigium.Brushes.*` matching the color keys the catalog consumes |

`ThemeVersion` matches `ThemeResourceKeys.ContractVersion` (`1.0`). A palette that omits a required key does not load.

---

## 6. Catalog contract

Styles are explicit and named. Views opt in.

| Rule | Behavior |
|---|---|
| No implicit `TargetType` | Stock WPF remains when a view does not set `Style`. |
| Static on the style key | `Style="{StaticResource Button.Primary}"`. |
| Dynamic on the color | Templates use `{DynamicResource Vestigium.Brushes.*}` so a switch repaints the tree. |
| Intent over hex | `Primary`, `Error`, `Success`. Views do not name a hex. |

---

## 7. Non-goals

Implicit styles. A designer UI. Persisted theme preference. Non-WPF ports. Packing policy beyond `Vestigium.Themes.Pack` as a project reference aggregator.

---

## 8. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 15 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. Rewritten from Vestigium.Themes requirements 1.0. |
