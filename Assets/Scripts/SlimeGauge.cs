using System.Drawing;
using UnityEngine;

public class SlimeGauge : MonoBehaviour
{
    [SerializeField] const float MINPLAYERSIZE = 1;
    [SerializeField] const float MAXPLAYERSIZE = 4;
    Subscription<CollectSlimeEvent> collectSlimeEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collectSlimeEvent = EventBus.Subscribe<CollectSlimeEvent>(_IncreasePlayerSize);
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

    void OnDestroy()
    {
        EventBus.Unsubscribe<CollectSlimeEvent>(collectSlimeEvent);
    }
}
