# Vestigium.Controls — Requirements Specification

**Application:** Controls
**Document ID:** VEST-CTL-SRS-000
**Version:** 1a
**Status:** Staging — monorepo release
**Date:** 15 November 2026
**Target:** .NET 10 LTS / `net10.0-windows` / WPF
**Companions:** `DesignSpec-Rev-1a.md`
**Projects:** `Controls/Vestigium.Controls/`, NumericUpDown, QueryBar, StatusBar, UnderConstruction
**Package:** `Vestigium.Controls` and the control libraries

Binding contract for the control set as staged in the Vestigium monorepo. Source material is the Vestigium.Controls umbrella 1.2. The properties grid is not part of this release.

---

## 0. Purpose

Reusable WPF controls and the default Vestigium form. A host starts from the shell and drops in the controls it needs. The set answers:

- a shared shell and default form (`Vestigium.Controls`);
- a numeric editor with step, snap, and sign rules (`NumericUpDown`);
- a query entry bar (`QueryBar`);
- a status bar (`StatusBar`);
- an under-construction placeholder (`UnderConstruction`).

It does not include a properties grid. That control is out of this monorepo.

---

## 1. Scope

### 1.1 In scope (1a)

| ID | Capability |
|---|---|
| S-01 | Umbrella shell and default form |
| S-02 | NumericUpDown engine: min, max, increment, decimal places, sign mode, snap |
| S-03 | QueryBar |
| S-04 | StatusBar |
| S-05 | UnderConstruction placeholder |
| S-06 | ThemeLab sample for the numeric control |

### 1.2 Out of scope (1a)

- `Vestigium.Controls.PropertiesGrid`. Not brought over. Not planned for repair.
- Implicit theme styles. Hosts style these controls from `Vestigium.Themes`.

---

## 2. Projects

| Project | Role |
|---|---|
| `Vestigium.Controls` | Shell, default form, DI host. References StatusBar and UnderConstruction. |
| `Vestigium.Controls.NumericUpDown` | Numeric editor and `NumericEngine`. |
| `Vestigium.Controls.QueryBar` | Query entry. |
| `Vestigium.Controls.StatusBar` | Status bar. |
| `Vestigium.Controls.UnderConstruction` | Placeholder view. |
| `Vestigium.Controls.ThemeLab` | Sample. Not part of the contract. |

---

## 3. Numeric contract

| Rule | Behavior |
|---|---|
| Range | Value stays inside minimum and maximum. |
| Step | Increment and page increment are explicit. |
| Sign | Sign mode controls whether a negative value is accepted. |
| Snap | Optional snap to increment from a base. |
| Input | Full, restricted, or read-only. Read-only does not spin. |
| Commit | Auto or explicit, per `CommitMode`. |

---

## 4. Non-goals

No properties grid. No theme catalogs in this folder. Themes owns palettes.

---

## 5. Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 15 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. Properties grid excluded. |
