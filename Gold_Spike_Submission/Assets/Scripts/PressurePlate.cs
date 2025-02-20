using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PressurePlate : MonoBehaviour
{
    int numOnPressurePlate = 0;
    void OnTriggerEnter2D(Collider2D other)
    {
        EventBus.Publish<PressurePlateEvent>(new PressurePlateEvent(gameObject, true));
        numOnPressurePlate++;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        numOnPressurePlate--;
        if(numOnPressurePlate == 0)
        {
            EventBus.Publish<PressurePlateEvent>(new PressurePlateEvent(gameObject, false));
        }
    }
}

public class PressurePlateEvent
{
    public Dictionary<GameObject, bool> pressurePlateStates = new Dictionary<GameObject, bool>();
    public PressurePlateEvent(GameObject pressurePlate, bool state)
    {
        Debug.Log("Pressure plate: " + state);
        pressurePlateStates[pressurePlate] = state;
    }
}
