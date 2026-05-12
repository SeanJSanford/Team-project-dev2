//using UnityEngine;

//public class OldDevMenu : MonoBehaviour
//{
//    [Header("References")]
//    [SerializeField] private PlayerStats playerStats;
//    [SerializeField] private EnemySpawner enemySpawner;

//    [Header("Debug Settings")]
//    [SerializeField] private bool devMenuOpen = false;
//    [SerializeField] private bool forceLootDrop = false;

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.F1))
//        {
//            devMenuOpen = !devMenuOpen;
//        }

//        if (!devMenuOpen)
//            return;

//        if (Input.GetKeyDown(KeyCode.Alpha1))
//        {
//            enemySpawner.SpawnEnemy();
//        }

//        if (Input.GetKeyDown(KeyCode.Alpha2))
//        {
//            playerStats.Heal(25);
//        }

//        if (Input.GetKeyDown(KeyCode.Alpha3))
//        {
//            playerStats.TakeDamage(25);
//        }

//        if (Input.GetKeyDown(KeyCode.Alpha4))
//        {
//            playerStats.AddDamage(5);
//        }

//        if (Input.GetKeyDown(KeyCode.Alpha5))
//        {
//            playerStats.AddFireRateModifier(0.25f);
//        }

//        if (Input.GetKeyDown(KeyCode.Alpha6))
//        {
//            forceLootDrop = !forceLootDrop;
//            LootDrop.ForceLootDrop = forceLootDrop;
//            Debug.Log("Force Loot Drop: " + forceLootDrop);
//        }
//    }

//    private void OnGUI()
//    {
//        if (!devMenuOpen)
//            return;

//        GUI.Box(new Rect(10, 10, 320, 220), "DEV MENU");

//        GUI.Label(new Rect(25, 40, 280, 25), "F1 - Toggle Dev Menu");
//        GUI.Label(new Rect(25, 65, 280, 25), "1 - Spawn Enemy");
//        GUI.Label(new Rect(25, 90, 280, 25), "2 - Heal Player +25");
//        GUI.Label(new Rect(25, 115, 280, 25), "3 - Damage Player -25");
//        GUI.Label(new Rect(25, 140, 280, 25), "4 - Add Damage +5");
//        GUI.Label(new Rect(25, 165, 280, 25), "5 - Add Fire Rate Modifier +0.25");
//        GUI.Label(new Rect(25, 190, 280, 25), "6 - Toggle 100% Loot Drop: " + forceLootDrop);
//    }
//}