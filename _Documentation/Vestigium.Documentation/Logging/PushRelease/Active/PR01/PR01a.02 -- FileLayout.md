# PR01a.02 — FileLayout

**Step:** 02  
**Delivers:** Size-based names and rotation  
**Requirements:** R-05, R-07

## Change

Replace date-stamped names with size-based parts under `%ProgramData%\Vestigium\Logging\<AppId>\`.

## Technical detail

- Main active: `<AppId>.jsonl`. Rolled: `<AppId>.001.jsonl`, `<AppId>.002.jsonl`.
- Mini active: `<AppId>.<MiniId>.jsonl`. Rolled: `<AppId>.<MiniId>.001.jsonl`.
- Writer opens the highest existing part under the size limit and appends. Rolls to the next part when the next line would exceed the limit. No date in the name.
- Part number is always pure digits, so it cannot collide with an id that must contain 4 letters.

## Done when

- [ ] Names match the pattern. No date stamp.
- [ ] Size roll produces `.001`, `.002`.
- [ ] Highest under-limit part is reopened on the next run.
