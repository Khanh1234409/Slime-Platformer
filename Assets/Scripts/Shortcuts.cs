using UnityEditor;
using UnityEngine;

public class Shortcuts : MonoBehaviour
{
    [SerializeField] GameObject menuOverlay;
    bool menuIsActive = false;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            menuOverlay.SetActive(menuIsActive);
            menuIsActive = !menuIsActive;
        }
    }
}
