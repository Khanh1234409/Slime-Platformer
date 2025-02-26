using System.Drawing;
using UnityEngine;

public class SlimeGauge : MonoBehaviour
{
    [SerializeField] float minSize = 1;
    [SerializeField] float maxSize = 4;
    Subscription<CollectSlimeEvent> collectSlimeEvent;
    Subscription<RespawnEvent> respawnEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collectSlimeEvent = EventBus.Subscribe<CollectSlimeEvent>(_IncreaseSize);
        respawnEvent = EventBus.Subscribe<RespawnEvent>(_ResetGuage);
    }

    void ChangeSize(float size)
    {
        transform.localScale = new Vector3(size, size, 0);
    }

    void _IncreaseSize(CollectSlimeEvent e)
    {
        if(e.obj == gameObject)
        {
            float newSize = Mathf.Min(maxSize, transform.localScale.x + e.amount, minSize + e.amount);
            ChangeSize(newSize);
        }
    }

    void _ResetGuage(RespawnEvent e)
    {
        ChangeSize(minSize);
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<CollectSlimeEvent>(collectSlimeEvent);
        EventBus.Unsubscribe<RespawnEvent>(respawnEvent);
    }
}
