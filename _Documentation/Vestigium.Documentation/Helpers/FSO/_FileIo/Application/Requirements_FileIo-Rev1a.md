# Vestigium.Helpers.FileIo — Requirements Specification

**Document ID:** VEST-HLP-FILEIO-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_FileIo_Rev-1a.md`  
**Package:** `Vestigium.Helpers.FileIo`  
**APPID:** `FileIo`  
**EventIds:** reserved

Binding contract for FileIo as staged in the Vestigium monorepo.

---

## 0. Purpose

Validated file jobs. Robocopy is the behavior reference; FileIo is the record. This package does not spawn robocopy.exe. Recon fills five size buckets. Default collision is UniqueName (.##). Cap is NameCap — the original dest is never overwritten unless the caller picks Overwrite.

---

## 1. Surface

- Recon into five size buckets.
- Copy / move jobs with collision policy.
- UniqueName default. Overwrite is opt-in.

---

## 2. Rules that do not move

- Does not spawn robocopy.exe.
- Default collision is UniqueName.
- Original dest is never overwritten unless Overwrite is chosen.

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

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.FileIo.Tests`.

---

## 6. Non-goals

- Spawning robocopy.exe.
- Silent overwrite.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
