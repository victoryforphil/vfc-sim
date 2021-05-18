using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIDDriver : MonoBehaviour
{
    public Transform goal;
    public DroneController mDrone;
    public Graph mGraph;


    public PIDController altController, rollController, posRollController, posPitchController, pitchController;
    // Start is called before the first frame update
    void Start()
    {
        mDrone = FindObjectOfType<DroneController>();
        mGraph = FindObjectOfType<Graph>();
        
        posRollController = new PIDController().Tune(0.1f, 0.5f, 0.1f, -1.0f, 1.0f);
        posPitchController = new PIDController().Tune(0.1f, 0.5f, 0.1f, -1.0f, 1.0f);
        altController = new PIDController().Tune(0.2f, 0.1f, 0.5f, -1.0f, 1.0f);
        rollController = new PIDController().Tune(0.32f, 0.1f,0.1f, -1.0f, 1.0f);
        pitchController = new PIDController().Tune(0.2f, 0.1f,0.1f, -1.0f, 1.0f);
    }

    float clip(float val, float min, float max)
    {
        return Math.Max(min, Math.Min(val, max));
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        /*
         *
         *  Position Roll PId
         * 
         */

        float currentX = mDrone.gameObject.transform.position.x;
        float goalX= goal.position.x;
        float errorPosPitch = currentX - goalX;
        errorPosPitch /= 20.0f;
        errorPosPitch *= -1;
        errorPosPitch = clip(errorPosPitch, -1.0f, 1.0f);
        
        float outPosPitch = posPitchController.process(errorPosPitch);
       // outPosPitch -= 0.1f * (mDrone.GetComponent<Rigidbody>().velocity.x );

        float posPitchAngle = outPosPitch * 0.3f;
        
        /*
         *
         *  Position Roll PId
         * 
         */

        float currentZ = mDrone.gameObject.transform.position.z;
        float goalZ = goal.position.z;
        float errorPosRoll = currentZ - goalZ;
        errorPosRoll /= 20.0f;
        errorPosRoll = clip(errorPosRoll, -1.0f, 1.0f);
        
        float outPosRoll = posRollController.process(errorPosRoll);
       // outPosRoll += 0.1f * (mDrone.GetComponent<Rigidbody>().velocity.z);
        mGraph.AddVal(errorPosRoll);

        float posRollAngle = outPosRoll * 0.3f;




        float currentHeight = mDrone.gameObject.transform.position.y;
        float goalHeight = goal.position.y;
        float errorHeight = goalHeight - currentHeight;
        errorHeight /= 10.0f;
        errorHeight = clip(errorHeight, -1.0f, 1.0f);
        float outHeight = altController.process(errorHeight);

        outHeight += 1.0f;
        outHeight /= 2.0f;
        outHeight = Mathf.Clamp(outHeight,0.0f, 1.0f);
    

        /*
         *
         * ROLL CONTROL PID
         *
         * 
         */

        float currentRoll = mDrone.gameObject.transform.rotation.x;
        float targetRoll = (currentRoll - posRollAngle);
        
        float outRoll = rollController.process(targetRoll );
        mDrone.ApplyInput(new double[]{outRoll + outHeight,outRoll + outHeight,outHeight-outRoll,outHeight-outRoll});
        
        /*
         *
         * PITCH CONTROL PID
         *
         * 
         */

        float currentPitch = mDrone.gameObject.transform.rotation.z;
        float targetPitch = (currentPitch - posPitchAngle);
        
        float outPitch = pitchController.process(targetPitch );
        mDrone.ApplyInput(new double[]{outRoll + outHeight - outPitch,outRoll + outHeight + outPitch,outHeight-outRoll - outPitch,outHeight-outRoll + outPitch});

    }
}
