using UnityEngine;

public class CollectableSlime : MonoBehaviour
{
    [SerializeField] int slimeAmount = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        EventBus.Publish<CollectSlimeEvent>(new CollectSlimeEvent(other.gameObject, slimeAmount));
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}

class CollectSlimeEvent
{
    public GameObject obj;
    public int amount;
    public CollectSlimeEvent(GameObject _obj, int _amount)
    {
        obj = _obj;
        amount = _amount;
    }
}