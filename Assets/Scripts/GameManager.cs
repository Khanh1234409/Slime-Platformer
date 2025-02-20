using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    Vector2 lastCheckpointPosition;
    Vector2 playerInitialPosition;
    bool hasCheckpoint = false;

    Subscription<CheckpointEvent> checkpointEvent;
    Subscription<GoalEvent> goalEvent;

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
        checkpointEvent = EventBus.Subscribe<CheckpointEvent>(_SetPlayerCheckpoint);
        goalEvent = EventBus.Subscribe<GoalEvent>(_ResetPlayerCheckpoint);
        playerInitialPosition = GameObject.Find("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public Vector2 GetCheckpointPosition()
    {
        return hasCheckpoint ? lastCheckpointPosition : playerInitialPosition;
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<CheckpointEvent>(checkpointEvent);
        EventBus.Unsubscribe<GoalEvent>(goalEvent);
    }
}
