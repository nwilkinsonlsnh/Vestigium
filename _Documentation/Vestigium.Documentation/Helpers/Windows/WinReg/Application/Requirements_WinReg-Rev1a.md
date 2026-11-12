# Vestigium.Helpers.WinReg — Requirements Specification

**Document ID:** VEST-HLP-WINREG-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0-windows`  
**Companions:** `DesignSpec_WinReg_Rev-1a.md`  
**Package:** `Vestigium.Helpers.WinReg` 1.0.0  
**APPID:** `WinReg`  
**EventIds:** 16000–16499

Binding contract for WinReg as staged in the Vestigium monorepo.

---

## 0. Purpose

Windows Registry read/write helpers. Not `reg.exe`. Not `regedit`. Windows only.

---

## 1. Surface

| Call | Returns | Notes |
|---|---|---|
| `RegistryHelper.Local` / `For(machine)` / `CanConnect` | client | ConnectTimeout default 3 s. |
| `GetKey` / `GetValue` / `ListSubKeys` | info or null | Missing is null. `/` throws. |
| `Search` | hits | Cap 256 / depth 32. |
| Writes (`SetValue`, `DeleteKey`, …) | `RegistryWriteResult` | Need `confirm: true`. |
| `Export` / `Import` | result | |
| `MountHive` / `DismountHive` | mount / result | HKLM or HKU, local, confirm. |
| `WriteIndex` / `Compare` | index / diff | Offline JSONL. No payloads in the index. |
| `WinRegCatalog.Register(cfg)` | void | Host-only. |

---

## 2. Rules that do not move

- Windows only. No `reg.exe` / `regedit` spawn.
- Writes need `confirm: true`. Results, not surprise exceptions, on deny.
- Logs path + value **name** only. Never log value data.
- Journal is `vest-regjnl/1`. Compact / Purge fail `InUse` while open.
- Compare is index vs index. Restore without payload is `Unsupported`.
- The library never calls `VestigiumLogger.Initialize`.

---

## 3. Logging

Hosts that want a disk trace call `Initialize` and `WinRegCatalog.Register`. Writes are no-ops until then. JSONL lands at `%ProgramData%\Vestigium\Logs\{host-APPID}\`.

---

## 4. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0-windows` |
| Depends on | `System.Security.Cryptography.ProtectedData` 10.0.0, `Vestigium.Logging` |
| License | MIT |

---

## 5. Non-goals

`reg.exe`. `regedit`. Remote write without `CanConnect`. Logging value data.

## Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. |
