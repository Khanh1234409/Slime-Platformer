using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] List<GameObject> pressurePlates = new List<GameObject>();
    [SerializeField] bool isToggle = false;
    public bool openDoor = false;

    Subscription<PressurePlateEvent> pressurePlateEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pressurePlateEvent = EventBus.Subscribe<PressurePlateEvent>(_CheckDoor);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<BoxCollider2D>().enabled = !openDoor;
        GetComponent<SpriteRenderer>().enabled = !openDoor;
    }

    void _CheckDoor(PressurePlateEvent e)
    {
        foreach(GameObject pressurePlate in pressurePlates)
        {
            if(!PressurePlateEvent.pressurePlateStates.ContainsKey(pressurePlate) || PressurePlateEvent.pressurePlateStates[pressurePlate] == false)
            {
                if(!isToggle)
                {
                    openDoor = false;
                }
                return;
            }
        }
        openDoor = true;
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<PressurePlateEvent>(pressurePlateEvent);
    }
}
