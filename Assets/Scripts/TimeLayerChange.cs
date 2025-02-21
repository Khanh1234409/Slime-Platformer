using UnityEngine;

public class TimeLayerChange : MonoBehaviour
{
    [SerializeField] float timeToChangeLayer = 0.1f;
    [SerializeField] LayerMask layerToChangeInto;
    [SerializeField] float minSize = 2;
    // float nextLayerChangeTime;
    Subscription<ThrowEvent> throwEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        throwEvent = EventBus.Subscribe<ThrowEvent>(_ChangeSlimeLayer);
        // nextLayerChangeTime = Time.time;
    }

    void _ChangeSlimeLayer(ThrowEvent e)
    {
        if(e.obj == gameObject && transform.localScale.x >= minSize)
        {
            // SetLayerChangeTime();
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
