using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeHealthOnTouch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == GameObject.Find("Player"))
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            EventBus.Publish<RespawnEvent>(new RespawnEvent());
        }
    }
}