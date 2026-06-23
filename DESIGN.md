# EPTools.Core Design Document

Eclipse Phase 2e character creation tool — architecture reference and conventions.

---

## Purpose

EPTools is a character creation and management tool for the Eclipse Phase 2e tabletop RPG. The Core library provides the domain model, game data access, and character generation logic consumed by multiple front-ends (Desktop, Blazor, CLI).

---

## Conceptual Model

Eclipse Phase characters have two orthogonal dimensions:

- **Ego** — the persistent mind: aptitudes, skills, traits, rep, memories, psi. Survives death and morph changes.
- **Morph** — the physical body: pools, wares, morph traits, durability. Temporary; can be swapped.

This distinction is foundational and must be reflected clearly in every layer: naming, namespace, service methods, and DTOs.

---

## Layer Responsibilities

```
┌─────────────────────────────────────────────┐
│  Front-ends (Desktop / Blazor / CLI)        │
│  Own: ViewModels, Presenters, Components    │
└────────────────┬────────────────────────────┘
                 │  depends on
┌────────────────▼────────────────────────────┐
│              EPTools.Core                   │
│                                             │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐ │
│  │ Services │  │  Models  │  │Interfaces│ │
│  └──────────┘  └──────────┘  └──────────┘ │
│  ┌──────────────────────────────────────┐  │
│  │         Extensions / Constants       │  │
│  └──────────────────────────────────────┘  │
└─────────────────────────────────────────────┘
```

**Services** — all business logic lives here. No logic in models; no logic in front-ends.
**Models** — pure data. Ego models are mutable (character state). Data models are immutable (game rules).
**Interfaces** — define service contracts. Front-ends depend on interfaces, never concrete types.

---

## Namespace Layout

```
EPTools.Core
├── Models
│   ├── Ego/              ← mutable character state
│   ├── Data/             ← immutable game data
│   └── LifePathGen/      ← generation session models (ephemeral)
├── Services/
│   ├── EpDataService.cs  (implements IEpDataService)
│   ├── EgoService.cs     (implements IEgoService)
│   ├── EgoManager.cs     (no interface yet — see action items)
│   └── LifepathService.cs (implements ILifepathService)
├── Interfaces/           ← all service contracts
├── Extensions/
└── Constants/
```

---

## Model Design Rules

### Two distinct model families

#### 1. Ego Models (`Models/Ego/`)
Represent a **live character instance**. Mutable classes because character state changes during creation and play.

- Named with `Ego` prefix when the ego owns it: `EgoSkill`, `EgoTrait`, `EgoAptitude`
- Named with the thing itself when scoped to a morph: `Morph`, `Ware`
- Collections are `List<T>` — they change over time

#### 2. Data Models (`Models/Data/`)
Represent **game rules loaded from data files**. Read-only reference data — never mutated at runtime.

- Use `record` (enforces value equality and immutability)
- Named after the game concept: `Skill`, `MorphTemplate`, `Trait`
- Suffix `Template` when the record is a blueprint that gets instantiated onto an `Ego` model: `MorphTemplate` → `Morph`

**Rule:** If a type is loaded from a JSON data file, it is a data model. If it lives on an `Ego` object, it is an ego model. Never conflate the two — a `Trait` record from data becomes an `EgoTrait` on the character.

#### 3. Character Generation Models (`Models/LifePathGen/`)
Represent the **state of a generation session**. Mutable but ephemeral — not persisted. `LifepathContext` holds the execution stack; `LifePathNode` is the data record.

---

### Type selection guide

| Scenario | Use |
|---|---|
| Game data loaded from JSON | `record` |
| Character state on `Ego` | `class` |
| Lifepath generation node definition | `record` |
| Lifepath generation session state | `class` |
| Service/repository contract | `interface` |

---

### Calculated properties

Derived stats (Initiative, Lucidity, TraumaThreshold, etc.) are calculated in `EgoManager` methods, not stored on `Ego`. They take the active `Morph?` as a parameter when morph modifiers apply.

```csharp
// In EgoManager — takes optional morph for modifier aggregation
public int GetInitiative(Ego ego, Morph? activeMorph = null)
public int GetLucidity(Ego ego)
public int GetSkillTotal(Ego ego, EgoSkill skill, Morph? activeMorph = null)
```

`RollModifier` lists on `Ego` and `Morph` feed these calculations. `IsActive` controls whether a modifier is currently applied. `Source` (nullable string) tracks where the modifier came from — null means user-entered, non-null means a trait or ware name.

---

