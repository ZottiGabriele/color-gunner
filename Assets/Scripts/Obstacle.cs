using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class Obstacle : MonoBehaviour {

    [Header("General proprieties")]
    [SerializeField] int pointsOnValidHit = 1;
    [SerializeField] float gameOverAnimValue;

    [Header("Obstacle difficulty proprieties")]
    [SerializeField] float spinningSpeed;
    [SerializeField] float maxShrinkingSpeed;
    [SerializeField] float minShrinkingSpeed;
    [SerializeField] float minShrinkScaleTrigger;


    [Header("Shader porprieties")]
    [Range(0, 360)]
    [SerializeField] protected float targetRange;
    [Range(0, 1)]
    [SerializeField] float dissolve;
    [SerializeField] Color targetColor = new Color(0, 0, 0, 1);
    [SerializeField] Color obstacleColor = new Color(1, 1, 1, 1);

    float currentShrinkingSpeed;

    Material myMaterial;
    protected Animator myAnimator;

    protected virtual void Start()
    {
        ObstacleManager.Instance.registerObstacle(this);

        myMaterial = GetComponent<SpriteRenderer>().material;
        myAnimator = GetComponent<Animator>();
    }

    protected virtual void OnDestroy()
    {
        if (ObstacleManager.Instance)
            ObstacleManager.Instance.unregisterObstacle(this);
    }

    protected virtual void Update()
    {
        updateShaderValues();
        spin();
        shrink();
    }

    void shrink() {

        currentShrinkingSpeed = Mathf.Max(maxShrinkingSpeed * (transform.localScale.x - minShrinkScaleTrigger), minShrinkingSpeed);

        transform.localScale -= Vector3.one * currentShrinkingSpeed * Time.deltaTime;

        if (transform.localScale.x < 0) { Destroy(gameObject); }
    }

    void spin() {
        transform.localEulerAngles += Vector3.forward * spinningSpeed * Time.deltaTime;
    }

    /*
     * Checks whether or not a player collision has happened on the target
     * part of the obstacle (valid) or not. 
     * This method should be invoked by the player on a collision
     * 
     * @return true if the collision is valid
    */
    public bool checkForValidCollision()
    {
        float normalizedStart = (transform.localEulerAngles.z + 180) % 360;

        float normalizedStartRad = normalizedStart * Mathf.Deg2Rad;
        float normalizedEndRad = (normalizedStart + targetRange) * Mathf.Deg2Rad;

        float cosStart = Mathf.Cos(normalizedStartRad);
        float cosEnd = Mathf.Cos(normalizedEndRad);

        return (cosStart > .12f && cosEnd < -0.12f) || (cosStart > .12f || cosEnd < -0.12f) && targetRange > 180f;
    }

    public void addPointsToScore() {
        GameManager.Instance.incrementScoreBy(pointsOnValidHit);
    }

    public virtual void startDestruction()
    {
        myAnimator.SetTrigger("Destroy");
        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponentInChildren<TargetVisualEffect>().startVisualEffect();
    }

    public void endDestruction() {
        Destroy(gameObject);
    }

    public void startGameOverAnimation() {
        myAnimator.SetTrigger("GameOver");
        StartCoroutine(gameOverAnimation(currentShrinkingSpeed, maxShrinkingSpeed));
        maxShrinkingSpeed = 0;
        minShrinkingSpeed = 0;
    }

    IEnumerator gameOverAnimation(float initialShrinkSpeed, float initialMaxShrinkSpeed) {

        while(transform.localScale.x > 0) {
            Vector3 dim = Vector3.Lerp(Vector3.one * initialShrinkSpeed, Vector3.one * initialMaxShrinkSpeed / 1.5f, gameOverAnimValue);
            transform.localScale -= dim * Time.deltaTime;
            yield return null;
        }
    }

    /*
     * Passes this class values to the Obstacle material custom shader 
     * in order to update its status
    */
    void updateShaderValues()
    {
        myMaterial.SetFloat("_TargetRange", targetRange);
        myMaterial.SetFloat("_Dissolve", dissolve);
        myMaterial.SetColor("_Target_Color", targetColor);
        myMaterial.SetColor("_Obstacle_Color", obstacleColor);
    }

    public float getTargetRange() {
        return targetRange;
    }

    public float getDissolve()
    {
        return dissolve;
    }
}
