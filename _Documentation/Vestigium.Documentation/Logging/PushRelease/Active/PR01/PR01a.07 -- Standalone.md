# PR01a.07 — Standalone

**Step:** 07  
**Delivers:** Runs when host is not initialized  
**Requirements:** R-10

## Change

Mini runs even when `VestigiumLogger` is not initialized. It writes a warning line into its own file and does not throw.

## Technical detail

- No host required.
- Warning line notes that there is no host.
- No start/end markers are written to a host log that does not exist.

## Done when

- [ ] Mini constructs and writes when the host is not initialized.
- [ ] Warning line is present in the Mini file.
- [ ] No exception is thrown.
