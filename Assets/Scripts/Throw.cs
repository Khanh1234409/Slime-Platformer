using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField] const float MAXPLAYERSIZE = 4;
    [SerializeField] const float MINPLAYERSIZE = 1;
    [SerializeField] GameObject throwableSlime;
    [SerializeField] float throwingPower = 10;
    [SerializeField] float scaleChangeAmount = 0.2f;
    [SerializeField] float chargeInterval = 0.2f;
    [SerializeField] bool aimAssistOn = true;

    float nextChargeTime;
    GameObject slimeInstance = null;
    float throwableSlimePosY;
    bool isGrounded = false;
    Subscription<IsGroundedEvent> isGroundedEvent;
    private float[] presetAngles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGroundedEvent = EventBus.Subscribe<IsGroundedEvent>(_SetGrounded);
        nextChargeTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // getting the mouse position
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.z = 0; // Keep on the same Z plane

        // spawn throwable slime
        // if(Input.GetMouseButtonDown(0) && !slimeInstance && transform.localScale.x > MINPLAYERSIZE)
        // {
        //     nextChargeTime = Time.time;
        //     Vector2 spawnPos = transform.position + (transform.localScale / 2) + (throwableSlime.transform.localScale / 2);
        //     throwableSlimePosY = transform.localScale.y / 2;
        //     slimeInstance = Instantiate(throwableSlime, spawnPos, quaternion.identity);
        //     Debug.Log("Throwing " + throwableSlime);
        // }
        // throw slime at where the cursor is
        Vector2 direction = (cursorPos - transform.position).normalized;
        if(aimAssistOn)
        {
            direction = AimAssistSnap().normalized;
        }
        
        if(Input.GetMouseButtonUp(0) && slimeInstance)
        {
            slimeInstance.GetComponent<Rigidbody2D>().linearVelocity = direction * throwingPower;
            Vector2 velocity = slimeInstance.GetComponent<Rigidbody2D>().linearVelocity;
            EventBus.Publish<ThrowEvent>(new ThrowEvent(slimeInstance, velocity));
            slimeInstance = null;
        }

        // charge slime throw
        if(Input.GetMouseButton(0))
        {
            EventBus.Publish<MouseDirectionEvent>(new MouseDirectionEvent(direction));
            if(!slimeInstance && transform.localScale.x > MINPLAYERSIZE)
            {
                nextChargeTime = Time.time;
                Vector2 spawnPos = transform.position + (transform.localScale / 2) + (throwableSlime.transform.localScale / 2);
                throwableSlimePosY = transform.localScale.y / 2;
                slimeInstance = Instantiate(throwableSlime, spawnPos, quaternion.identity);
            }

            if(slimeInstance)
            {
                slimeInstance.transform.position = new Vector2(transform.position.x, transform.position.y + throwableSlimePosY);
                // if the player is still larger than the min size, charge up throw
                if(MINPLAYERSIZE < transform.localScale.x && MAXPLAYERSIZE >= slimeInstance.transform.localScale.x && Time.time >= nextChargeTime)
                {
                    nextChargeTime = Time.time + chargeInterval;

                    // change player size
                    float scaleChangeX = Mathf.Max(MINPLAYERSIZE, transform.localScale.x - scaleChangeAmount);
                    transform.localScale = new Vector2(scaleChangeX, scaleChangeX);

                    // update player position if grounded
                    if(isGrounded)
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y - scaleChangeAmount / 2);
                    }
                    
                    // change throwable slime size
                    float instanceChangeX = slimeInstance.transform.localScale.x + scaleChangeAmount;
                    slimeInstance.transform.localScale = new Vector2(instanceChangeX, instanceChangeX);
                }
            }
        }
    }

    Vector2 AimAssistSnap()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDirection = (cursorPos - (Vector2)transform.position).normalized;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        // Find the closest preset angle
        float closestAngle = presetAngles[0];
        float minDifference = Mathf.Abs(Mathf.DeltaAngle(aimAngle, closestAngle));

        foreach (float angle in presetAngles)
        {
            float angleDiff = Mathf.Abs(Mathf.DeltaAngle(aimAngle, angle));
            if (angleDiff < minDifference)
            {
                closestAngle = angle;
                minDifference = angleDiff;
            }
        }

        // Convert angle to direction and shoot
        return new Vector2(Mathf.Cos(closestAngle * Mathf.Deg2Rad), Mathf.Sin(closestAngle * Mathf.Deg2Rad)).normalized;
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<IsGroundedEvent>(isGroundedEvent);
    }

    void _SetGrounded(IsGroundedEvent e)
    {
        isGrounded = e.isGrounded;
    }
}

public class ThrowEvent
{
    public GameObject obj;
    public Vector2 velocity;
    public ThrowEvent(GameObject _obj, Vector2 _velocity)
    {
        obj = _obj;
        velocity = _velocity;
    }
}

public class MouseDirectionEvent
{
    public Vector2 direction;
    public MouseDirectionEvent(Vector2 _direction)
    {
        direction = _direction;
    }
}