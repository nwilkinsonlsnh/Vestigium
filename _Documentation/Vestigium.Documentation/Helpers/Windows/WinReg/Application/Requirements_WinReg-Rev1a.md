# Vestigium.Helpers.WinReg — Requirements Specification

**Document ID:** VEST-HLP-WINREG-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0-windows`  
**Companions:** `DesignSpec_WinReg_Rev-1a.md`  
**Package:** `Vestigium.Helpers.WinReg`  
**APPID:** `WinReg`  
**EventIds:** 16000–16499

Binding contract for WinReg as staged in the Vestigium monorepo.

---

## 0. Purpose

Windows Registry read/write helpers. Not reg.exe. Not regedit. Windows only.

---

## 1. Surface

- RegistryHelper.Local.GetKey.
- Read / write values.
- Hive selection is explicit.

---

## 2. Rules that do not move

- Windows only.
- Does not spawn reg.exe.
- Hive is caller-chosen.

---

## 3. Logging

The library never calls `VestigiumLogger.Initialize` and never chooses a log folder. Hosts that want a disk trace call `Initialize` and the library catalog `Register`.

Writes are no-ops until the host initializes. JSONL lands at `%ProgramData%\Vestigium\Logs\{host-APPID}\`.

---

## 4. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0-windows` |
| Depends on | System.Security.Cryptography.ProtectedData 10.0.0, Vestigium.Logging |
| License | MIT |

---

## 5. Tests

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.WinReg.Tests`.

---

## 6. Non-goals

- reg.exe.
- regedit.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
