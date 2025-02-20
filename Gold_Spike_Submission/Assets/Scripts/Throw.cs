using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField] const float MAXPLAYERSIZE = 3;
    [SerializeField] const float MINPLAYERSIZE = 1;
    [SerializeField] GameObject throwableSlime;
    [SerializeField] float throwingPower = 10;
    [SerializeField] float scaleChangeAmount = 0.2f;
    [SerializeField] float chargeInterval = 0.25f;
    float nextChargeTime;
    GameObject slimeInstance = null;
    float throwableSlimePosY;
    bool isGrounded = false;
    Subscription<IsGroundedEvent> isGroundedEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGroundedEvent = EventBus.Subscribe<IsGroundedEvent>(_SetGrounded);
    }

    // Update is called once per frame
    void Update()
    {
        // getting the mouse position
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.z = 0; // Keep on the same Z plane
        
        // Debug.Log(Input.GetMouseButtonDown(0));
        // Debug.Log(slimeInstance);
        // Debug.Log("Local scale = " + transform.localScale.x);
        // Debug.Log("MINPLAYERSIZE = " + MINPLAYERSIZE);
        // spawn throwable slime
        if(Input.GetMouseButtonDown(0) && !slimeInstance && transform.localScale.x > MINPLAYERSIZE)
        {
            nextChargeTime = Time.time;
            Vector2 spawnPos = transform.position + (transform.localScale / 2) + (throwableSlime.transform.localScale / 2);
            throwableSlimePosY = transform.localScale.y / 2;
            slimeInstance = Instantiate(throwableSlime, spawnPos, quaternion.identity);
            Debug.Log("Throwing " + throwableSlime);
        }
        // throw slime at where the cursor is
        else if(Input.GetMouseButtonUp(0) && slimeInstance)
        {
            Vector2 direction = (cursorPos - transform.position).normalized;
            slimeInstance.GetComponent<Rigidbody2D>().linearVelocity = direction * throwingPower;
            Vector2 velocity = slimeInstance.GetComponent<Rigidbody2D>().linearVelocity;
            EventBus.Publish<ThrowEvent>(new ThrowEvent(slimeInstance, velocity));
            slimeInstance = null;
        }

        // charge slime throw
        if(Input.GetMouseButton(0) && slimeInstance)
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
    public GameObject thrown;
    public Vector2 velocity;
    public ThrowEvent(GameObject _thrown, Vector2 _velocity)
    {
        thrown = _thrown;
        velocity = _velocity;
    }
}