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
            // Debug.Log(e.pressurePlateStates[pressurePlate]);
            if(!e.pressurePlateStates.ContainsKey(pressurePlate) || e.pressurePlateStates[pressurePlate] == false)
            {
                Debug.Log("false");
                if(!isToggle)
                {
                    openDoor = false;
                }
                return;
            }
        }
        Debug.Log("true");
        openDoor = true;
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<PressurePlateEvent>(pressurePlateEvent);
    }
}
