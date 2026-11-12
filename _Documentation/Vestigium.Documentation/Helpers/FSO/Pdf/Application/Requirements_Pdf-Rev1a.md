# Vestigium.Helpers.Pdf — Requirements Specification

**Document ID:** VEST-HLP-PDF-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_Pdf_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Pdf`  
**APPID:** `Pdf`

Binding contract for Pdf as staged in the Vestigium monorepo.

---

## 0. Purpose

PDF read and write helpers for Vestigium hosts. Not a full authoring suite. Not a browser.

---

## 1. Surface

- Read text from a PDF.
- Write a simple document.
- Page count.
- Seal / verdict types for report flows.

---

## 2. Rules that do not move

- Does not embed a browser.
- Fail closed on corrupt input.
- The library never calls `VestigiumLogger.Initialize`.

---

## 3. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0` |
| Depends on | `Vestigium.Logging` |
| License | MIT |

---

## 4. Non-goals

Full layout engine. Form filling as a product. Browser embed.

## Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. |
