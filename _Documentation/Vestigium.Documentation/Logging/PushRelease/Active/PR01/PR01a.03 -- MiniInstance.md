# PR01a.03 — MiniInstance

**Step:** 03  
**Delivers:** Instanced type, own file, own flood  
**Requirements:** R-01, R-02, R-03

## Change

Add an instanced Mini logger that writes only to its own file and owns its own flood tracker.

## Technical detail

- New type `VestigiumLoggerMini`. Constructed with AppId and MiniId.
- Not static. Each instance is independent.
- Own `FloodTracker`. Does not share state with the host or other Mini instances.
- Lines go only to the Mini file. Host lines never appear in a Mini file.

## Done when

- [ ] Mini can be constructed and written to.
- [ ] Mini file contains only Mini lines.
- [ ] Flooding one Mini does not suppress the host or another Mini.
