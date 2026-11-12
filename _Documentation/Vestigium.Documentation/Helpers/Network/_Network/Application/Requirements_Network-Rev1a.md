# Vestigium.Helpers.Network — Requirements Specification

**Document ID:** VEST-HLP-NETWORK-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_Network_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Network`  
**APPID:** `Network`  
**EventIds:** reserved

Binding contract for Network as staged in the Vestigium monorepo.

---

## 0. Purpose

Network helpers: DNS, ports, routes, and connectivity probes that do not need raw sockets.

---

## 1. Surface

- Resolve.
- Port probe.
- Route lookup.

---

## 2. Rules that do not move

- No raw sockets. Those live in Network.RawSocket.
- Does not capture traffic.

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

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.Network.Tests`.

---

## 6. Non-goals

- Packet capture.
- Raw sockets.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
