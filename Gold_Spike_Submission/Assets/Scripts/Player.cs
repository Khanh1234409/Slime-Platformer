using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 startingPosition;

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
