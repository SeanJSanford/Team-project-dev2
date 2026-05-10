/*
 * IDamageable
 * 
 * Purpose:
 * Shared combat interface used by any object
 * that can receive damage.
 * 
 * Why Use An Interface:
 * Interfaces allow combat systems to damage multiple
 * object types without needing to know exactly
 * what they are.
 * 
 * Example:
 * A projectile or weapon can damage:
 * - Players
 * - Enemies
 * - Bosses
 * - Breakable objects
 * 
 * without hardcoding references to specific classes.
 * 
 * Benefits:
 * - Reduces code duplication
 * - Makes systems modular
 * - Simplifies future enemy/player expansion
 * - Allows combat systems to stay generic
 * 
 * Example Flow:
 * Weapon -> Raycast Hit -> Check for IDamageable -> Apply Damage
 * 
 * Important:
 * Any class using this interface MUST implement
 * its own TakeDamage() behavior.
 * 
 * Example:
 * EnemyAI may die and drop loot.
 * PlayerStats may trigger death or UI updates.
 */

public interface IDamageable
{
    // Called when an object receives damage.
    // The implementing class decides how damage is processed.
    void TakeDamage(int amount);
}