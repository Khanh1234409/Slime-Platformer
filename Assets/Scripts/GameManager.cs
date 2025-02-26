using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    Vector2 lastCheckpointPosition;
    Vector2 playerInitialPosition;
    bool hasCheckpoint = false;
    GameObject player;

    // [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject completionScreen;
    Vector3 playerPausePosition;
    bool menuIsActive = false;

    Subscription<CheckpointEvent> checkpointEvent;
    Subscription<GoalEvent> goalEvent;
    Subscription<RespawnEvent> respawnEvent;
    Subscription<ResetCheckpointEvent> resetPlayerCheckpoint;
    Subscription<TogglePauseMenuEvent> togglePauseMenu;
    Subscription<PlayerInitialPositionEvent> playerInitialPositionEvent;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player = GameObject.Find("Player");
        checkpointEvent = EventBus.Subscribe<CheckpointEvent>(_SetPlayerCheckpoint);
        goalEvent = EventBus.Subscribe<GoalEvent>(_ResetPlayerCheckpoint);
        respawnEvent = EventBus.Subscribe<RespawnEvent>(_Respawn);
        resetPlayerCheckpoint = EventBus.Subscribe<ResetCheckpointEvent>(_ResetPlayerCheckpoint);
        togglePauseMenu = EventBus.Subscribe<TogglePauseMenuEvent>(_TogglePauseMenu);
        playerInitialPositionEvent = EventBus.Subscribe<PlayerInitialPositionEvent>(_SetPlayerInitialPosition);
        playerInitialPosition = player.transform.position;
        lastCheckpointPosition = playerInitialPosition;
        // pauseMenu.SetActive(menuIsActive);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(menuIsActive);
        GameObject canvas = GameObject.Find("Canvas");
        GameObject pauseMenu = canvas.transform.Find("Pause Menu")?.gameObject;
        player = GameObject.Find("Player");
        if(Input.GetKeyDown(KeyCode.R))
        {
            EventBus.Publish<RespawnEvent>(new RespawnEvent());
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            playerPausePosition = player.transform.position;
            EventBus.Publish<TogglePauseMenuEvent>(new TogglePauseMenuEvent());
        }
        if(menuIsActive && player)
        {
            player.transform.position = playerPausePosition;
        }
        if(pauseMenu)
        {
            pauseMenu.SetActive(menuIsActive);
        }
    }

    void _SetPlayerCheckpoint(CheckpointEvent e)
    {
        lastCheckpointPosition = e.checkpoint.transform.position;
        hasCheckpoint = true;
    }

    void _SetPlayerInitialPosition(PlayerInitialPositionEvent e)
    {
        playerInitialPosition = e.position;
    }

    void _ResetPlayerCheckpoint(GoalEvent e)
    {
        lastCheckpointPosition = playerInitialPosition;
        hasCheckpoint = false;
    }

    void _ResetPlayerCheckpoint(ResetCheckpointEvent e)
    {
        Debug.Log("Resetting Player Checkpoints");
        lastCheckpointPosition = playerInitialPosition;
        hasCheckpoint = false;
    }

    void _Respawn(RespawnEvent e)
    {
        Debug.Log("Respawning");
        Respawn();
    }

    void _TogglePauseMenu(TogglePauseMenuEvent e)
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject pauseMenu = canvas.transform.Find("Pause Menu")?.gameObject;
        if(pauseMenu)
        {
            Debug.Log("Pause menu found");
            menuIsActive = TogglePauseMenuEvent.isEnabled;
            EventBus.Publish<MovementEvent>(new MovementEvent(!menuIsActive));
        }
    }

    void Respawn()
    {
        player.transform.position = lastCheckpointPosition;
        hasCheckpoint = false;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        menuIsActive = false;
    }

    public Vector2 GetCheckpointPosition()
    {
        return hasCheckpoint ? lastCheckpointPosition : playerInitialPosition;
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<CheckpointEvent>(checkpointEvent);
        EventBus.Unsubscribe<GoalEvent>(goalEvent);
        EventBus.Unsubscribe<RespawnEvent>(respawnEvent);
        EventBus.Unsubscribe<ResetCheckpointEvent>(resetPlayerCheckpoint);
        EventBus.Unsubscribe<TogglePauseMenuEvent>(togglePauseMenu);
        EventBus.Unsubscribe<PlayerInitialPositionEvent>(playerInitialPositionEvent);
    }
}

public class RespawnEvent
{
    public RespawnEvent(){}
}

public class ResetCheckpointEvent
{
    public ResetCheckpointEvent(){}
}

public class TogglePauseMenuEvent
{
    public static bool isEnabled = false;
    public TogglePauseMenuEvent()
    {
        isEnabled = !isEnabled;
    }
    public TogglePauseMenuEvent(bool _isEnabled)
    {
        isEnabled = _isEnabled;
    }
}