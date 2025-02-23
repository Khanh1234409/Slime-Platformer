using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowGuide : MonoBehaviour
{
    // public Transform player; // Assign the player GameObject here
    private Camera mainCamera;
    Vector2 mouseDirection;
    SpriteRenderer arrowSprite;
    Subscription<MouseDirectionEvent> mouseDirectionEvent;

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera reference
        mouseDirectionEvent = EventBus.Subscribe<MouseDirectionEvent>(_GetMouseDirection);
        arrowSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.position = GameObject.Find("Player").transform.position;
        if(Input.GetMouseButton(0))
        {
            // Calculate the angle in degrees
            float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;

            // Rotate the arrow
            transform.rotation = Quaternion.Euler(0, 0, angle);
            arrowSprite.enabled = true;
        }
        else
        {
            arrowSprite.enabled = false;
        }
    }

    void _GetMouseDirection(MouseDirectionEvent e)
    {
        mouseDirection = e.direction;
    }
}
