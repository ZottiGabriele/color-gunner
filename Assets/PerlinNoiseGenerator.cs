using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGenerator : Singleton<PerlinNoiseGenerator> {

    [SerializeField] float scale = 1;
    [SerializeField] int numOfValues = 250;

    public Queue<float[]> animations = new Queue<float[]>();

    private void Update()
    {
        if (animations.Count > 5) { return; }

        //animations.Enqueue(genereteAnim(Random.Range(0, 45), Random.Range(90, 180)));
        animations.Enqueue(genereteAnim(20, Random.Range(40, 180)));
    }
    
    public float[] genereteAnim(float min, float max)
    {
        int counter = numOfValues;

        float[] values = new float[numOfValues];
        float[] result = new float[numOfValues * 2];

        float y = Random.Range(min, max);

        for (int i = 0; i < numOfValues; i++)
        {
            values[i] = getPerlinValueMapped(min, max, (float)i / numOfValues, y);
            result[i] = values[i];
        }

        for (int i = numOfValues - 1; i >= 0; i--) {
            result[counter] = values[i];
            counter++;
        }

        return result;
    }


    float getPerlinValueMapped(float min, float max, float x, float y) {

        float val = Mathf.PerlinNoise(x * scale, y);
        
        if (max > min) {
            val *= (max - min);
        } else {
            Debug.LogError("Tried to map a max < min, returning [0,1] perlin value");
        }

        return min + val;
    }



    public float[] getAnim() {
        return animations.Dequeue();
    }
}
