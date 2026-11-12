# Vestigium.Helpers.Watch.Dns — Requirements Specification

**Document ID:** VEST-HLP-WATCH-DNS-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0-windows`  
**Companions:** `DesignSpec_Watch.Dns_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Watch.Dns`  
**APPID:** `Watch.Dns`  
**EventIds:** reserved

Binding contract for Watch.Dns as staged in the Vestigium monorepo.

---

## 0. Purpose

DNS watch. Resolves a name on a cadence and reports changes. Does not draw.

---

## 1. Surface

- Watch a name.
- Report the current answer.
- Signal a change.

---

## 2. Rules that do not move

- Windows targeting.
- Does not own the timer in a host product.
- Does not draw.

---

## 3. Logging

The library never calls `VestigiumLogger.Initialize` and never chooses a log folder. Hosts that want a disk trace call `Initialize` and the library catalog `Register`.

Writes are no-ops until the host initializes. JSONL lands at `%ProgramData%\Vestigium\Logs\{host-APPID}\`.

---

## 4. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0-windows` |
| Depends on | Vestigium.Logging |
| License | MIT |

---

## 5. Tests

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.Watch.Dns.Tests`.

---

## 6. Non-goals

- Full DNS server.
- Charting.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
