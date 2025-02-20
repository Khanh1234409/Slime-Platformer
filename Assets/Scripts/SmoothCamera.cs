using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float smoothTime = 0.25f;
    Vector3 offset = new Vector3(0f, 0f, -10f);
    Vector3 velocity = Vector3.zero;
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
        }
        transform.position = target.transform.position + offset;
    }

    void FixedUpdate()
    {
        if(!target)
        {
            target = GameObject.Find("Player");
        }
        Vector3 targetPosition = target.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
