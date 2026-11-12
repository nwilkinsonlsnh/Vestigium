# Vestigium.Helpers.Hashing — Requirements Specification

**Document ID:** VEST-HLP-HASHING-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_Hashing_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Hashing`  
**APPID:** `Hashing`  
**EventIds:** reserved

Binding contract for Hashing as staged in the Vestigium monorepo.

---

## 0. Purpose

Digest and HMAC helpers. Hashing stays a sibling of Encryption. Trailer slots in Encryption stay zeros; digests live here.

---

## 1. Surface

- File and buffer digests.
- HMAC.
- Algorithm selection is explicit.

---

## 2. Rules that do not move

- No encryption.
- Algorithm is caller-chosen.
- Does not log digest inputs that are secrets.

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

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.Hashing.Tests`.

---

## 6. Non-goals

- Encryption.
- Key derivation.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
