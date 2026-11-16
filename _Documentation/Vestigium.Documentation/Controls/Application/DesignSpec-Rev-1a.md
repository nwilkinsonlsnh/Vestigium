# Vestigium.Controls — Design Specification

**Application:** Controls
**Document ID:** VEST-CTL-SDS-000
**Version:** 1a
**Status:** Staging — monorepo release
**Date:** 15 November 2026
**Target:** .NET 10 LTS / `net10.0-windows` / WPF
**Companions:** `Requirements-Rev1a.md`
**Projects:** `Controls/`

How the staged control set meets the 1a requirements.

---

## 0. Shape

One umbrella and four control libraries. The umbrella references StatusBar and UnderConstruction. NumericUpDown and QueryBar stand alone so a host can take one without the shell.

```text
Vestigium.Controls
  ├── Vestigium.Controls.StatusBar
  └── Vestigium.Controls.UnderConstruction
Vestigium.Controls.NumericUpDown
Vestigium.Controls.QueryBar
```

---

## 1. Numeric engine

`NumericEngine` owns the value rules. The control binds to it.

| Piece | Role |
|---|---|
| Minimum / Maximum | Clamp. |
| Increment / PageIncrement | Step size. |
| DecimalPlaces / FormatString | Display. |
| SignMode | Negative policy. |
| SnapToIncrement / SnapBase | Optional grid snap. |
| InputMode | Full, restricted, read-only. |
| CommitMode | Auto or explicit. |

`CanSpin` is false when input is read-only. `CanType` is true only for full input.

---

## 2. Tests

`Tests/Controls/Vestigium.Controls.Tests` references the umbrella, StatusBar, NumericUpDown, UnderConstruction, and QueryBar. It does not reference a properties grid. Coverage is the engines and the shell, not the grid.

---

## 3. Decisions

| Decision | Why |
|---|---|
| Grid left behind | The properties grid implementation is not one we will repair. |
| Numeric engine separate from the control | Rules are testable without a window. |
| Umbrella references only what the shell needs | A numeric-only host does not pull the form. |

---

## 4. Non-goals

No properties grid. No palette work. That stays in Themes.

---

## 5. Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 15 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. Properties grid excluded. |
