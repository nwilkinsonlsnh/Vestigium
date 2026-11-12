# Vestigium.Helpers.Xml — Design

**Document ID:** VEST-HLP-XML-DSN-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Binding:** `Requirements_Xml-Rev1a.md` wins on conflict  

This page records *why* Xml is shaped this way. It does not add requirements.

---

## 1. Intent

One helper for payload XML. Platform parser. Session so the host can show Diff before disk changes.

```
Open / Parse / Create → XmlSession
Snapshot → Set* / Insert / Delete → Diff → Commit → Save
OpenMulti for back-to-back documents (read/search only in v1)
```

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| Platform parser only | No custom grammar to certify. |
| RFC 7303 order | BOM, then MIME charset, then declaration, then UTF-8. |
| DTD off / resolver null | PUBLIC DTD URLs must never fetch. |
| Session like Json | Hosts already know Snapshot / Diff / Commit / Save. |
| Open vs OpenMulti | A multi dump is not one document. Fail closed. |
| Never log bodies | Settings XML holds secrets. Diff is `ops=` only. |
| Not FileIo | One path, atomic sibling temp. No UniqueName. |
| Never `Initialize` | Folder follows the host APPID. |

---

## 3. Shape

| File | Role |
|---|---|
| `XmlHelper.cs` | Identity, Probe, Parse, Open, OpenMulti, Create, WriteFile |
| `XmlSession.cs` | Snapshot / Diff / Commit / Save / Search |
| `XmlIO.cs` | Load / split / atomic write |
| `XmlEncoding.cs` | RFC 7303 detect / decode |
| `XmlSearch.cs` | Local-name and attribute filters |
| `XmlMediaType.cs` | `application/xml` family |
| `XmlCatalog` / `XmlEvents` | Logging |

Tests live under `src/Vestigium.Helpers.Tests/`.

---

## 4. Exception policy

| Class | When |
|---|---|
| `ArgumentException` | Blank path, unreadable stream. |
| `XmlException` | Bad document, over maxCharacters, Open of a multi file. |
| `IOException` | Disk. |

Expected exceptions rethrow without `HelperLog.Trap`.

---

## 5. Still out

Custom XML grammar, XInclude, UTF-32, UniqueName, logging values, treating DDF/UPnP as product types.

---

## 6. Document control

| Version | Date | Change |
|---|---|---|
| 1.0 | 21 Sep 2026 | First standalone Design. Matches shipped session. |

---

## Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. Carried forward from the prior library contract. |
