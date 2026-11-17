# PR01a.06 — Catalog

**Step:** 06  
**Delivers:** Host catalog default, optional overlay  
**Requirements:** R-09

## Change

Mini uses the host catalog and taxonomy by default. A caller may opt in to a catalog overlay for that Mini only.

## Technical detail

- Default path resolves events against the host catalog.
- Overlay is optional and merged for that instance only. It does not change the host catalog.

## Done when

- [ ] Mini resolves events from the host catalog when no overlay is supplied.
- [ ] Overlay events are visible only to the Mini that received them.
