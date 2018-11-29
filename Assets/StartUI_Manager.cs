using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas), typeof(Animator))]
public class StartUI_Manager : MonoBehaviour {

    Canvas myCanvas;
    Animator myAnimator;

    private void Start()
    {
        myCanvas = GetComponent<Canvas>();
        myAnimator = GetComponent<Animator>();
        GameManager.Instance.onGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null) {
            GameManager.Instance.onGameStateChanged -= OnGameStateChanged;
        }
    }

    void OnGameStateChanged(GameManager.GameState newGameState)
    {
        switch(newGameState) {
            case GameManager.GameState.waiting:
                myAnimator.SetTrigger("FadeIn");
                break;
            case GameManager.GameState.running:
                myAnimator.SetTrigger("FadeOut");
                break;
        }
    }

}
