using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    Vector2 startingPosition;
    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (GameManager.Instance.GetCheckpointPosition() != Vector2.zero)
        {
            transform.position = GameManager.Instance.GetCheckpointPosition();
        }
        else
        {
            startingPosition = transform.position;
        }
    }
}
