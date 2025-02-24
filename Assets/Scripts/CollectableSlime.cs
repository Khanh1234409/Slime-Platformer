using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CollectableSlime : MonoBehaviour
{
    [SerializeField] int slimeAmount = 1;
    [SerializeField] List<string> tagsThatCanCollect;
    [SerializeField] float collectedOpacity = 0.3f;
    [SerializeField] float respawnTime = 5;
    float nextRespawnTime;

    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider2D;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        nextRespawnTime = Time.time;
    }

    void Update()
    {
        Debug.Log(nextRespawnTime);
        if(Time.time > nextRespawnTime)
        {
            Color color = spriteRenderer.color;
            color.a = 1;
            spriteRenderer.color = color;
            GetComponent<BoxCollider2D>().enabled = true;
            spriteRenderer.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach(string tag in tagsThatCanCollect)
        {
            if(other.gameObject.CompareTag(tag))
            {
                EventBus.Publish<CollectSlimeEvent>(new CollectSlimeEvent(other.gameObject, slimeAmount));
                Color color = spriteRenderer.color;
                color.a = collectedOpacity;
                spriteRenderer.color = color;
                GetComponent<BoxCollider2D>().enabled = false;
                nextRespawnTime = Time.time + respawnTime;
                Debug.Log(nextRespawnTime);
                return;
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