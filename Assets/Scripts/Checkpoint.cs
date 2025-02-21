using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Subscription<CheckpointEvent> checkpointEvent;
    SpriteRenderer flagSprite;
    void Start()
    {
        checkpointEvent = EventBus.Subscribe<CheckpointEvent>(_ShowFlag);
        flagSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
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
        if(e.checkpoint == gameObject)
        {
            // red
            Debug.Log("Changing color");
            flagSprite.color = new Color(255f / 255f, 73f / 255f, 73f / 255f);
        }
        else
        {
            // white
            flagSprite.color = Color.white;
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
