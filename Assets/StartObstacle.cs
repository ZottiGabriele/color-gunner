using UnityEngine;

public class StartObstacle : Obstacle {

    [Header("Start Obstacle specific parameters")]
    [SerializeField] AnimationCurve startAnimationCurve;

    float timer;
    float animationLength;

    // Use this for initialization
    protected override void Start () {
        base.Start();

        animationLength = startAnimationCurve.keys[startAnimationCurve.length - 1].time;
        GameManager.Instance.changeGameState(GameManager.GameState.waiting);
	}

    protected override void Update()
    {
        base.Update();

        timer += Time.deltaTime;
        timer %= animationLength * 2;

        float newScale = startAnimationCurve.Evaluate(timer);

        transform.localScale = Vector3.one * newScale;
    }

    public override void startDestruction()
    {
        base.startDestruction();
        GameManager.Instance.changeGameState(GameManager.GameState.running);
        GameManager.Instance.incrementScoreBy(-GameManager.Instance.getCurrentScore());
    }
}
