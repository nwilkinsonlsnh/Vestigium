# Converters — Design Specification

**Application:** Converters
**Document ID:** VEST-CONV-SDS-000
**Version:** 1a
**Status:** Staging — monorepo release
**Date:** 15 November 2026
**Target:** .NET 10 LTS / `net10.0-windows` / WPF
**Companions:** `Requirements-Rev1a.md`
**Project:** `Converters/Vestigium.Converters`

How the staged library meets the 1a requirements.

---

## 0. Shape

One WPF class library. `VestigiumConverterHost.ServiceProvider` is the seam. The markup extension asks that provider for the converter type. State-free converters are registered singleton so a continuous binding does not allocate per tick.

---

## 1. Decisions

| Decision | Why |
|---|---|
| Own project, not a Helper | A converter returns a `Brush`. Helpers must stay usable from a CLI. |
| Own project, not Controls | A host may take Controls without taking the catalog. |
| DI, not static resources | One registration, no per-view converter resource. |

---

## 2. Non-goals

- A demo executable.
- Converters that reference a suite host.

---

## 3. Revision History

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 15 Nov 2026 | Nathaniel Wilkinson | Initial Vestigium release. |
