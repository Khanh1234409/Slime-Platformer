using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Cursor.visible = gameManager.menuIsActive;
        // transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
