using UnityEngine;

public class DifficultyRampUp : MonoBehaviour
{

    public static DifficultyRampUp instance;

    [Range(1f, 2f)] [SerializeField] float EnemySpawnRamp;
    [Range(1f, 2f)] [SerializeField] float EnemyHPRamp;
    [Range(1f, 2f)] [SerializeField] float EnemyDmgRamp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    public float EnemySpawnRampUp(float baseValue)
    {
        return baseValue * Mathf.Pow(EnemySpawnRamp, EnemySpawnsCenter.instance.roomDifficulty);
    }

    public float EnemeyHPRampUp(float baseValue)
    {
        return baseValue * Mathf.Pow(EnemyHPRamp, EnemySpawnsCenter.instance.roomDifficulty);
    }
    public float EnemeyDamageRampUp(float baseValue)
    {
        return baseValue * Mathf.Pow(EnemyDmgRamp, EnemySpawnsCenter.instance.roomDifficulty);
    }
}
