using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : Singleton<ObstacleManager> {

    [SerializeField] GameObject startingObstaclePrefab;
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] GameObject rand;

    List<Obstacle> obstacles = new List<Obstacle>();

    private void Start()
    {
        GameManager.Instance.onGameStateChanged += OnGameStateChanged;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onGameStateChanged -= OnGameStateChanged;
        }
    }

    void OnGameStateChanged(GameManager.GameState newGameState) {
        switch(newGameState) {
            case GameManager.GameState.game_over:
                foreach(var obs in obstacles) {
                    obs.startGameOverAnimation();
                }
                StartCoroutine(waitForGameOverAnimations());
                break;
        }
    }

    public void registerObstacle(Obstacle caller) {
        obstacles.Add(caller);
    }

    public void unregisterObstacle(Obstacle caller)
    {
        if (obstacles.Contains(caller)) {
            obstacles.Remove(caller);
            onUnregisteredObstacle();
        } else {
            Debug.LogError(caller.gameObject + " tried to unregister from the ObstacleManager but never registered");
        }
    }

    void onUnregisteredObstacle() {
        if (GameManager.Instance && GameManager.Instance.getCurrentGameState() == GameManager.GameState.running) {
            spawnNewObstacle();
        }
    }

    IEnumerator waitForGameOverAnimations() {
        while(obstacles.Count > 0) {
            yield return null;
        }
        spawnStartingObstacle();
    }

    public void spawnNewObstacle() {

        bool flip = Random.Range(0, 1) + 0.5f > 1;
        float zRotationOffset = Random.Range(0, 360);

        //var inst = Instantiate(obstaclePrefab, Vector3.zero, Quaternion.Euler(new Vector3(0, flip ? 0 : 180, zRotationOffset)), transform);

        //int seed = Random.Range((int)1, (int)4);
        //Debug.Log(seed);
        ////TODO: here should be initialized the obstacle
        //switch (seed) {
        //    case 1:
        //        inst.GetComponent<Animator>().Play("Target_Anim_1");
        //        break;
        //    case 2:
        //        inst.GetComponent<Animator>().Play("Target_Anim_2");
        //        break;
        //    case 3:
        //        inst.GetComponent<Animator>().Play("Target_Anim_3");
        //        break;
        //}

        Instantiate(rand, Vector3.zero, Quaternion.Euler(new Vector3(0, flip ? 0 : 180, zRotationOffset)), transform);

    }

    void spawnStartingObstacle() {
        Instantiate(startingObstaclePrefab);
        //TODO: animate
        GameManager.Instance.changeGameState(GameManager.GameState.restarting);
    }
}

