using UnityEngine;

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

/*
========================================================
Project: Team Code Sick
Script: GameManager.cs

Original Framework Source:
- Full Sail Lecture Prototype Code

Primary Team Integration Lead:
- Nilo

Team Contributions:
- Avery Wilson
    - Integration planning
    - System separation architecture
    - Runtime stat/inventory framework planning
    - Gameplay system documentation

Purpose:
- Controls high-level scene/gameplay state.
- Handles pausing and unpausing gameplay.
- Opens pause, win, and lose menus.
- Maintains lightweight scene-level game flow.

Core Responsibilities:
- Pause state management
- Scene-level gameplay flow
- Win/loss menu handling
- Lightweight global game state
- Player reference management

Connected Team Systems:
- Dai: PlayerMovement integration
- Sean: Combat/gameplay flow interactions
- Heather: Future inventory/menu integration
- Avery: Runtime stat and progression integration
- Nilo: Gameplay direction and scene coordination

Design Philosophy:
GameManager should remain lightweight and only manage
broad gameplay state.

This script intentionally avoids handling:
- Combat calculations
- Inventory systems
- Player stat calculations
- Enemy AI behavior
- Weapon systems
- Loot systems

Those responsibilities remain separated into
their own modular systems.

Why This Exists:
Separating scene-level game flow from gameplay systems:
- Reduces system overlap
- Simplifies debugging
- Prevents "god object" architecture
- Improves long-term scalability

Development Notes:
- Originally derived from Full Sail lecture framework code.
- Refactored and cleaned up for team integration.
- Expanded with clearer separation-of-responsibility structure.
- Intended as a temporary lightweight gameplay manager
  until additional manager systems are implemented.

Current Features:
- Pause/unpause support
- Win/lose menu handling
- Basic scene state management
- Player reference tracking

Future Expansion Ideas:
- RunManager integration
- SceneLoader integration
- Save/load handling
- Event-driven game states
- UI manager separation
- Audio manager hooks
- Multiplayer session management
- Match/game mode states
========================================================
*/