## Naming Conventions

| Concept | Convention | Example |
|---|---|---|
| Data model (game rule) | `NounTemplate` or plain `Noun` | `MorphTemplate`, `Faction`, `Skill` |
| Ego model (character state) | `EgoNoun` or `Noun` under `Ego` | `EgoSkill`, `Morph` |
| Service interface | `INounService` or `IEpDataService` | `IEgoService`, `IEpDataService` |
| Skill type discriminator | Enum, not magic string | `SkillType.KnowledgeSkill` |
| Node type discriminator | Enum, not magic string | `LifePathNodeType.Skill` |

---

## Service Layer Rules

### Separation of concerns

| Service | Owns |
|---|---|
| `EpDataService` | Loading and caching game data from files/HTTP. No character logic. |
| `EgoService` | Creating a fresh `Ego` from defaults. No mutation after creation. |
| `EgoManager` | All character mutations post-creation. Validates EP rules. |
| `LifepathService` | Random character generation. Delegates mutations to `EgoManager`. |

### EgoManager is the single mutation gate

All changes to an `Ego` or `Morph` go through `EgoManager`. No service, viewmodel, or component mutates `Ego` properties directly. This is where EP2e rules are enforced:

- Minimum 1 identity
- Valid aptitude ranges
- Skill type classification
- Equipped/Active flags reset when items move to cache

**Front-ends must not call `.Add()`, `.Remove()`, or `.Clear()` on any `Ego` collection directly.**

### Data service is read-only cache

`EpDataService` loads from files/HTTP once and caches. It never writes. User-created templates go through `IUserDataStore`.

---

## RollModifier System

Two separate lists: `Ego.RollModifiers` and `Morph.RollModifiers`. EgoManager aggregates both when calculating totals.

- `IsActive` — toggle without removing; supports situational modifiers
- `Source` — nullable string; null = manually entered, non-null = trait/ware name that created it
- Remove by source with `RemoveEgoModifiersBySource` / `RemoveMorphModifiersBySource` when a trait or ware is removed

**AptitudeCheck modifiers do not apply to skill checks.** They modify only raw aptitude check rolls (COG check, INT check, etc.). `GetSkillTotal` does not aggregate `RollModifierType.AptitudeCheck`.

---

## Skill Classification

Skill type is determined by the `Skill` data record loaded from the catalog, not by parsing the skill name.

| Template flag | SkillType |
|---|---|
| `Know == true` | `SkillType.KnowledgeSkill` |
| `Name.StartsWith("Exotic Skill")` | `SkillType.ExoticSkill` |
| (default) | `SkillType.EgoSkill` |

`EgoSkill.SkillType` is set at creation time from the template and does not change.

---

## LifePath Node Generation

Stack-based iterative evaluation. `LifepathContext.Nodes` is a `Stack<LifePathNode>`.

- `OptionLists` with total weight > 0 → pick one weighted item
- `OptionLists` with total weight == 0 → push all items
- `PushAll` reverses the list before pushing so first item processes first (LIFO)
- Skip logic: `CharacterGenStep` nodes with a matching `Value` in `ctx.SkipSections` are skipped; the skip is registered by a preceding `Skip` node
- Table nodes push the selected result back onto the stack — it is dispatched normally through the type system

---

## Serialization

`Ego` serializes to/from JSON with `System.Text.Json`. The polymorphic `Gear` hierarchy is handled with:

```csharp
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(GearWare), "ware")]
[JsonDerivedType(typeof(GearArmor), "armor")]
// ... all 17 concrete types
public abstract class Gear : EpModel { }
```

`InventoryItem.BaseGear` round-trips correctly because of this. Character save/load works across all front-ends.

---

## Inventory and Gear

`InventoryItem` wraps a `Gear` instance from the catalog.

- `BaseGear` is the canonical source for name/description/stats
- `InventoryItem` owns: `Quantity`, `Equipped`, `Active`, `Notes`, `InstanceId`
- Display reads from `BaseGear`; per-character state lives on `InventoryItem`
- When items move to a cache, `Equipped` and `Active` are reset to `false`

`InventoryCache` holds items and a `Morphs` list (for stashed morphs). The morph-in-cache feature has no `EgoManager` API yet — see action items.

---

## Reputation

Rep networks belong to an `Identity`. The lifepath generator always awards rep to `Identities[0]` (correct per EP2e rules). Manual rep editing goes through `EgoManager.AddReputation(Identity identity, string network, int amount)` with the identity passed explicitly.

---

