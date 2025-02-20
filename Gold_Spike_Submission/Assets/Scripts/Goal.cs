using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] int level;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            EventBus.Publish<GoalEvent>(new GoalEvent(level));
        }
    }
}

public class GoalEvent
{
    public int level;
    public GoalEvent(int _level)
    {
        level = _level;
    }
}
