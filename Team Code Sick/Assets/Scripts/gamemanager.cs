using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.SpeedTree.Importer;
using Unity.VisualScripting;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject inRoomHUD;
    [SerializeField] GameObject safeRoomRequirements;
    [SerializeField] GameObject floorCleared;
    [SerializeField] GameObject safeRoomIndication;
    [SerializeField] GameObject safeRoomInstructions;
    [SerializeField] GameObject roomClearedText;
  

    public Material[] elementMaterials;

    public TMP_Text enemyCount;
    public TMP_Text waveCount;
    public TMP_Text roomsLeft;
    public TMP_Text currentFloorText;
    public TMP_Text difficultyText;
    public TMP_Text bossAmount;


    public GameObject playerDamageScreen;
    public Image playerHPBar;
    public GameObject BossHP;
    public Image BossHPBar;

    [Header("Player Cooldown UI")]
    public Image playerDashCooldownBar;
    public Image playerStaminaBar;
    public Image playerSkillCooldownBar;

    public TMP_Text dashCooldownText;
    public TMP_Text staminaText;
    public TMP_Text skillCooldownText;

    [Header("Extraction")]
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private string hubSceneName = "hub";
    [SerializeField] private InventoryUI inventoryUI;
    public bool isExtracting;
    
    private BossCutscene bossCutscene;

    public int seed;
    public int worldSize;

    public bool isPaused;
    public bool playerInRoom = false;
    public bool roomStarted = false;
    public bool playerInSafeRoom = false;
    public int currentRoom = -1;
    public (int x, int y) playerGridPosition;
    public GameObject player;
    public playerMovement playerScript;
    public int unitSize = 10; // The size for each unit such as wall, tunnels, etc.

    [Header("Wave and Floors")]

    public int waves;
    public int currentWave;
    public bool waveCleared;
    public int startingAmountOfEnemies;
    public int enemyInRoom;
    public int remainingBoses;
    public int maxBoses;
    public bool roomCleared;
    public int floorsTillBoss;
    public int maxFloorsTillBoss;
    public bool bossCleared;

    [Header("Level Creation")]

    public List<(int x, int y)> directions = new List<(int x, int y)> { (0, -1), (0, 1), (-1, 0), (1, 0) };
    public List<List<LevelCreation>> worldGrid = new List<List<LevelCreation>>();
    public List<int> finishedRooms; // This will hold the index of the rooms from allCenters
    public List<GameObject> allDoors = new List<GameObject>();

    [Header("Text")]

    int remainingRooms;
    int gameGoalCount;
    int currentFloor = 1;

    bool floorFinished = false;

    public float timeScaleOrig;

    [Header("Difficulty Options")]

    public int startDifficulty;
    public int difficultyRampUp;
    public int amountOfRooms;
    public int bossFrequncy;
    public int extractionAmount;
    public int luckAmount;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (seed != -1)
            UnityEngine.Random.InitState(seed);

        instance = this;
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerMovement>();
        remainingBoses = maxBoses;

        if (playerInventory == null)
        {
            playerInventory = GetComponent<Inventory>();
        }
    }

    IEnumerator Start()
    {
        worldGrid.Clear();

        DifficultyOptions.instance.LoadSettings(instance);
        for (int i = 1; i < startDifficulty; i++)
        {
            EnemySpawnsCenter.instance.roomDifficulty++;
            DifficultyRampUp.instance.Dif();
        }
        DifficultyRampUp.instance.difficultyBoost = difficultyRampUp;
        LevelCreation.instance.amountOfRooms = amountOfRooms;
        maxFloorsTillBoss = bossFrequncy;
        maxBoses = extractionAmount;

        LevelCreation.instance.size = 10 * amountOfRooms <= 20 ? 20 : 20 + 2 * amountOfRooms;
        remainingBoses = maxBoses;

        DifficultyOptions.instance.CalculateBoost(true, playerScript);

        for (int y = 0; y < worldSize; y++)
        {
            List<LevelCreation> row = new List<LevelCreation>();

            for (int x = 0; x < worldSize; x++)
            {
                row.Add(null);
            }

            worldGrid.Add(row);
        }

        playerScript.playerWorldPosition = (2, 2);

        yield return null;

        LevelCreation.instance.StartGrid();

        yield return null;

        if (LevelCreation.instance.allCenters == null || LevelCreation.instance.allCenters.Count == 0)
        {
            yield break;
        }

        Vector3 spawnPos = new Vector3(
            LevelCreation.instance.allCenters[0].x * unitSize,
            1,
            LevelCreation.instance.allCenters[0].y * unitSize
        );

        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        player.transform.position = spawnPos;

        if (controller != null)
            controller.enabled = true;

        difficultyText.text = EnemySpawnsCenter.instance.roomDifficulty.ToString("f0");
        roomsLeft.text = remainingRooms.ToString("f0");
        currentFloorText.text = currentFloor.ToString("f0");
        bossAmount.text = $"Defeat {remainingBoses} more Bosses to Extract.";
        floorsTillBoss = maxFloorsTillBoss;
    }

    // Update is called once per frame
    void Update()
    {
        playerGridPosition = ((int)player.transform.position.x / unitSize, (int)player.transform.position.z / unitSize);
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }
    }

    private void LateUpdate()
    {
        if (Input.GetButtonDown("Continue"))
        {
            if (inventoryUI != null && inventoryUI.IsInventoryOpen)
                return;


            if (floorFinished && playerInSafeRoom)
                StartNewFloor();
        }

        if (remainingBoses == 0 && Input.GetButtonDown("Extraction"))
        {
            Extract();
        }
    }
    private void Extract()
    {
        if (isExtracting)
            return;

        isExtracting = true;

        if (playerInventory == null)
        {
            isExtracting = false;
            return;
        }

        playerInventory.SaveInventory();

        PlayerPrefs.Save();

        Time.timeScale = timeScaleOrig;

        SceneManager.LoadScene("hub");
    }
    public void statePause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void stateUnpause()

    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateRemainingRooms(int amount)
    {
        remainingRooms += amount;
        roomsLeft.text = remainingRooms.ToString("f0");
        if (remainingRooms <= 0)
        {
            floorFinished = true;
        }
    }
    public void updateGameGoal(int amount)
    {
        gameGoalCount += amount;
        roomsLeft.text = gameGoalCount.ToString("f0");

        if (gameGoalCount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }

    public void updateEnemyCount(int amount)
    {
        enemyInRoom += amount;

        if (enemyInRoom <= 0)
        {
            waveCleared = true;
        }
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
    public void youWin()
    {
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }

    public void EnterRoom()
    {
        inRoomHUD.SetActive(true);
    }

    public void ExitRoom()
    {
        inRoomHUD.SetActive(false);
    }

    public void FinishedRoomOn()
    {
        if (!floorFinished)
            roomClearedText.SetActive(true);
        else
            floorCleared.SetActive(true);
    }

    public void FinishedRoomOff()
    {
        if (!floorFinished)
            roomClearedText.SetActive(false);
        else
            floorCleared.SetActive(true);
    }

    public void InSafeRoom()
    {
        safeRoomRequirements.SetActive(true);
        safeRoomIndication.SetActive(true);
        if (floorFinished)
        {
            floorCleared.SetActive(false);
            safeRoomInstructions.SetActive(true);
        }
    }

    public void OutSafeRoom()
    {
        safeRoomRequirements.SetActive(false);
        safeRoomIndication.SetActive(false);
        safeRoomInstructions.SetActive(false);
    }
    public void StartNewFloor()
    {
        currentFloor++;
        floorsTillBoss--;
        safeRoomInstructions.SetActive(false);
        Physics.SyncTransforms();
        player.transform.position = new Vector3(0, 0, 0);
        LevelCreation.instance.ClearGrid();
        LevelCreation.instance.StartGrid();
        Vector3 spawnPos = new Vector3(LevelCreation.instance.allCenters[0].x * unitSize, 1, LevelCreation.instance.allCenters[0].y * unitSize);
        player.transform.position = spawnPos;
        roomsLeft.text = remainingRooms.ToString("f0");
        currentFloorText.text = currentFloor.ToString("f0");
        bossCleared = false;
        floorFinished = false;
        finishedRooms = new List<int>();
    }

    public void PlayBossCutscene(GameObject boss)
    {
        if (bossCutscene == null || boss == null)
            return;

        StartCoroutine(
            bossCutscene.PlayBossIntro(
                boss.transform
            )
        );
    }

    public void StartRoutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    public void UpdateDashCooldownUI(float currentTimer, float maxCooldown)
    {
        if (playerDashCooldownBar != null)
        {
            if (maxCooldown <= 0)
            {
                playerDashCooldownBar.fillAmount = 1f;
            }
            else
            {
                playerDashCooldownBar.fillAmount = 1f - Mathf.Clamp01(currentTimer / maxCooldown);
            }
        }

        if (dashCooldownText != null)
        {
            if (currentTimer > 0)
                dashCooldownText.text = currentTimer.ToString("F1");
            else
                dashCooldownText.text = "Dash Ready";
        }
    }

    public void UpdateStaminaUI(float currentStamina, float maxStamina)
    {
        if (playerStaminaBar != null)
        {
            playerStaminaBar.fillAmount = currentStamina / maxStamina;
        }

        if (staminaText != null)
        {
            staminaText.text = currentStamina.ToString("F0") + " / " + maxStamina.ToString("F0");
        }
    }

    public void UpdateSkillCooldownUI(float currentTimer, float maxCooldown)
    {
        if (playerSkillCooldownBar != null)
        {
            if (maxCooldown <= 0)
            {
                playerSkillCooldownBar.fillAmount = 1f;
            }
            else
            {
                playerSkillCooldownBar.fillAmount = 1f - Mathf.Clamp01(currentTimer / maxCooldown);
            }
        }

        if (skillCooldownText != null)
        {
            if (currentTimer > 0)
                skillCooldownText.text = currentTimer.ToString("F1");
            else
                skillCooldownText.text = "Skill Ready";
        }
    }
}
