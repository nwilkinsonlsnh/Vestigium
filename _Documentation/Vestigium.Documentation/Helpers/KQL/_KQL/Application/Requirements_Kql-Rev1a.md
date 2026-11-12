# Vestigium.Helpers.Kql — Requirements Specification

**Document ID:** VEST-HLP-KQL-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_Kql_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Kql`  
**APPID:** `Kql`  
**EventIds:** reserved

Binding contract for Kql as staged in the Vestigium monorepo.

---

## 0. Purpose

KQL filter and search core. Hosts hand it rows; it applies a KQL expression and returns the match set. It does not talk to a store.

---

## 1. Surface

- Parse a KQL expression.
- Filter a row set.
- Report parse errors without throwing on a bad filter when asked.

---

## 2. Rules that do not move

- No store.
- Expression is caller-supplied.
- Does not invent columns.

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

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.Kql.Tests`.

---

## 6. Non-goals

- Query engine over a database.
- UI.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
