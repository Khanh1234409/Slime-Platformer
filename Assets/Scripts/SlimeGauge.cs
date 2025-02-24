using System.Drawing;
using UnityEngine;

public class SlimeGauge : MonoBehaviour
{
    [SerializeField] const float MINPLAYERSIZE = 1;
    [SerializeField] const float MAXPLAYERSIZE = 4;
    Subscription<CollectSlimeEvent> collectSlimeEvent;
    Subscription<RespawnEvent> respawnEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collectSlimeEvent = EventBus.Subscribe<CollectSlimeEvent>(_IncreasePlayerSize);
        respawnEvent = EventBus.Subscribe<RespawnEvent>(_ResetGuage);
    }

    void ChangePlayerSize(float size)
    {
        transform.localScale = new Vector3(size, size, 0);
    }

    void _IncreasePlayerSize(CollectSlimeEvent e)
    {
        if(e.obj == gameObject)
        {
            float newSize = Mathf.Min(MAXPLAYERSIZE, transform.localScale.x + e.amount, MINPLAYERSIZE + e.amount);
            ChangePlayerSize(newSize);
        }
    }

    void _ResetGuage(RespawnEvent e)
    {
        ChangePlayerSize(MINPLAYERSIZE);
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<CollectSlimeEvent>(collectSlimeEvent);
        EventBus.Unsubscribe<RespawnEvent>(respawnEvent);
    }
}
