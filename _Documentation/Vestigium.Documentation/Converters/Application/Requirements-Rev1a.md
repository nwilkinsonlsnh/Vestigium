# Converters — Requirements Specification

**Application:** Converters
**Document ID:** VEST-CONV-SRS-000
**Version:** 1a
**Status:** Staging — monorepo release
**Date:** 15 November 2026
**Target:** .NET 10 LTS / `net10.0-windows` / WPF
**Companions:** `DesignSpec-Rev-1a.md`
**Project:** `Converters/Vestigium.Converters`

Binding contract for Vestigium.Converters as staged in the Vestigium monorepo. Rewritten from the source repo requirements.

---

## 0. Purpose

A DI-driven catalog of WPF `IValueConverter` and `IMultiValueConverter` implementations. A host registers the catalog once, assigns `VestigiumConverterHost.ServiceProvider`, and resolves converters from XAML by type. Views do not declare a converter resource per page.

---

## 1. Scope

### 1.1 In scope (1a)

| ID | Capability |
|---|---|
| S-01 | One registration path for the catalog |
| S-02 | Markup extension resolves a converter by type |
| S-03 | State-free converters are singleton |
| S-04 | Designer-safe when no host provider is present |
| S-05 | Domain converters accept enum, string, or number and reference no host assembly |

### 1.2 Out of scope (1a)

- A demo executable.
- Host-specific converters that reference PingIQ, DnsIQ, or TraceIQ.

---

## 2. Binding

| Rule | Behavior |
|---|---|
| This library wins on conversion | Brush, text, and numeric conversion for a bound value. |
| The host wins on registration | The host builds the provider and assigns it. |
| No host assembly reference | A converter must not take a dependency on a suite host. |

---

## 3. Revision History

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 15 Nov 2026 | Nathaniel Wilkinson | Initial Vestigium release. |
