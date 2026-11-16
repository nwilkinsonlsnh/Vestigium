No push release is open.

## Structure

A push release is a folder. The letter is a role, not a sequence.

```text
Active/
  PR01/
    PR01a -- Implementation.md
    PR01b -- TestSuite.md
    PR01c -- Documentation.md
  PR02/
    PR02a -- Implementation.md
Completed/
  PR01/
    (the same files, moved here when the release is accepted)
```

| Folder | What it is |
|---|---|
| `PR01`, `PR02` | One push release. Many may be open. |
| `a` | Implementation. Technical detail and the change. |
| `b` | Test suite. What proves the change. |
| `c` | Documentation. Requirements and design updates. |

Rules:

- A letter is a role. `b` is always the test suite. It is never a second implementation.
- A release that needs two implementation slices gets `PR01a` and a later `PR0Na`. It does not spend `b` on implementation.
- Copy the starters from `../_Template` into `Active/PR01` and rename. Do not edit the template.
- When the release is accepted, move the `PR01` folder to `Completed/`. Do not leave it in `Active`.
- `Active` is empty when nothing is open.
