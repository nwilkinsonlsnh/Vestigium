# PR05 — Requirements

**Application:** Vestigium.Logging  
**Status:** Draft  
**Release:** PR05  
**Slices:** a implementation, b test suite, c documentation

## 1. Summary

Quality of life for the caller. Mini gets the missing levels and an options object. The host gets a category scope and an ambient correlation scope. Nothing here changes what a line contains. It changes how little the caller has to type to produce one.

## 2. Motivation

The surface works. It costs more than it should. Mini has one write method. Its constructor has eleven parameters. Every host call repeats category, subcategory, and correlation id. These are the four gaps that show up in real use.

## 3. Specification

| ID | Requirement |
|---|---|
| R-01 | `VestigiumLoggerMiniOptions` carries the optional Mini settings. The constructor that takes AppId, MiniId, and options is the preferred entry. The long constructor remains. |
| R-02 | Mini exposes `Debug`, `Warning`, `Error`, `Fatal`, and `Thrown` with the same shape as the host. |
| R-03 | `VestigiumLog.Category(category, subcategory)` returns a scope. Writes inside it omit category and subcategory. Nested scopes override. |
| R-04 | `VestigiumLog.Correlation(id)` returns a scope. Writes inside it carry that correlation id unless the call passes one. Nested scopes override. |
| R-05 | Scopes are `AsyncLocal`. They do not leak across tasks that did not inherit the context. |
| R-06 | A call that passes category, subcategory, or correlation id explicitly wins over the scope. |

## 4. Slices

- [ ] PR05a -- Implementation.md
- [ ] PR05b -- TestSuite.md
- [ ] PR05c -- Documentation.md

## 5. Path

| Step | Delivers | Requirements |
|---|---|---|
| PR05a.01 | Mini options object and preferred constructor | R-01 |
| PR05a.02 | Mini Debug, Warning, Error, Fatal, Thrown | R-02 |
| PR05a.03 | Category scope | R-03, R-06 |
| PR05a.04 | Correlation scope | R-04, R-05, R-06 |

## 6. Out of scope

- An analyzer for taxonomy.
- An `ILogger` bridge.
- A properties helper.
- Richer `Initialize` error text.
- XML docs and the NuGet README. Those can follow.

## 7. Revision History

| Revision | Date | Author | Description |
|---|---|---|---|
| 1 | 2026-11-19 | Vestigium | Initial draft from the developer experience review. |
