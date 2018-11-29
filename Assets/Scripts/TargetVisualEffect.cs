using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TargetVisualEffect : MonoBehaviour {

    Obstacle myObstacle;
    Material myMaterial;
    Color myTargetColor;

    bool doIt;

    private void Start()
    {
        myMaterial = GetComponent<SpriteRenderer>().material;
        myObstacle = GetComponentInParent<Obstacle>();
        myMaterial.SetColor("_ObstacleColor", Color.clear);
        myTargetColor = myMaterial.GetColor("_TargetColor");
    }

    public void startVisualEffect() {

        myMaterial.SetFloat("_TargetRange", myObstacle.getTargetRange());
        StartCoroutine(visualEffect());
    }

    IEnumerator visualEffect() {

        while(true) {
            myMaterial.SetColor("_TargetColor", new Color(myTargetColor.r, myTargetColor.g, myTargetColor.b, 1 - myObstacle.getDissolve()));
            transform.localEulerAngles = Vector3.forward * myObstacle.getTargetRange();
            transform.localPosition = Vector3.up * -myObstacle.getDissolve() + Vector3.right * -myObstacle.getDissolve();
            transform.localEulerAngles = Vector3.zero;
            yield return new WaitForEndOfFrame();
        }
    }
}
