using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesSystemManager : MonoBehaviour
{
    public ParticleSystem[] particleSystems;
    void Start()
    {
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
    }
    private void GameStart()
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Clear();
            particleSystems[i].enableEmission = true;
        }
    }
    private void GameOver()
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].enableEmission = false;
        }
    }

}
