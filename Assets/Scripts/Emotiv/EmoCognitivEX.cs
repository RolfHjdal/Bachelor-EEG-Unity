using UnityEngine;
using System.Collections;

public class EmoCognitivEX
{
   	public static float threshold;
    public float power;
    public float[] powerSamples;
    public bool smoothMode = false;//Enable Smoothen Value From Epoc
   
    void SwitchSmoothMode()
    {
        smoothMode = !smoothMode;
    }

    public EmoCognitivEX()
    {
        power = 0;
        powerSamples = new float[6];
        for (int i = 0; i < 6; i++)
        {
            powerSamples[i] = 0;
        }
    }

    public void ClearSamples()
    {
        for (int i = 0; i < 6; i++)
        {
            powerSamples[i] = 0;
        }
    }
    public float Average(float newcome)
    {
        float total = 0.0f;
        for (int k = 0; k < 5; k++)
        {
            powerSamples[k] = powerSamples[k + 1];
        }

        powerSamples[5] = newcome;

        for (int k = 0; k < 6; k++)
        {
            total += powerSamples[k];
        }
        return total / 6;
    }
    public void UpdatePower(float iPower)
    {
        power = Average(iPower);
    }
}
	
