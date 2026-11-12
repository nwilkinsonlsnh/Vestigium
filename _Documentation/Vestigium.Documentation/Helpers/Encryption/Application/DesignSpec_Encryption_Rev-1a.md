# Vestigium.Helpers.Encryption — Design Specification

**Document ID:** VEST-HLP-ENCRYPTION-DSN-000  
**Version:** 1a  
**Status:** Staging companion to Requirements 1a  
**Date:** 11 November 2026  
**Binding:** `Requirements_Encryption-Rev1a.md` wins on conflict

Why the library is shaped this way. It does not add requirements.

---

## 1. Intent

Authenticated encryption for strings and files. AES-256-GCM default, ChaCha20-Poly1305 opt-in, AES-256-CBC+HMAC interop, Argon2id passphrases, RSA-OAEP wrap of the content key. Not TLS, BitLocker, DPAPI, or a key vault. Wrong secret or a bit flip fails closed.

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| No custom primitives | Locked. |
| Never CBC without HMAC | Locked. |
| Fail closed | Locked. |
| Never log plaintext, keys, passphrases, PKCS8, issuedTo, or full thumbprints | Locked. |
| Large files stream | Locked. |
| Hashing / HMAC digests belong in Vestigium | Locked. |
| Logging APPID stamps library identity | Folder follows the host. |
| No host adapter | Orchestration is a different project. |

---

## 3. Dependencies

| Package | Role |
|---|---|
| Konscious.Security.Cryptography.Argon2 1.3.1 | Primary |
| Vestigium.Logging | JSONL writer. This library never initializes it. |

---

## 4. Still out

- TLS.
- BitLocker.
- DPAPI.
- Key vault.
- Hashing digests.

---

## 5. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
