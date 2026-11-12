# Vestigium.Helpers.Network.RawSocket — Requirements Specification

**Document ID:** VEST-HLP-NETWORK-RAWSOCKET-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_Network.RawSocket_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Network.RawSocket`  
**APPID:** `Network.RawSocket`  
**EventIds:** reserved

Binding contract for Network.RawSocket as staged in the Vestigium monorepo.

---

## 0. Purpose

Raw socket helpers for hosts that need them. Not a packet sniffer product.

---

## 1. Surface

- Open a raw socket.
- Send / receive a frame.

---

## 2. Rules that do not move

- Requires elevation where the OS demands it.
- Depends on the Network core.

---

## 3. Logging

The library never calls `VestigiumLogger.Initialize` and never chooses a log folder. Hosts that want a disk trace call `Initialize` and the library catalog `Register`.

Writes are no-ops until the host initializes. JSONL lands at `%ProgramData%\Vestigium\Logs\{host-APPID}\`.

---

## 4. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0` |
| Depends on | Vestigium.Helpers.Network, Vestigium.Logging |
| License | MIT |

---

## 5. Tests

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.Network.RawSocket.Tests`.

---

## 6. Non-goals

- Full capture UI.
- Protocol decode suite.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
