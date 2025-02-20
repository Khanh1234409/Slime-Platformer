using TMPro;
using UnityEngine;

public class UIMessage : MonoBehaviour
{
    Subscription<TriggerUIEvent> triggerUIEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        triggerUIEvent = EventBus.Subscribe<TriggerUIEvent>(_DisplayMessage);
    }

    void _DisplayMessage(TriggerUIEvent e)
    {
        if (e.UIStates.ContainsKey(gameObject))
        {
            GetComponent<TextMeshProUGUI>().enabled = e.UIStates[gameObject];
        }
        else
        {
            GetComponent<TextMeshProUGUI>().enabled = false;
        }
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<TriggerUIEvent>(triggerUIEvent);
    }
}
