using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PressurePlate : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        EventBus.Publish<PressurePlateEvent>(new PressurePlateEvent(gameObject, true));
    }

    void OnTriggerExit2D(Collider2D other)
    {
        EventBus.Publish<PressurePlateEvent>(new PressurePlateEvent(gameObject, false));
    }
}

public class PressurePlateEvent
{
    public static Dictionary<GameObject, bool> pressurePlateStates = new Dictionary<GameObject, bool>();
    public PressurePlateEvent(GameObject pressurePlate, bool state)
    {
        pressurePlateStates[pressurePlate] = state;
    }
}
