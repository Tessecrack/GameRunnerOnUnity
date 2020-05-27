using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public Vector3 offset, rotationVelocity;
    public AudioSource TouchBoosterSound;
    public float recycleOffset, spawnChance;
    public void SpawnAvailable(Vector3 position) 
    {
        if (gameObject.activeSelf || spawnChance <= Random.Range(0f, 100f)) return;
        transform.localPosition = position + offset;
        gameObject.SetActive(true);
    }
    void Start()
    {
        GameEventManager.GameOver += GameOver;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.x + recycleOffset < Runner.DistanceTraveled)
        {
            gameObject.SetActive(false);
            return;
        }
        transform.Rotate(rotationVelocity * Time.deltaTime);
    }
    void OnTriggerEnter()
    {
        Runner.AddBoost();
        TouchBoosterSound.Play();
        gameObject.SetActive(false);
    }
    private void GameOver()
    {
        gameObject.SetActive(false);
    }
}
