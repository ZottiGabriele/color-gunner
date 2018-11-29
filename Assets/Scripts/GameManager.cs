using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    public bool debug;

    public Action<GameState> onGameStateChanged = (GameState gs) => {};

    public enum GameState {
        waiting,
        running,
        paused,
        game_over,
        restarting
    }

    [SerializeField] GameState currentState = GameState.running;
    [SerializeField] int currentScore = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    public GameState getCurrentGameState() {
        return currentState;
    }

    public void changeGameState(GameState newGameState) {
        currentState = newGameState;
        onGameStateChanged(newGameState);
    }


    public void incrementScoreBy(int increment) {
        currentScore += increment;
    }

    public int getCurrentScore() {
        return currentScore;
    }
}
