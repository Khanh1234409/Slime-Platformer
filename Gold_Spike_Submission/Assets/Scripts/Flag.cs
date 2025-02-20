using System.ComponentModel.Design;
using System.Security.Cryptography;
using UnityEngine;

public class Flag : MonoBehaviour
{
    Subscription<CheckpointEvent> checkpointEvent;
    void Start()
    {
        checkpointEvent = EventBus.Subscribe<CheckpointEvent>(_ChangeVisibility);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void _ChangeVisibility(CheckpointEvent e)
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
