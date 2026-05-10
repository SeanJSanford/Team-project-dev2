using UnityEngine;

/*
 * GameManager
 * 
 * Purpose:
 * Handles high-level game state for the current scene.
 * 
 * Current Responsibilities:
 * - Tracks pause/unpause state
 * - Shows pause, win, and lose menus
 * - Stores a reference to the player
 * - Tracks a simple game goal counter
 * 
 * Connected Systems:
 * - PlayerMovement
 * - Pause menu UI
 * - Win/Lose UI
 * - Future RunManager or SceneLoader systems
 * 
 * Design Notes:
 * This script should only manage broad game state.
 * 
 * It should NOT:
 * - Handle player stats
 * - Handle combat logic
 * - Handle inventory logic
 * - Spawn enemies directly
 * 
 * Future Expansion Ideas:
 * - Move run-specific logic into RunManager
 * - Move scene loading into SceneLoader
 * - Add restart/quit button methods
 * - Add event-based win/loss triggers
 */

public class GameManager : MonoBehaviour
{
    // Simple global access point for scene-level game state.
    public static GameManager Instance;

    [Header("Menus")]
    [SerializeField] private GameObject menuActive;
    [SerializeField] private GameObject menuPause;
    [SerializeField] private GameObject menuWin;
    [SerializeField] private GameObject menuLose;

    [Header("Player References")]
    public GameObject player;
    public PlayerMovement playerScript;

    [Header("Game State")]
    public bool isPaused;

    // Tracks remaining goals/objectives/enemies.
    private int gameGoalCount;

    // Stores the original time scale so unpause can restore it.
    private float timeScaleOrig;

    private void Awake()
    {
        // Singleton setup.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        timeScaleOrig = Time.timeScale;

        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            playerScript = player.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogWarning("GameManager could not find a GameObject tagged Player.");
        }
    }

    private void Update()
    {
        // Escape / Cancel toggles pause menu.
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                PauseGame(menuPause);
            }
            else if (menuActive == menuPause)
            {
                UnpauseGame();
            }
        }
    }

    /*
     * PauseGame
     * 
     * Pauses gameplay and optionally opens a menu.
     */
    public void PauseGame(GameObject menuToOpen = null)
    {
        isPaused = true;
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        menuActive = menuToOpen;

        if (menuActive != null)
        {
            menuActive.SetActive(true);
        }
    }

    /*
     * UnpauseGame
     * 
     * Restores gameplay from pause state.
     */
    public void UnpauseGame()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }
    }

    /*
     * UpdateGameGoal
     * 
     * Adjusts the current game goal counter.
     * 
     * Example:
     * - Add enemies remaining at start
     * - Subtract when enemies are defeated
     * - Trigger win when count reaches 0
     */
    public void UpdateGameGoal(int amount)
    {
        gameGoalCount += amount;

        if (gameGoalCount <= 0)
        {
            WinGame();
        }
    }

    /*
     * WinGame
     * 
     * Opens the win menu and pauses gameplay.
     */
    public void WinGame()
    {
        PauseGame(menuWin);
    }

    /*
     * LoseGame
     * 
     * Opens the lose menu and pauses gameplay.
     */
    public void LoseGame()
    {
        PauseGame(menuLose);
    }
}