using System.Collections.Generic;
using UnityEngine;

public class TriggerUI : MonoBehaviour
{
    [SerializeField] GameObject UI;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            EventBus.Publish<TriggerUIEvent>(new TriggerUIEvent(UI, true));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            EventBus.Publish<TriggerUIEvent>(new TriggerUIEvent(UI, false));
        }
    }
}

public class TriggerUIEvent
{
    public Dictionary<GameObject, bool> UIStates = new Dictionary<GameObject, bool>();
    public TriggerUIEvent(GameObject _UI, bool state)
    {
        UIStates[_UI] = state;
    }
}
