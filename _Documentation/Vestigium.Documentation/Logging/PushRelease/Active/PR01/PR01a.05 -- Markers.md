# PR01a.05 — Markers

**Step:** 05  
**Delivers:** Automatic start and end markers  
**Requirements:** R-08

## Change

Write a start marker to the host log on construct and an end marker on dispose, when the host is initialized.

## Technical detail

- Markers are automatic. No opt-in.
- Each marker carries the application correlation id, the Mini correlation id, and the Mini file path.
- Markers are the only Mini-related lines in the host file.

## Done when

- [ ] Start marker appears in the host log on construct.
- [ ] End marker appears on dispose.
- [ ] Both carry the two correlation ids and the Mini file path.
