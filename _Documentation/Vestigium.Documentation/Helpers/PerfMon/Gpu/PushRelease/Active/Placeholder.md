No push release is open.

## Structure

A push release is a folder. The parent file is the gate. The letter is a role, not a sequence.

```text
Active/
  PR01/
    PR01 -- Requirements.md
    PR01a -- Implementation.md
    PR01b -- TestSuite.md
    PR01c -- Documentation.md
  PR02/
    PR02 -- Requirements.md
    PR02a -- Implementation.md
Completed/
  PR01/
    (the same files, moved here when the release is accepted)
```

| File | Role |
|---|---|
| `PR01 -- Requirements.md` | The release. Scope, path, and the checklist. RFC form. |
| `a` | Implementation. The change. |
| `b` | Test suite. What proves it. |
| `c` | Documentation. Requirements and design updates. |

Rules:

- The parent is the gate. A release is not done until every slice it checks exists and is filled.
- A letter is a role. `b` is always the test suite. It is never a second implementation.
- A release that needs two implementation slices gets `PR01a` and a later `PR0Na`. It does not spend `b` on implementation.
- A release may not need every slice. The parent says which. An unchecked box is a choice, not an omission.
- Copy the starters from `../_Template` into `Active/PR01` and rename. Do not edit the template.
- When the release is accepted, move the `PR01` folder to `Completed/`. Do not leave it in `Active`.
- `Active` is empty when nothing is open.
- No standing d, e, or f. A one-off note goes inside `a`, or as a named file for that release only.
