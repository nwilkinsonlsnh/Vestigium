# Vestigium.Helpers.FileSystemPaths — Requirements Specification

**Document ID:** VEST-HLP-FILESYSTEMPATHS-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_FileSystemPaths_Rev-1a.md`  
**Package:** `Vestigium.Helpers.FileSystemPaths`  
**APPID:** `FileSystemPaths`  
**EventIds:** reserved

Binding contract for FileSystemPaths as staged in the Vestigium monorepo.

---

## 0. Purpose

Path identity and normalization for Vestigium hosts. Resolves, compares, and classifies filesystem paths without touching file contents.

---

## 1. Surface

- Normalize.
- Compare.
- Classify (file / directory / UNC / relative).

---

## 2. Rules that do not move

- Does not read file contents.
- Path identity is the contract.

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

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.FileSystemPaths.Tests`.

---

## 6. Non-goals

- File IO.
- Content hashing.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
