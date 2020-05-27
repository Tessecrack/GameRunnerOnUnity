using System;
using System.Linq;
using UnityEngine;

public class Runner : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private bool godMode = false;
    private Vector3 directionRunner = Vector3.zero;
    public Vector3 JumpVelocity, BoostVelocity;
    public AudioSource JumpSound;
    public AudioSource SuperJumpSound;
    
    public static float DistanceTraveled;
    public float Acceleration; 
    private bool touchingPlatform;
    public Rigidbody RigidBody;
    public float GameOverXY;
    private Vector3 startPosition;
    public static int boosts;

    void Start()
    {
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        startPosition = transform.localPosition;
        GetComponent<Renderer>().enabled = false;        
        RigidBody.isKinematic = true;
        enabled = false;
    }
    private void GameStart()
    {
        boosts = 0; 
        DistanceTraveled = 0f;
        transform.localPosition = startPosition;
        GetComponent<Renderer>().enabled = true;
        RigidBody.isKinematic = false;
        GUIManager.SetBoosts(boosts);
        GUIManager.SetDistance(DistanceTraveled);
        enabled = true;
    }
    private void GameOver()
    {
        GetComponent<Renderer>().enabled = false;
        RigidBody.isKinematic = true;
        enabled = false;
    }
     void Update() 
    {   
        Game();
        if (transform.localPosition.y < GameOverXY) 
            GameEventManager.TriggerGameOver();

    }
    void OnCollisionEnter()
    {
        touchingPlatform = true;
    }
    void OnCollisionExit()
    {
        touchingPlatform = false;
    }
     private void Game()
    {
        var smoothingMove = Time.deltaTime * moveSpeed;
        DistanceTraveled = transform.localPosition.x;
        GUIManager.SetDistance(DistanceTraveled);
        JumpRunner();
        MoveRunner(smoothingMove);
    }
    private void MoveRunner(float smoothingMove)
    {
        transform.Translate(-Vector3.left * smoothingMove);
    }

    private void MoveGodMode(float smoothingMove)
    {
        if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.up * smoothingMove );
        if (Input.GetKey(KeyCode.S)) transform.Translate(-Vector3.up * smoothingMove);
        if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.left * smoothingMove);
        if (Input.GetKey(KeyCode.D)) transform.Translate(-Vector3.left * smoothingMove);
    }
    private void JumpRunner()
    {
        if (touchingPlatform && Input.GetButtonDown("Jump") && boosts == 0)
        {
            JumpSound.Play();
            RigidBody.AddForce(JumpVelocity, ForceMode.VelocityChange);
            touchingPlatform = false;
        }
        else if (boosts > 0 && Input.GetButtonDown("Jump") && DistanceTraveled > 0)
        {
            SuperJumpSound.Play();
            RigidBody.AddForce(BoostVelocity, ForceMode.VelocityChange);
            boosts--;
            GUIManager.SetBoosts(boosts);
        }
    }
    public static void AddBoost()
    {
        boosts++;
        GUIManager.SetBoosts(boosts);
    }
}
