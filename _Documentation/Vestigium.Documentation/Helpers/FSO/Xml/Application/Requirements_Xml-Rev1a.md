# Vestigium.Helpers.Xml — Requirements Specification

**Document ID:** VEST-HLP-XML-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_Xml_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Xml`  
**APPID:** `Xml`  
**EventIds:** reserved

Binding contract for Xml as staged in the Vestigium monorepo.

---

## 0. Purpose

XML codec for Vestigium hosts. Load, query, and write. Not an XSLT engine.

---

## 1. Surface

- Load.
- Query.
- Write.

---

## 2. Rules that do not move

- One options contract.
- Does not execute XSLT.

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

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.Xml.Tests`.

---

## 6. Non-goals

- XSLT.
- Schema validation as a product.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
