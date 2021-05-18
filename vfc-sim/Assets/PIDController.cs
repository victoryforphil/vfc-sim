using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIDController
{
    // Start is called before the first frame update
    public float kP, kI, kD, kMin, kMax;

    private float mLastError, mErrorSum;
    
    public PIDController()
    {
        
    }

    
    public PIDController Tune(float p, float i, float d, float min, float max)
    {
        kP = p;
        kI = i;
        kD = d;
        kMax = max;
        kMin = min;
        return this;
    }

    public PIDController reset()
    {
        return this;
    }
    
    public float process(float error)
    {
        float output =0f;
        
        float pTerm = kP * error;

        mErrorSum += Time.fixedDeltaTime * error;
        mErrorSum = Mathf.Clamp(mErrorSum, kMin, kMax);

        float iTerm = kI * mErrorSum;

        float dTerm = (error - mLastError) / Time.fixedDeltaTime;
        //dTerm = Mathf.Clamp(dTerm, kMin, kMax);
        dTerm *= kD;
     
        mLastError = error;
        
        output = pTerm + iTerm + dTerm;
        output = Mathf.Clamp(output, kMin, kMax);
       
        return output;
    }
    
    float clamp(float val, float min, float max)
    {
        return Math.Max(min, Math.Min(val, max));
    }

    
}
