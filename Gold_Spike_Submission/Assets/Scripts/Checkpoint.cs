using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Subscription<CheckpointEvent> checkpointEvent;
    void Start()
    {
        checkpointEvent = EventBus.Subscribe<CheckpointEvent>(_ShowFlag);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            EventBus.Publish<CheckpointEvent>(new CheckpointEvent(gameObject));
        }
    }

    void _ShowFlag(CheckpointEvent e)
    {
        SpriteRenderer flagSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if(e.checkpoint == gameObject)
        {
            flagSprite.enabled = true;
        }
        else
        {
            flagSprite.enabled = false;
        }
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<CheckpointEvent>(checkpointEvent);
    }
}

public class CheckpointEvent
{
    public GameObject checkpoint;
    public CheckpointEvent(GameObject _checkpoint)
    {
        checkpoint = _checkpoint;
    }
}
