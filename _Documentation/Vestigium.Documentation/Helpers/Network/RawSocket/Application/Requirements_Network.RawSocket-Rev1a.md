# Vestigium.Helpers.Network.RawSocket — Requirements Specification

**Document ID:** VEST-HLP-RAWSOCK-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_Network.RawSocket_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Network.RawSocket`  
**APPID:** `Network.RawSocket`

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
- The library never calls `VestigiumLogger.Initialize`.

---

## 3. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0` |
| Depends on | `Vestigium.Helpers.Network`, `Vestigium.Logging` |
| License | MIT |

---

## 4. Non-goals

Full capture UI. Protocol decode suite.

## Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. |
