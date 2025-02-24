using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    Vector2 lastCheckpointPosition;
    Vector2 playerInitialPosition;
    bool hasCheckpoint = false;
    GameObject player;

    [SerializeField] GameObject menuOverlay;
    bool menuIsActive = false;

    Subscription<CheckpointEvent> checkpointEvent;
    Subscription<GoalEvent> goalEvent;
    Subscription<RespawnEvent> respawnEvent;

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
        playerInitialPosition = player.transform.position;
        menuOverlay.SetActive(menuIsActive);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            EventBus.Publish<RespawnEvent>(new RespawnEvent());
        }
        if(Input.GetKeyDown(KeyCode.Escape) && menuOverlay)
        {
            menuOverlay.SetActive(menuIsActive);
            menuIsActive = !menuIsActive;
        }
    }

    void _SetPlayerCheckpoint(CheckpointEvent e)
    {
        lastCheckpointPosition = e.checkpoint.transform.position;
        hasCheckpoint = true;
    }

    void _ResetPlayerCheckpoint(GoalEvent e)
    {
        lastCheckpointPosition = playerInitialPosition;
        hasCheckpoint = false;
    }

    void _Respawn(RespawnEvent e)
    {
        Respawn();
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
    }
}

public class RespawnEvent
{
    public RespawnEvent(){}
}
