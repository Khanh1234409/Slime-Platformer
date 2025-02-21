using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    Subscription<ThrowEvent> throwEvent;
    Rigidbody2D rb;
    [SerializeField] float recoilTime = 2;
    float recoilEndTime;
    bool inRecoil = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        throwEvent = EventBus.Subscribe<ThrowEvent>(_Recoil);
        rb = GetComponent<Rigidbody2D>();
        recoilEndTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(inRecoil && Time.time > recoilEndTime)
        {
            EventBus.Publish<MovementEvent>(new MovementEvent(true));
        }
    }
    
    void _Recoil(ThrowEvent e)
    {
        EventBus.Publish<MovementEvent>(new MovementEvent(false));
        float extraSpeed = e.obj.transform.localScale.x > 1 ? 1 + e.obj.transform.localScale.x * .25f : 1;
        rb.linearVelocity = -e.velocity * extraSpeed;
        Debug.Log(-e.velocity * (e.obj.transform.localScale.x));

        recoilEndTime = Time.time + recoilTime;
        inRecoil = true;
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<ThrowEvent>(throwEvent);
    }
}
