# Vestigium.Helpers.Watch.Dns — Requirements Specification

**Document ID:** VEST-HLP-WATCHDNS-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0-windows`  
**Companions:** `DesignSpec_Watch.Dns_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Watch.Dns`  
**APPID:** `Watch.Dns`

Binding contract for Watch.Dns as staged in the Vestigium monorepo.

---

## 0. Purpose

DNS watch. Resolves a name and reports changes. Packet and resolver paths. Does not draw.

---

## 1. Surface

- `WatchPipe` / `WatchRow` — the row the host consumes.
- `ResolverWatch.RunAsync` — resolver path.
- `PacketWatch.RunAsync` — packet path.
- `WatchClock` — cadence.
- `QueryName.TryNormalize` — name validation.
- `DnsMessage.TryRead` — parse a response.

---

## 2. Rules that do not move

- Windows targeting.
- Does not own the timer in a host product.
- Does not draw.
- The library never calls `VestigiumLogger.Initialize`.

---

## 3. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0-windows` |
| Depends on | `Vestigium.Logging` |
| License | MIT |

---

## 4. Non-goals

Full DNS server. Charting.

## Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. |
