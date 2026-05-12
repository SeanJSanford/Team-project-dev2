
public interface IDamageable
{
    // Called when an object receives damage.
    // The implementing class decides how damage is processed.
    void TakeDamage(int amount);
}

/*
========================================================
Project: Team Code Sick
Script: IDamageable.cs

Primary Developer:
- Sean

Integration Support:
- Avery Wilson

Original Framework Source:
- Inspired by Full Sail lecture combat architecture

System Category:
- Combat Interface
- Shared Gameplay Architecture

Purpose:
- Shared interface used by any object
  capable of receiving damage.

Why This Exists:
Interfaces allow combat systems to interact
with multiple gameplay objects generically
without hardcoded class dependencies.

This allows systems like:
- Weapons
- Projectiles
- Enemy attacks
- Hazards
- Traps

to damage:
- Players
- Enemies
- Bosses
- Breakable objects

using the same workflow.

Connected Team Systems:
- Sean: Combat systems / enemy combat interactions
- Avery: PlayerStats / EnemyHealth integration
- Heather: Future destructible loot objects
- Dai: Gameplay interaction testing
- Nilo: Overall gameplay architecture oversight

Design Benefits:
- Reduces code duplication
- Supports modular gameplay systems
- Improves scalability
- Simplifies future enemy expansion
- Keeps combat systems generic

Example Workflow:
Weapon -> Collision/Raycast ->
Check for IDamageable ->
Apply TakeDamage()

Design Philosophy:
This interface intentionally separates:
- Combat interaction
from:
- Object-specific health logic

Each gameplay object determines how it handles:
- Damage
- Death
- Reactions
- Effects
- Destruction

This keeps combat systems modular and reusable.

Development Notes:
- Became the shared combat standard replacing
  earlier prototype-specific damage interfaces
  like AW_IDamage.
- Intended to unify all future combat interactions
  across the project.
- Supports scalable gameplay architecture
  without hardcoded combat dependencies.

Important:
Any class implementing IDamageable must define
its own TakeDamage() behavior.
========================================================
*/