using UnityEngine;

public class TimeLayerChange : MonoBehaviour
{
    [SerializeField] float timeToChangeLayer = 0.1f;
    [SerializeField] LayerMask layerToChangeInto;
    [SerializeField] float minSize = 2;
    // float nextLayerChangeTime;
    Subscription<ThrowEvent> throwEvent;
    Subscription<CollectSlimeEvent> collectSlimeEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        throwEvent = EventBus.Subscribe<ThrowEvent>(_ChangeSlimeLayer);
        collectSlimeEvent = EventBus.Subscribe<CollectSlimeEvent>(_ChangeSlimeLayer);
        // nextLayerChangeTime = Time.time;
    }

    void _ChangeSlimeLayer(ThrowEvent e)
    {
        if(e.obj == gameObject && transform.localScale.x >= minSize)
        {
            Invoke(nameof(ChangeLayer), timeToChangeLayer);
        }
    }

    void _ChangeSlimeLayer(CollectSlimeEvent e)
    {
        if((e.obj == gameObject && transform.localScale.x >= minSize) || (e.obj == gameObject && e.amount >= minSize - 1))
        {

            Invoke(nameof(ChangeLayer), timeToChangeLayer);
        }
    }

    // void SetLayerChangeTime()
    // {
    //     nextLayerChangeTime = Time.time + timeToChangeLayer;
    // }

    void ChangeLayer()
    {
        gameObject.layer = Mathf.RoundToInt(Mathf.Log(layerToChangeInto.value, 2));
    }

    // Update is called once per frame
    void Update()
    {
        // if(Time.time > nextLayerChangeTime)
        // {
        //     gameObject.layer = layerToChangeInto;
        // }
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<ThrowEvent>(throwEvent);
    }
}
