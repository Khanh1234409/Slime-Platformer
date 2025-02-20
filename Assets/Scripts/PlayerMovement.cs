using System.Data;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerSpeed;
    [SerializeField] float maxSpeedX;
    [SerializeField] float maxSpeedY;
    [SerializeField] float maxSlideSpeedY;
    [SerializeField] float gravityAcceleration;
    [SerializeField] float playerAcceleration;
    Rigidbody2D rb;

    float jumpEndTime;
    [SerializeField] float playerJumpSpeed;
    [SerializeField] float playerWallJumpSpeedX;
    [SerializeField] float playerWallJumpSpeedY;
    [SerializeField] float jumpDurationTime = 0.5f;
    [SerializeField] float groundDistance = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallJumpLayer;

    [SerializeField] float wallJumpDisableMovementTime = 0.2f;
    float nextMovementTime;

    bool playerDirectionRight = true;
    bool movementEnabled = true;
    Subscription<MovementEvent> movementEvent;
    Subscription<ThrowEvent> throwEvent;
    LastJump lastJump = LastJump.None;
    
    enum LastJump
    {
        Ground,
        LeftWall,
        RightWall,
        Throw,
        None
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementEvent = EventBus.Subscribe<MovementEvent>(_ChangeMovement);
        throwEvent = EventBus.Subscribe<ThrowEvent>(_SetLastJumpAsThrow);
    }

    // Update is called once per frame
    void Update()
    {
        if(movementEnabled && Time.time > nextMovementTime)
        {
            // rb.linearVelocityY = rb.linearVelocityY - gravityAcceleration * Time.deltaTime;
            float currentSpeed = Mathf.Max(playerSpeed, Mathf.Abs(rb.linearVelocityX));
            // move left
            if(Input.GetKey(KeyCode.A))
            {
                // deccelerate
                if(rb.linearVelocityX < -maxSpeedX)
                {
                    currentSpeed = rb.linearVelocityX + (playerAcceleration / 5) * Time.deltaTime;
                }
                // accelerate
                else
                {
                    currentSpeed = rb.linearVelocityX - playerAcceleration * Time.deltaTime;
                }
                rb.linearVelocityX = currentSpeed;
                // rb.linearVelocityX = Mathf.Max(currentSpeed, -maxSpeedX);
                playerDirectionRight = false;
            }
            // move right
            else if(Input.GetKey(KeyCode.D))
            {
                // deccelerate
                if(rb.linearVelocityX > maxSpeedX)
                {
                    currentSpeed = rb.linearVelocityX - (playerAcceleration / 5) * Time.deltaTime;
                }
                // accelerate
                else
                {
                    currentSpeed = rb.linearVelocityX + playerAcceleration * Time.deltaTime;
                }
                rb.linearVelocityX = currentSpeed;
                // rb.linearVelocityX = Mathf.Min(currentSpeed, maxSpeedX);
                playerDirectionRight = true;
            }
            // no movement
            else
            {
                // moving right
                if(rb.linearVelocityX > 0)
                {
                    rb.linearVelocityX = Mathf.Max(rb.linearVelocityX - playerAcceleration * Time.deltaTime, 0);
                }
                // moving left
                else if(rb.linearVelocityX < 0)
                {
                    rb.linearVelocityX = Mathf.Max(rb.linearVelocityX + playerAcceleration * Time.deltaTime, 0);
                }
                // rb.linearVelocityX = 0;
            }
        }
        // jump
        if(Input.GetKeyDown(KeyCode.Space) && (IsGrounded() || IsWalled(true) || IsWalled(false)))
        {
            jumpEndTime = Time.time + jumpDurationTime;
        }
        else if(Input.GetKeyUp(KeyCode.Space) && lastJump != LastJump.Throw)
        {
            if(rb.linearVelocityY > 0)
            {
                rb.linearVelocityY = rb.linearVelocityY / 2;
            }
        }
        
        // holding down jump
        if(Input.GetKey(KeyCode.Space) && Time.time < jumpEndTime)
        {
            if(lastJump == LastJump.Ground)
            {
                rb.linearVelocityY = playerJumpSpeed;
            } 
            else if(lastJump == LastJump.RightWall)
            {
                rb.linearVelocity = new Vector2(-playerWallJumpSpeedX, playerWallJumpSpeedY);
            }
            else if(lastJump == LastJump.LeftWall)
            {
                rb.linearVelocity = new Vector2(playerWallJumpSpeedX, playerWallJumpSpeedY);
            }
        }
        else
        {
            if((IsWalled(false) && Input.GetKey(KeyCode.A)) || (IsWalled(true) && Input.GetKey(KeyCode.D)))
            {

                rb.linearVelocityY = Mathf.Max(rb.linearVelocityY - gravityAcceleration * Time.deltaTime, -maxSlideSpeedY);
            }
            else
            {
                rb.linearVelocityY -= gravityAcceleration * Time.deltaTime;
            }
        }
    }

    bool IsGrounded() {
        if(IsWalled(true) || IsWalled(false))
        {
            return false;
        }
        Vector2 raycastSize = new Vector2(transform.localScale.x - 0.2f, 0.1f);
        Vector2 raycastPos = new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2);
        RaycastHit2D hit = Physics2D.BoxCast(raycastPos, raycastSize, 0f, Vector2.down, groundDistance, groundLayer);

        // Draw the box for visualization
        Debug.DrawLine(raycastPos, raycastPos + Vector2.left * raycastSize.x, Color.red);  // Left side
        Debug.DrawLine(raycastPos, raycastPos + Vector2.right * raycastSize.x, Color.red); // Right side
        Debug.DrawLine(raycastPos, raycastPos + Vector2.up * raycastSize.y, Color.red);    // Top side
        Debug.DrawLine(raycastPos + Vector2.down * raycastSize.y, raycastPos + Vector2.left * raycastSize.x + Vector2.down * raycastSize.y, Color.red); // Bottom side

        if (hit.collider != null) {
            int hitLayer = hit.collider.gameObject.layer;
            Debug.Log("layer = " + LayerMask.LayerToName(hitLayer));
            EventBus.Publish<IsGroundedEvent>(new IsGroundedEvent(true));
            lastJump = LastJump.Ground;
            return true;
        }
        EventBus.Publish<IsGroundedEvent>(new IsGroundedEvent(false));
        return false;
    }

    bool IsWalled(bool isRightWall) {
        Vector2 raycastSize = new Vector2(0.1f, transform.localScale.y - 0.02f);
        Vector2 raycastPos;
        if(isRightWall)
        {
            raycastPos = new Vector2(transform.position.x + transform.localScale.x / 2, transform.position.y);
        }
        else
        {
            raycastPos = new Vector2(transform.position.x - transform.localScale.x / 2, transform.position.y);
        }

        RaycastHit2D hit = Physics2D.BoxCast(raycastPos, raycastSize, 0f, Vector2.left, groundDistance, wallJumpLayer);

        if (hit.collider != null) {
            int hitLayer = hit.collider.gameObject.layer;
            if(isRightWall)
            {
                EventBus.Publish<IsWalledEvent>(new IsWalledEvent(false, true));
                lastJump = LastJump.RightWall;
            }
            else
            {
                EventBus.Publish<IsWalledEvent>(new IsWalledEvent(true, false));
                lastJump = LastJump.LeftWall;
            }
            nextMovementTime = Time.time + wallJumpDisableMovementTime;
            return true;
        }
        EventBus.Publish<IsWalledEvent>(new IsWalledEvent(false, false));
        return false;
    }

    void _ChangeMovement(MovementEvent e)
    {
        movementEnabled = e.isEnabled;
    }

    void _SetLastJumpAsThrow(ThrowEvent e)
    {
        lastJump = LastJump.Throw;
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<MovementEvent>(movementEvent);
    }
}

public class MovementEvent
{
    public bool isEnabled = true;
    public MovementEvent(bool _isEnabled)
    {
        isEnabled = _isEnabled;
    }
}

public class IsGroundedEvent
{
    public bool isGrounded = false;
    public IsGroundedEvent(bool _isGrounded)
    {
        isGrounded = _isGrounded;
    }
}

public class IsWalledEvent
{
    public bool isWalledRight = false;
    public bool isWalledLeft = false;
    public IsWalledEvent(bool _isWalledLeft, bool _isWalledRight)
    {
        isWalledLeft = _isWalledLeft;
        isWalledRight = _isWalledRight;
    }
}
