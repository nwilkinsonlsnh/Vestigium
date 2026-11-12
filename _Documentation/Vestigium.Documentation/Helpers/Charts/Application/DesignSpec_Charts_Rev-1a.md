# Vestigium.Helpers.Charts — Design Specification

**Document ID:** VEST-HLP-CHARTS-DSN-000  
**Version:** 1a  
**Status:** Staging companion to Requirements 1a  
**Date:** 11 November 2026  
**Binding:** `Requirements_Charts-Rev1a.md` wins on conflict

Why the library is shaped this way. It does not add requirements.

---

## 1. Intent

ScottPlot wrapper that puts Analytics numbers on a WPF form. Analytics supplies the numbers. Charts draws them. It does not compute UCL, CL, LCL, run rules, spec fences, or KDE points. ScottPlot types are not public.

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| Charts paints | Locked. |
| ScottPlot types stay internal | Locked. |
| Empty series and x/y length mismatch are rejected | Locked. |
| Line and Scatter accept at most two series | Locked. |
| On a non-Windows host, From returns a text stand-in | Locked. |
| The library never calls VestigiumLogger | Locked. |
| Logging APPID stamps library identity | Folder follows the host. |
| No host adapter | Orchestration is a different project. |

---

## 3. Dependencies

| Package | Role |
|---|---|
| Vestigium.Helpers.Analytics | Primary |
| Vestigium.Logging | JSONL writer. This library never initializes it. |

---

## 4. Still out

- Computing UCL in Charts.
- Public ScottPlot types.
- Host adapters.

---

## 5. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
