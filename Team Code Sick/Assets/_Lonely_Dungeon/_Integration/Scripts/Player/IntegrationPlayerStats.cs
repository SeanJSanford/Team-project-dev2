using UnityEngine;

public class IntegrationPlayerStats : MonoBehaviour, IDamage
{
    [Header("Health")]
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentHP;

    [Header("Combat Stats")]
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private float fireRateModifier = 1f;

    [Header("Movement Stats")]
    [SerializeField] private float moveSpeedModifier = 1f;

    public int MaxHP => maxHP;
    public int CurrentHP => currentHP;
    public int BaseDamage => baseDamage;
    public float FireRateModifier => fireRateModifier;
    public float MoveSpeedModifier => moveSpeedModifier;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log("Player HP: " + currentHP + " / " + maxHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log("Player healed. HP: " + currentHP + " / " + maxHP);
    }

    public void AddDamage(int amount)
    {
        baseDamage += amount;
        Debug.Log("Player damage increased to: " + baseDamage);
    }

    public void AddMaxHP(int amount)
    {
        maxHP += amount;
        currentHP += amount;

        Debug.Log("Player max HP increased to: " + maxHP);
    }

    public void AddFireRateModifier(float amount)
    {
        fireRateModifier += amount;
        fireRateModifier = Mathf.Max(0.1f, fireRateModifier);

        Debug.Log("Fire rate modifier: " + fireRateModifier);
    }

    public void AddMoveSpeedModifier(float amount)
    {
        moveSpeedModifier += amount;
        moveSpeedModifier = Mathf.Max(0.1f, moveSpeedModifier);

        Debug.Log("Move speed modifier: " + moveSpeedModifier);
    }

    private void Die()
    {
        Debug.Log("Player died.");

        // Later: connect this to GameOverManager or respawn logic
        gameObject.SetActive(false);
    }
}