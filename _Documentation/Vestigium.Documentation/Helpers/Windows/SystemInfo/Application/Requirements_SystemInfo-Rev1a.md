# Vestigium.Helpers.SystemInfo — Requirements Specification

**Document ID:** VEST-HLP-SYSINFO-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0-windows`  
**Companions:** `DesignSpec_SystemInfo_Rev-1a.md`  
**Package:** `Vestigium.Helpers.SystemInfo`  
**APPID:** `SystemInfo`

Binding contract for SystemInfo as staged in the Vestigium monorepo.

---

## 0. Purpose

Machine facts and inventory reads. Native readers and CIM doors live here. No text, no files. A failed section is `Fact<T>.Unavailable`, not a throw.

---

## 1. Surface

| Door | Returns |
|---|---|
| `CpuFacts.Host` / `Live` | Topology and live CPU |
| `MemoryFacts.Read` | Memory snapshot |
| `SystemInventory.ReadBase` | ComputerSystem, OperatingSystem, BIOS, Processor |
| `SystemInventory.ReadStorage` | Disk drives, logical disks, partitions, volumes |
| `SystemInventory.ReadNetwork` | Adapters, routes, ARP, listeners, TCP |
| `SystemInventory.ReadSecurity` | Firewall profiles, rules, certificates, drivers |
| `SystemInventory.ReadSoftware` | Installed, hotfixes, features, ODBC, environment, SQL services |
| `PendingReboot.Read` | Pending reboot state |
| `VersionCompare.Status` | Installed vs expected |

`Fact<T>.Ok(value)` / `Fact<T>.Unavailable()`.

---

## 2. Rules that do not move

- Windows only.
- A failed section is `Unavailable`, not a throw.
- No text files. No persistence.
- The library never calls `VestigiumLogger.Initialize`.

---

## 3. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0-windows` |
| Depends on | `System.Management`, `System.ServiceProcess.ServiceController` |
| License | MIT |

---

## 4. Non-goals

Remote WMI as a product. Inventory database. Text export.

## Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. |
