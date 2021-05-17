using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIDDriver : MonoBehaviour
{
    public Transform TEST_GOAL;
    public double TEST_P = 0.3;
    public double PARAM_I = 0.1;
    public double PARAM_D = 0.5;
    public double STATE_ERROR = 0;
    public double STATE_CORRECTION = 0;
    public double STATE_LAST = 0;
    public DroneController mDrone;

    public double STATE_INTERGRAL = 0;
    
    public Graph mGraph;
    // Start is called before the first frame update
    void Start()
    {
        mDrone = FindObjectOfType<DroneController>();
        mGraph = FindObjectOfType<Graph>();
    }

    double clip(double val, double min, double max)
    {
        return Math.Max(min, Math.Min(val, max));
    }
    // Update is called once per frame
    void Update()
    {
        STATE_LAST = mDrone.gameObject.transform.position.y;
        STATE_ERROR = TEST_GOAL.position.y - mDrone.gameObject.transform.position.y;
        STATE_ERROR = clip(STATE_ERROR/10, -1.0, 1.0);


        double dInput = mDrone.gameObject.transform.position.y - STATE_LAST;
        double dTerm = PARAM_D * (dInput / Time.deltaTime);

        STATE_INTERGRAL += (PARAM_I * STATE_ERROR * Time.deltaTime);
        STATE_INTERGRAL = clip(STATE_INTERGRAL, -1.0, 1.0);
        
        STATE_CORRECTION = (STATE_ERROR * TEST_P) + STATE_INTERGRAL + dTerm;
        mGraph.AddVal(((float)STATE_ERROR));
        
        mDrone.ApplyInput(new double[]{STATE_CORRECTION, STATE_CORRECTION, STATE_CORRECTION, STATE_CORRECTION});
    }
}
