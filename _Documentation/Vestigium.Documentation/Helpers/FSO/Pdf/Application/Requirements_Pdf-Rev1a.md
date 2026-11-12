# Vestigium.Helpers.Pdf — Requirements Specification

**Document ID:** VEST-HLP-PDF-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_Pdf_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Pdf`  
**APPID:** `Pdf`  
**EventIds:** reserved

Binding contract for Pdf as staged in the Vestigium monorepo.

---

## 0. Purpose

PDF read and write helpers for Vestigium hosts. Not a full authoring suite.

---

## 1. Surface

- Read text.
- Write a simple document.
- Page count.

---

## 2. Rules that do not move

- Does not embed a browser.
- Fail closed on corrupt input.

---

## 3. Logging

The library never calls `VestigiumLogger.Initialize` and never chooses a log folder. Hosts that want a disk trace call `Initialize` and the library catalog `Register`.

Writes are no-ops until the host initializes. JSONL lands at `%ProgramData%\Vestigium\Logs\{host-APPID}\`.

---

## 4. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0` |
| Depends on | Vestigium.Logging |
| License | MIT |

---

## 5. Tests

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.Pdf.Tests`.

---

## 6. Non-goals

- Full layout engine.
- Form filling as a product.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
