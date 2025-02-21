using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEvents : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Get the current scene name
            string currentScene = SceneManager.GetActiveScene().name;

            // Determine the next scene
            string nextScene = (currentScene == "Level 0") ? "Level 1" : "Level 0";

            // Load the next scene
            SceneManager.LoadScene(nextScene);
        }
    }
}