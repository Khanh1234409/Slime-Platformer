using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnityEngine.Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
