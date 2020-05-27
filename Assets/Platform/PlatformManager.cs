using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Prefab;
    public int NumberOfObjects;
    public float RecycleOffset;
    public Vector3 StartPosition;
    public Vector3 minSize, maxSize, minGap, maxGap;
    public float minY, maxY;
    private Vector3 nextPosition;
    private Queue<Transform> objectQueue;
    public Material[] materials;
    public PhysicMaterial[] physicMaterials;
    public Booster booster;
    void Start()
    {
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;


        objectQueue = new Queue<Transform>(NumberOfObjects);
        for (int i = 0; i < NumberOfObjects; i++)
            objectQueue.Enqueue((Transform)Instantiate(Prefab, new Vector3(0f, 0f, -100f), Quaternion.identity));
        enabled = false;
        nextPosition = StartPosition;
    }

    private void GameStart()
    {
        nextPosition = StartPosition;
        for (int i = 0; i < NumberOfObjects; i++) 
            Recycle();
        enabled = true;
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
        booster.SpawnAvailable(position);

        Transform obj = (Transform)objectQueue.Dequeue();
        obj.localScale = scale;
        obj.localPosition = position;
        int materialIndex = Random.Range(0, materials.Length);
        
        var renderer = obj.GetComponent<Renderer>();
        var collider = obj.GetComponent<Collider>();

        renderer.material = materials[materialIndex];
        collider.material = physicMaterials[materialIndex];

        objectQueue.Enqueue(obj);

        nextPosition += new Vector3
        (
            Random.Range(minGap.x, maxGap.x) + scale.x,
            Random.Range(minGap.y, maxGap.y),
            Random.Range(minGap.z, maxGap.z)
        );
        if (nextPosition.y < minY) 
            nextPosition.y = minY + maxGap.y;
        else if (nextPosition.y > maxY)
            nextPosition.y = maxY - maxGap.y;
    }
}
