# PR01a.01 — IdValidation

**Step:** 01  
**Delivers:** AppId and MiniId rules  
**Requirements:** R-06

## Change

Reject AppId or MiniId that does not contain at least 4 letters (A–Z / a–z), case-insensitive. Digits are allowed. Pure-digit ids are rejected.

## Technical detail

- Validation runs at construction for both the host logger and Mini.
- Count letters only. `PingIQ` passes. `0AB1C2D` passes. `001` fails. `AB1` fails.
- Failure throws `ArgumentException` with the offending id.

## Done when

- [ ] Validator exists and is called from both construction paths.
- [ ] Tests cover letter count, mixed digits, and pure-digit rejection.
