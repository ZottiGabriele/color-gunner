using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text), typeof(Animator))]
public class ScoreCounter : MonoBehaviour {

    Text myText;
    Animator myAnimator;

    bool shouldFadeIn = true;

    private void Start()
    {
        myText = GetComponent<Text>();
        myAnimator = GetComponent<Animator>();
        myText.enabled = true;
        GameManager.Instance.onGameStateChanged += OnGameStateChanged;
    }

    void OnGameStateChanged(GameManager.GameState newGameState) {
        switch (newGameState)
        {
            case GameManager.GameState.running:
                if (shouldFadeIn) {
                    myAnimator.SetTrigger("FadeIn");
                    shouldFadeIn = false;
                }
                break;
        }
    }

    private void Update()
    {
        myText.text = GameManager.Instance.getCurrentScore().ToString();
    }
}
