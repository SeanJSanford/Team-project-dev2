using UnityEngine;
using System.Collections.Generic;

public class DifficultyRampUp : MonoBehaviour
{

    public static DifficultyRampUp instance;

    [Range(1f, 2f)] [SerializeField] float EnemySpawnRamp;
    [Range(1f, 2f)] [SerializeField] float EnemyHPRamp;
    [Range(1f, 2f)] [SerializeField] float EnemyDmgRamp;

    [Range(1, 5)][SerializeField] int enemySpawnWaves;
    [Range(1, 5)][SerializeField] int enemyHPWaves;
    [Range(1, 5)][SerializeField] int enemyDMGWaves;

    public int difficultyBoost = 0;

    enum Difs
    {
        SPAWNS,
        HP,
        DMG
    }

    List<int> difficulties = new List<int> { 1, 0, 0 };
    List<int> rotations;

    int index = 0;
    int sum = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        rotations = new List<int> { enemySpawnWaves, enemyHPWaves, enemyDMGWaves };
    }

    public void Dif()
    {
        int value = 0;

        for (int i = 0; i <= index; i++)
        {
            value += rotations[i];
        }

        if (sum < value)
            sum += rotations[index];

        if (EnemySpawnsCenter.instance.roomDifficulty >= sum)
        { 
            index++;
            if (index == rotations.Count)
                index = 0;
        }

        difficulties[index % 3]++;
    }

    public float EnemySpawnRampUp(float baseValue)
    {
        return baseValue * Mathf.Pow(EnemySpawnRamp + (difficultyBoost / 100), difficulties[(int)Difs.SPAWNS]);
    }

    public float EnemyHPRampUp(float baseValue)
    {
        return baseValue * Mathf.Pow(EnemyHPRamp + (difficultyBoost / 100), difficulties[(int)Difs.HP]);
    }
    public float EnemyDamageRampUp(float baseValue)
    {
        return baseValue * Mathf.Pow(EnemyDmgRamp + (difficultyBoost / 100), difficulties[(int)Difs.DMG]);
    }
}
