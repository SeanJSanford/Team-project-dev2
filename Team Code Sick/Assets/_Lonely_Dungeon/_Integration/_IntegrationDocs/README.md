# Team Code Sick — Main Integration Repository

## Project Overview

Team Code Sick is a modular top-down/isometric action prototype built in Unity.

This repository contains the team's shared gameplay integration framework, including:

* Player systems
* Enemy AI
* Combat systems
* Loot systems
* Inventory systems
* Runtime stat architecture
* UI prototypes
* Progression framework prototypes

The project is structured around modular gameplay architecture to reduce Git conflicts and improve long-term scalability.

---

# Team Roles & Contributions

## Nilo — Team Lead / Level Generation / Repository Coordination

### Responsibilities

* Team coordination
* Repository organization
* Merge/branch oversight
* Gameplay direction
* Level generation systems
* Integration oversight

---

## Avery Wilson — Systems Integration / Gameplay Systems

### Responsibilities

* Runtime stat architecture
* Combat scaling systems
* Weapon systems
* Integration/documentation
* Gameplay framework architecture

### Major Systems

* PlayerStats
* WeaponStats
* WeaponData
* StatModifier
* StatType
* PlayerCombat
* PlayerHealthUI
* DevMenu

---

## Heather — Inventory / Loot Systems

### Responsibilities

* Inventory systems
* Loot systems
* Item frameworks
* Pickup systems

### Major Systems

* PlayerInventory
* InventoryItemStack
* _ItemData
* LootDropEntry
* _EnemyLoot
* WorldItemPickup

---

## Dai — Movement / Camera Systems

### Responsibilities

* Player movement
* Camera systems
* Gameplay feel
* Combat movement integration

### Major Systems

* PlayerMovement
* _PlayerCamera

---

## Sean — Combat Framework Support

### Responsibilities

* Damage systems
* Combat workflow support
* Shooting/melee framework integration

### Major Systems

* DamageDealer
* Combat integration support

---

# Repository Structure

```text
Assets/
└── Lonely_Dungeon/
    └── _Integration/
        └── Scripts/

            ├── Bridges/
            │   ├── EnemyLootBridge.cs
            │   ├── InventoryUIBridge.cs
            │   └── PlayerStatUIBridge.cs

            ├── Combat/
            │   └── DamageDealer.cs

            ├── Core/
            │   ├── EventBus.cs
            │   ├── GameManager.cs
            │   ├── SaveManager.cs
            │   └── SceneLoader.cs

            ├── Debug/
            │   ├── DebugLootDropTester.cs
            │   ├── DevMenu.cs
            │   └── SpawnTester.cs

            ├── Enemies/
            │   ├── _EnemyAI.cs
            │   ├── EnemyCombat.cs
            │   ├── EnemyHealth.cs
            │   ├── EnemyMeleeAI.cs
            │   ├── EnemyRangedAI.cs
            │   └── EnemySpawner.cs

            ├── Interfaces/
            │   ├── IDamageable.cs
            │   ├── IInteractable.cs
            │   └── ILootSource.cs

            ├── Inventory/
            │   ├── InventoryItemStack.cs
            │   ├── PlayerInventory.cs
            │   └── PlayerPickupCollector.cs

            ├── Items/
            │   └── _ItemData.cs

            ├── Loot/
            │   ├── _EnemyLoot.cs
            │   ├── LootDropEntry.cs
            │   ├── LootDropper.cs
            │   └── WorldItemPickup.cs

            ├── Player/
            │   ├── _PlayerCamera.cs
            │   ├── PlayerCombat.cs
            │   ├── PlayerEconomy.cs
            │   ├── PlayerHealth.cs
            │   ├── PlayerInventorySlots.cs
            │   └── PlayerMovement.cs

            ├── Progression/
            │   ├── CorruptionManager.cs
            │   ├── PerkManager.cs
            │   ├── RewardManager.cs
            │   └── RunManager.cs

            ├── Stats/
            │   ├── PlayerStats.cs
            │   ├── StatModifier.cs
            │   └── StatType.cs

            ├── UI/
            │   ├── DamagePopupUI.cs
            │   ├── InventoryUI.cs
            │   └── PlayerHealthUI.cs

            └── Weapons/
                ├── WeaponData.cs
                ├── WeaponProjectile.cs
                ├── WeaponRuntime.cs
                └── WeaponStats.cs
```

---

# Core Architecture Philosophy

## Separation of Responsibilities

Each gameplay system is intentionally separated to reduce overlap and improve scalability.

Example:

* EnemyHealth handles HP
* EnemyMeleeAI handles AI behavior
* LootDropper handles loot spawning
* PlayerStats handles runtime stat scaling

This helps:

* Reduce Git conflicts
* Simplify debugging
* Improve maintainability
* Support future feature expansion

---

# Current Gameplay Architecture

## Runtime Stat System

Modifier-based stat framework supporting:

* Flat modifiers
* Additive percentage modifiers
* Multiplicative scaling

Primary Systems:

* PlayerStats
* StatModifier
* StatType
* WeaponStats

---

## Combat Framework

Current combat systems include:

* Mouse aiming
* Raycast shooting
* Runtime weapon scaling
* Shared IDamageable interface
* Modular damage handling

Primary Systems:

* PlayerCombat
* DamageDealer
* WeaponStats
* IDamageable

---

## Loot / Inventory Framework

Data-driven inventory and loot architecture.

Primary Systems:

* PlayerInventory
* WorldItemPickup
* LootDropEntry
* _EnemyLoot
* _ItemData

---

## Camera / Movement Framework

Supports top-down/isometric gameplay.

Primary Systems:

* PlayerMovement
* _PlayerCamera

Features:

* Camera-relative movement
* Sprinting
* Dashing
* Mouse aiming support

---

# Development Notes

## Prototype Status

This project is currently in active gameplay prototype development.

Several systems are:

* temporary,
* experimental,
* or early framework implementations.

Many systems are designed for future expansion.

---

# Current Development Goals

* Complete gameplay integration
* Finalize loot/inventory pipeline
* Expand enemy behaviors
* Implement progression systems
* Improve combat feedback
* Build level generation systems
* Expand UI systems

---

# Future Expansion Plans

## Planned Systems

* Procedural level generation
* Weapon rarity
* Perks/cards
* Corruption scaling
* Boss encounters
* Save/load systems
* Economy systems
* Multiplayer considerations
* Advanced enemy AI
* Event-driven UI updates

---

# Git Workflow Notes

## Important

* Avoid editing unrelated systems
* Pull before pushing changes
* Use clear commit messages
* Keep responsibilities modular

## Branch Structure

* Main = stable integration branch
* Feature branches = active development work
* Merge through review/integration process

---

# Documentation Standards

All major gameplay scripts should include:

* Purpose
* Connected systems
* Design notes
* Responsibilities
* Future expansion ideas
* Team credits

This helps:

* Maintain clarity
* Improve grading visibility
* Reduce onboarding confusion
* Track ownership across systems

---

Team Code Sick
Unity Gameplay Prototype Framework