## What "Done" Looks Like for a Feature

A character operation (e.g., "add a trait") is complete when:

1. `EgoManager` has a method for it that enforces EP2e rules
2. `IEpDataService` can supply the data needed (trait catalog)
3. Tests cover the happy path and any rule-enforcement edge cases
4. The UI consumes only `EgoManager` + `IEpDataService` — no direct `Ego` mutation

---

## Current State (as of June 2026)

### What's fully done

- `EgoManager` as the single mutation gate — 40+ public methods covering skills, morphs, traits, psi, identities, inventory, caches, roll modifiers, and calculations
- `IEgoManager` interface extracted; all consumers depend on the abstraction
- `LifepathService` routes all mutations through `IEgoManager`
- `LifePathNode.Type` is an enum (`LifePathNodeType`) — no magic strings; all 40+ type values present in data files are now declared, with `[JsonStringEnumMemberName]` for multi-word values
- Skill type derived from template flags, not string parsing
- `[JsonPolymorphic]` on `Gear` with all 17 derived types registered
- Desktop save/load/export/import working
- Blazor save/load/export/import working
- Interfaces extracted: `IEgoManager`, `IEgoService`, `ILifepathService`, `IEpDataService`, `IRandomizer`, `IFetchService`, `IUserDataStore`, `IWeightedItem`
- Dead code removed: `NpcService`, `DiscordWebhookService`, `CalculateSkillTotal`
- Blazor front-end fully routed through `IEgoManager`: `CharacterSkills`, `CharacterPsi`, `CharacterTraits`, `CharacterGear`
- Desktop ViewModels fully routed through `IEgoManager`: `MorphViewModel`, `InventoryCacheViewModel`, `MainWindowViewModel`
- Morph stash implemented: `AddMorphToCache` / `RemoveMorphFromCache` on `IEgoManager` + UI in `CharacterGear.razor`
- 270 tests passing (88 Core + 182 Desktop)
  - `LifepathServiceTests.cs` — 31 tests covering all `Apply*` dispatch paths, skip logic, option list selection, table dispatch
  - `EpDataServiceTests.cs` — 46 tests covering all 17 gear types, round-trip polymorphic deserialization, caching, core data files

### What's missing / possible future work

- Lifepath `Apply*` handlers for new `LifePathNodeType` values (`Disorder`, `PsiAbility`, `ReputationNetwork`, `SkipAge`, etc.) — currently ignored silently
- Integration test coverage for LifepathService against real data files (currently uses stubs)
- `CharacterGenStep` skip targeting by section ID — only basic skip logic tested

---

## Action Items

*(All prior items completed — see git log for details.)*

---

## Resolved Decisions

1. **Serialization format** — `System.Text.Json` throughout. `[JsonPolymorphic]` / `[JsonDerivedType]` on `Gear` handles polymorphic `InventoryItem.BaseGear`. Fully implemented.

2. **`EgoTrait` owns its data** — When a trait is applied (from catalog or lifepath), all fields are copied onto `EgoTrait`. No live link back to the catalog `Trait`. A data update never silently alters an existing character.

3. **Multi-identity rep** — Lifepath always awards rep to `Identities[0]`. Manual rep editing passes the identity explicitly to `EgoManager.AddReputation(Identity, string, int)`.

4. **Custom templates** — `IEpDataService` is read-only official data. Custom content goes through `IUserDataStore`. UIs that need both call both services and merge at the presentation layer.

5. **Immutable copy at application** — When catalog data is applied to a character (morph, trait, gear), a mutable copy is made. The user can freely edit their copy; it is not linked to the source.

6. **AptitudeCheck modifiers** — Do not affect skill checks. `GetSkillTotal` aggregates only `RollModifierType.Skill` modifiers. `GetAptitudeCheckTotal` aggregates only `RollModifierType.AptitudeCheck`.

7. **Two-list roll modifiers** — `Ego.RollModifiers` and `Morph.RollModifiers` are separate lists. Morph computed properties (`TotalVigor`, etc.) are self-contained. Cross-list aggregation for derived stats is centralized in `EgoManager`.

8. **`LifePathNodeType` completeness** — The enum declares all ~40 type values present in data files, including structural meta-types (ignored by `LifepathService`), data typos as aliases (`Slight` → `ApplySleight`, `Trail` → `ApplyTrait`), and multi-word values using `[JsonStringEnumMemberName]`. Data typos `forcedInterest` and `Trail` are fixed at source; `Slight` is handled via enum alias to avoid touching 27+ data nodes.
