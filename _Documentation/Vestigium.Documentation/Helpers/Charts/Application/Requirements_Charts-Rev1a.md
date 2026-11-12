# Vestigium.Helpers.Charts — Requirements Specification

**Document ID:** VEST-HLP-CHARTS-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0-windows`  
**Companions:** `DesignSpec_Charts_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Charts`  
**APPID:** `Charts`  
**EventIds:** 16500–16999

Binding contract for Charts as staged in the Vestigium monorepo.

---

## 0. Purpose

ScottPlot wrapper that puts Analytics numbers on a WPF form. Analytics supplies the numbers. Charts draws them. It does not compute UCL, CL, LCL, run rules, spec fences, or KDE points. ScottPlot types are not public.

---

## 1. Surface

- ChartView.Control(series, limits, rules?) — control chart. Requires UCL > CL > LCL.
- ChartView.Histogram / Ecdf / Line / Scatter / Column / Bar / Pie / Pareto / Box / Bands.
- ChartView.MeanInterval(series, γ) — calls Analytics Confidence then paints.
- ChartView.From(spec) / SavePng(spec, path) — generic door and headless PNG.
- ChartsCatalog.Register(cfg) — host-only, during VestigiumLogger.Initialize.

---

## 2. Rules that do not move

- Charts paints. Analytics computes.
- ScottPlot types stay internal. Hosts receive FrameworkElement.
- Empty series and x/y length mismatch are rejected.
- Line and Scatter accept at most two series.
- On a non-Windows host, From returns a text stand-in.
- The library never calls VestigiumLogger.Initialize.

---

## 3. Logging

The library never calls `VestigiumLogger.Initialize` and never chooses a log folder. Hosts that want a disk trace call `Initialize` and the library catalog `Register`.

Writes are no-ops until the host initializes. JSONL lands at `%ProgramData%\Vestigium\Logs\{host-APPID}\`.

---

## 4. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0-windows` |
| Depends on | Vestigium.Helpers.Analytics, ScottPlot.WPF 5.1.59, Vestigium.Logging |
| License | MIT |

---

## 5. Tests

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.Charts.Tests`.

---

## 6. Non-goals

- Computing UCL in Charts.
- Public ScottPlot types.
- Host adapters.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
