using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.TestTools;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    public int seed;
    public int worldSize;

    public bool isPaused;
    public bool playerInRoom = false;
    public bool roomStarted = false;
    public int currentRoom = -1;
    public (int x, int y) playerGridPosition;
    public GameObject player;
    public playerMovement playerScript;
    public int unitSize = 10; // The size for each unit such as wall, tunnels, etc.

    public int waves;
    public int currentWave;
    public bool waveCleared;
    public int startingAmountOfEnemies;
    public int enemyInRoom;

    public List<(int x, int y)> directions = new List<(int x, int y)> { (0, -1), (0, 1), (-1, 0), (1, 0) };
    public List<List<LevelCreation>> worldGrid = new List<List<LevelCreation>>();
    public List<int> finishedRooms; // This will hold the index of the rooms from allCenters
    public List<GameObject> allDoors = new List<GameObject>();

    int gameGoalCount;

    float timeScaleOrig;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (seed != -1)
            UnityEngine.Random.InitState(seed);

        instance = this;
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerMovement>();
        
    }

    void Start()
    {

        for (int y = 0; y < worldSize; y++)
        {
            List<LevelCreation> row = new List<LevelCreation>();
            for (int x = 0; x < worldSize; x++)
            {
                row.Add(null);
            }
            worldGrid.Add(row);
        }

        playerScript.playerWorldPosition = (2, 2);// (UnityEngine.Random.Range(0, worldSize), UnityEngine.Random.Range(0, worldSize));
        LevelCreation.instance.StartGrid();
        player.transform.position = new Vector3(LevelCreation.instance.allCenters[0].x * 10, 1, LevelCreation.instance.allCenters[0].y * 10);
    }

    // Update is called once per frame
    void Update()
    {
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
        Cursor.visible = false;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        gameGoalCount += amount;

        if (gameGoalCount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
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


}
