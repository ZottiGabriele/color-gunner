using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : MonoBehaviour {

    [SerializeField] float shootSpeed;
    [SerializeField] bool shouldSpin;
    [SerializeField] float spinningSpeed;

    Rigidbody2D myRigidbody;
    SpriteRenderer myRenderer;
    ParticleSystem myDeathVisualEffect;
    bool shouldReactToInput = true;

    private void Start()
    {
        GameManager.Instance.onGameStateChanged += OnGameStateChanged;

        myRigidbody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myDeathVisualEffect = GetComponentInChildren<ParticleSystem>(true);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null) {
            GameManager.Instance.onGameStateChanged -= OnGameStateChanged;
        }
    }

    void OnGameStateChanged(GameManager.GameState newGameState)
    {
        switch (newGameState)
        {
            case GameManager.GameState.waiting:
            case GameManager.GameState.running:
                shouldReactToInput = true;
                break;
            case GameManager.GameState.game_over:
                shouldReactToInput = false;
                break;
            case GameManager.GameState.restarting:
                resetPosition();
                startFadeInAnimation();
                break;
        }
    }

    void Update () {
        

        if (shouldReactToInput)
        {
            processInput();
        }

        if (shouldSpin)
        {
            spin();
        }
    }

    void processInput() {

        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            shoot();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            resetPosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Obstacle hitObstacle;

        if (hitObstacle = other.GetComponentInParent<Obstacle>()) {

            if (hitObstacle.checkForValidCollision())
            {
                hitObstacle.startDestruction();
                hitObstacle.addPointsToScore();
                resetPosition();
            }
            //TODO: REMOVE
            else if (GameManager.Instance.debug) {
                resetPosition();
            } else
            {
                myRigidbody.velocity = Vector2.zero;
                myDeathVisualEffect.Play();
                myRenderer.enabled = false;
                GameManager.Instance.changeGameState(GameManager.GameState.game_over);
            }
        }
    }

    private void FixedUpdate()
    {
        slowDown();

        shouldReactToInput = (myRigidbody.velocity.y == 0);
    }

    void slowDown() {

        //TODO: have this method slow down movement in directions other then up

        if (myRigidbody.velocity.y > 0)
        {
            myRigidbody.AddForce(Vector2.down * 9.81f);
        }
        else if (myRigidbody.velocity.y < 0)
        {
            myRigidbody.velocity = Vector3.zero;
        }
    }

    void spin()
    {
        transform.localEulerAngles += Vector3.forward * spinningSpeed * Time.deltaTime;
    }

    void shoot()
    {
        //TODO: have this method shoot in directions other then up

        myRigidbody.AddForce(Vector2.up * shootSpeed, ForceMode2D.Impulse);
    }

    void resetPosition() 
    {
        transform.position = Vector3.zero;
        myRigidbody.velocity = Vector3.zero;
    }

    void startFadeInAnimation() {
        myRenderer.enabled = true;
        StartCoroutine(fadeInAnimation());
    }

    IEnumerator fadeInAnimation() {
        var col = myRenderer.color * new Color(1,1,1,0);
        while (col.a < 1) {
            col += new Color(0, 0, 0, 1.5f * Time.deltaTime);
            myRenderer.color = col;
            yield return null;
        }
    }
}
