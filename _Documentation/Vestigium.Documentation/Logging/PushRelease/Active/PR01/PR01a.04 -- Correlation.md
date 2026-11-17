# PR01a.04 — Correlation

**Step:** 04  
**Delivers:** Application and Mini correlation ids  
**Requirements:** R-04

## Change

Carry an application correlation id (caller-supplied) and a Mini correlation id (generated or accepted) on every Mini line.

## Technical detail

- Application correlation id is optional and passed in.
- Mini correlation id is generated when not supplied.
- Both appear on every Mini line.

## Done when

- [ ] Both ids appear on Mini lines.
- [ ] Generated Mini id is stable for the instance.
- [ ] Caller-supplied Mini id is used when provided.
