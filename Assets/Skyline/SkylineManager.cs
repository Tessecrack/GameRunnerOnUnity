using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkylineManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Prefab;
    public int NumberOfObjects;
    public float RecycleOffset;
    public Vector3 StartPosition;
    public Vector3 minSize, maxSize;
    private Vector3 nextPosition;
    private Queue<Transform> objectQueue;
    void Start()
    {
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        objectQueue = new Queue<Transform>(NumberOfObjects);
        for (int i = 0; i < NumberOfObjects; i++)
            objectQueue.Enqueue((Transform)Instantiate(Prefab));
        nextPosition = StartPosition;
    }

    private void GameStart()
    {
        nextPosition = StartPosition;
        for (int i = 0; i < NumberOfObjects; i++) 
            Recycle();
            this.enabled = true;
    }
    private void GameOver()
    {
        enabled = false;
    }

    void Update()
    {
        if (objectQueue.Peek().localPosition.x + RecycleOffset < Runner.DistanceTraveled)
        {
            Recycle();
        }
    }

    private void Recycle()
    {
        var scale = new Vector3
        (
            Random.Range(minSize.x, maxSize.x),
            Random.Range(minSize.y, maxSize.y),
            Random.Range(minSize.z, maxSize.z)
        );
        
        var position = nextPosition; 
        position.x += scale.x * 0.5f;
        position.y += scale.y * 0.5f;

        Transform obj = (Transform)objectQueue.Dequeue();
        obj.localScale = scale;
        obj.localPosition = position;
        nextPosition.x += scale.x;
        objectQueue.Enqueue(obj);
    }
}
