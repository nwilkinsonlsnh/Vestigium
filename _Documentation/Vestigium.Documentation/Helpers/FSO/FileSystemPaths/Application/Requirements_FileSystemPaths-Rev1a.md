# Vestigium.Helpers.FileSystemPaths — Requirements Specification

**Document ID:** VEST-HLP-FSP-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_FileSystemPaths_Rev-1a.md`  
**Package:** `Vestigium.Helpers.FileSystemPaths`  
**APPID:** `FileSystemPaths`

Binding contract for FileSystemPaths as staged in the Vestigium monorepo.

---

## 0. Purpose

Path identity and normalization for Vestigium hosts. Resolves, compares, and classifies filesystem paths without touching file contents.

---

## 1. Surface

- Normalize a path.
- Compare two paths.
- Classify (file / directory / UNC / relative).

---

## 2. Rules that do not move

- Does not read file contents.
- Path identity is the contract.
- The library never calls `VestigiumLogger.Initialize`.

---

## 3. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0` |
| Depends on | `Vestigium.Logging` |
| License | MIT |

---

## 4. Non-goals

File IO. Content hashing. Encryption.

## Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. |
