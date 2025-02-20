// using Unity.VisualScripting;
// using UnityEngine;

// public class IsGrounded : MonoBehaviour
// {
//     static bool isGrounded = false;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         EventBus.Publish<IsGroundedEvent>(new IsGroundedEvent(isGrounded));
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         CheckGrounded();
//     }
    
//     bool CheckGrounded() {
//         Vector2 raycastSize = new Vector2(transform.localScale.x - 0.02f, 0.1f);
//         Vector2 raycastPos = new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2);
//         // RaycastHit2D hit1 = Physics2D.Raycast(raycastPos1, Vector2.down, groundDistance, layer);
//         // RaycastHit2D hit2 = Physics2D.Raycast(raycastPos2, Vector2.down, groundDistance, layer);
//         RaycastHit2D hit = Physics2D.BoxCast(raycastPos, raycastSize, 0f, Vector2.down, groundDistance, groundLayer);

//         if (hit.collider != null) {
//             int hitLayer = hit.collider.gameObject.layer;
//             Debug.Log("layer = " + LayerMask.LayerToName(hitLayer));
//             return true;
//         }
//         return false;
//     }
// }


