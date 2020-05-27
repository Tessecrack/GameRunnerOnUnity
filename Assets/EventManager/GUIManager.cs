using UnityEngine;
using UnityEngine.UI;
public class GUIManager : MonoBehaviour 
{
    private static GUIManager instance;
    public Text gameOverText, instructionsText, runnerText, boostsText, distanceText;
    void Start()
    {        
        instance = this;
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver +=  GameOver;
        gameOverText.enabled = false;
    }
    public static void SetBoosts(int boosts) => instance.boostsText.text = boosts.ToString();
    public static void SetDistance(float distance) => instance.distanceText.text = distance.ToString("f0");
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            GameEventManager.TriggerGameStart();
    }
    private void GameStart()
    {

        gameOverText.enabled = false;
        instructionsText.enabled = false;
        runnerText.enabled = false;
        enabled = false;
    }
    private void GameOver()
    {
        gameOverText.enabled = true;
        instructionsText.enabled = true;        
        enabled = true;
    }
}