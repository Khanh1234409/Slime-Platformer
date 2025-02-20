using System.ComponentModel.Design;
using System.Security.Cryptography;
using UnityEngine;

public class Flag : MonoBehaviour
{
    Subscription<CheckpointEvent> checkpointEvent;
    void Start()
    {
        checkpointEvent = EventBus.Subscribe<CheckpointEvent>(_ChangeColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void _ChangeColor(CheckpointEvent e)
    {
        GetComponent<SpriteRenderer>().color = new Color(255, 73, 73);
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<CheckpointEvent>(checkpointEvent);
    }
}
