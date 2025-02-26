// using System.Numerics;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float smoothTime = 0.25f;
    // [SerializeField] float maxCameraDistance = 3;
    Vector3 offset = new Vector3(0f, 0f, -10f);
    Vector3 velocity = Vector3.zero;
    Rigidbody2D targetRb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SnapCamera();
    }

    void SnapCamera()
    {
        if(!target)
        {
            target = GameObject.Find("Player");
            targetRb = target.GetComponent<Rigidbody2D>();
        }
        transform.position = target.transform.position + offset;
    }

    void FixedUpdate()
    {
        if(!target)
        {
            target = GameObject.Find("Player");
        }
        targetRb = target.GetComponent<Rigidbody2D>();
        // Vector3 velocity = new Vector3(targetRb.linearVelocity.x, targetRb.linearVelocity.y, 0);
        // Vector3 velocityOffset = Vector3.ClampMagnitude(velocity, maxCameraDistance);
        // Vector3 targetPosition = target.transform.position + velocityOffset + offset;
        Vector3 targetPosition = target.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
