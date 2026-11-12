# Vestigium.Helpers.Encryption — Requirements Specification

**Document ID:** VEST-HLP-ENCRYPTION-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_Encryption_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Encryption`  
**APPID:** `Encryption`  
**EventIds:** 12000–12499

Binding contract for Encryption as staged in the Vestigium monorepo.

---

## 0. Purpose

Authenticated encryption for strings and files. AES-256-GCM default, ChaCha20-Poly1305 opt-in, AES-256-CBC+HMAC interop, Argon2id passphrases, RSA-OAEP wrap of the content key. Not TLS, BitLocker, DPAPI, or a key vault. Wrong secret or a bit flip fails closed.

---

## 1. Surface

- SealString / OpenString — same envelope as files, Base64.
- SealFile / OpenFile — 64 KiB frames. Never ReadAllBytes the payload.
- IsVestigiumFile / PeekFile / ValidateFile / RevealOriginalFileName.
- EncryptionSecret.FromPassphrase / FromKey. Dispose clears.
- EncryptionRsaKey.Generate — wraps the 32-byte content key only.
- EncryptionKeyRing — pairs vs contacts. Enable / Disable / Expire / Retire / Compromise.
- EncryptionCatalog.Register(cfg).

---

## 2. Rules that do not move

- No custom primitives. BCL AEAD / RSA-OAEP. One approved Argon2 package.
- Never CBC without HMAC. Never RSA on the file body.
- Fail closed. Do not distinguish wrong password from corrupt from wrong company key.
- Never log plaintext, keys, passphrases, PKCS8, issuedTo, or full thumbprints.
- Large files stream. Do not encrypt in place.
- Hashing / HMAC digests belong in Vestigium.Helpers.Hashing.

---

## 3. Logging

The library never calls `VestigiumLogger.Initialize` and never chooses a log folder. Hosts that want a disk trace call `Initialize` and the library catalog `Register`.

Writes are no-ops until the host initializes. JSONL lands at `%ProgramData%\Vestigium\Logs\{host-APPID}\`.

---

## 4. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0` |
| Depends on | Konscious.Security.Cryptography.Argon2 1.3.1, Vestigium.Logging |
| License | MIT |

---

## 5. Tests

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.Encryption.Tests`.

---

## 6. Non-goals

- TLS.
- BitLocker.
- DPAPI.
- Key vault.
- Hashing digests.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
