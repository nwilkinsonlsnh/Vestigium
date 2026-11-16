# Vestigium.Themes — Design Specification

**Application:** Themes
**Document ID:** VEST-THM-SDS-000
**Version:** 1a
**Status:** Staging — monorepo release
**Date:** 15 November 2026
**Target:** .NET 10 LTS / `net10.0-windows` / WPF
**Companions:** `Requirements-Rev1a.md`
**Projects:** `Themes/`

How the staged theme system meets the 1a requirements.

---

## 0. Shape

Three layers.

| Layer | Project | Lifetime |
|---|---|---|
| Core | `Vestigium.Themes` | Merged once. Metrics, converters, contracts, manager. |
| Catalog | `Vestigium.Themes.Controls` | Merged once. Explicit styles. |
| Palette | one `Vestigium.Themes.<Name>` | The slot. Replaced on `SwitchTheme`. |

`ThemeManager` finds the dictionary that contains `Vestigium.Theme.Id` and replaces that dictionary only.

```text
Application.Resources.MergedDictionaries
  [0] Metrics.xaml          pack from Vestigium.Themes
  [1] Controls.xaml         pack from Vestigium.Themes.Controls
  [2] Theme.xaml            pack from the active palette   <-- the slot
```

---

## 1. Types

| Type | Role |
|---|---|
| `IThemeManager` | Register, initialize, switch, unload, current, changed. |
| `ThemeManager` | The implementation. Holds the registered list and the `Application`. |
| `ThemeDefinition` | Id, display name, pack URI, dark flag, description. `FromPack` builds the URI. |
| `ThemeDefinitions.SuiteV1` | The nine shipped palettes. |
| `ThemeResourceKeys` | Required metadata, color, and brush keys. `ContractVersion` is `1.0`. |
| `ThemeContractValidator` | `Validate(ResourceDictionary)` throws `ThemeLoadException` with `MissingKeys`. |
| `ThemeChangedEventArgs` | Previous and current `ThemeDefinition`. |
| `ThemeLoadException` | Load, unknown id, or incomplete contract. |

---

## 2. Switch

`SwitchTheme` loads the palette with `new ResourceDictionary { Source = theme.ResourceUri }`, validates it, then replaces the slot.

| Step | Behavior |
|---|---|
| Resolve | Case-insensitive match on `ThemeDefinition.Id`. Miss throws. |
| Load | Pack URI from `FromPack`. A bad URI throws `ThemeLoadException`. |
| Validate | Every key in `ThemeResourceKeys` must be present. |
| Replace | Remove the dictionary that owns `Vestigium.Theme.Id`. Insert the new one. |
| Notify | `Current` updates. `ThemeChanged` fires with previous and current. |

Core and catalog are not touched. `{DynamicResource}` bindings repaint. `{StaticResource}` style keys stay valid because the catalog did not move.

`Initialize` calls `EnsureCore` then `SwitchTheme`. No registered themes is a `ThemeLoadException`.

`Unload` removes the Vestigium dictionaries. `Current` clears.

---

## 3. Palettes

XAML-only projects. They compile as WPF libraries so the pack URI resolves.

| Id | Display | Dark | Notes |
|---|---|---|---|
| LightBlue | Light Blue | no | Default-leaning light palette |
| DarkMode | Dark Mode | yes | |
| Terminal | Terminal | yes | |
| SolarizedDark | Solarized Dark | yes | |
| StandardWPF | Standard WPF | no | Platform-like hues, same keys |
| Monokai | Monokai | yes | |
| Sublime | Sublime | yes | Mariana |
| Dracula | Dracula | yes | |
| Nord | Nord | yes | |

`RegisterSuiteV1` registers this list. A host that ships two palettes calls `Register` twice and never calls `RegisterSuiteV1`.

---

## 4. Catalog

`Controls.xaml` merges the explicit style dictionaries (buttons, inputs, lists, grids, layout). Style keys are static. Color inside a template is dynamic.

A view that does not set `Style` is unchanged. That is intentional.

---

## 5. Pack

`Vestigium.Themes.Pack` is a project-reference aggregator. It references the core, the controls, and every palette, with `PrivateAssets=all` on those references. A host that wants the whole suite references Pack. A host that wants a subset references the projects it needs and does not reference Pack.

---

## 6. Decisions

| Decision | Why |
|---|---|
| Explicit styles only | A host that has not opted in keeps stock WPF. |
| Static key, dynamic color | The style identity is stable. The paint follows the slot. |
| Validate on load | A missing key fails at switch, not at first use of a control. |
| Slot by `Vestigium.Theme.Id` | The manager does not assume dictionary index. |
| Register is opt-in | Importing the package does not load nine palettes. |
| Pack is an aggregator | One reference for the suite, none for a two-palette host. |

---

## 7. Non-goals

No implicit styles. No saved preference. No non-WPF port. No authoring surface.

---

## 8. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 15 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. Rewritten from Vestigium.Themes 1.0. |
