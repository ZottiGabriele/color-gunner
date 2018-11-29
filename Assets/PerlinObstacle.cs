using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinObstacle : Obstacle
{
    [SerializeField] float[] myAnimValues;

    int counter;

    public override void startDestruction()
    {
        base.startDestruction();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void Start()
    {
        base.Start();
        myAnimValues = PerlinNoiseGenerator.Instance.getAnim();
    }

    protected override void Update()
    {
        animateWithPerlinNoise();
        base.Update();
    }

    void animateWithPerlinNoise() {
        counter %= myAnimValues.Length;
        base.targetRange = myAnimValues[counter];
        counter++;
    }
}
