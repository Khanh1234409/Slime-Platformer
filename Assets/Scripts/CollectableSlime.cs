using System.Collections.Generic;
using UnityEngine;

public class CollectableSlime : MonoBehaviour
{
    [SerializeField] int slimeAmount = 1;
    [SerializeField] List<string> tagsThatCanCollect;

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach(string tag in tagsThatCanCollect)
        {
            if(other.gameObject.CompareTag(tag))
            {
                EventBus.Publish<CollectSlimeEvent>(new CollectSlimeEvent(other.gameObject, slimeAmount));
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
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