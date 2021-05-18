using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class DroneController : MonoBehaviour
{
    private Rigidbody mRigidbody;

    public float THRUST_SCALER = 5.0f;
    
    public Transform[] mMotorPositions;
    // Start is called before the first frame update
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyInput(double[] motors)
    {
        for(int i=0;i<mMotorPositions.Length;i++)
        {
            float val = (float)motors[i];
            val = Math.Max(0, val);
            
            Vector3 _forceVector = new Vector3( 0,val * THRUST_SCALER, 0);

            mMotorPositions[i].localScale = new Vector3(0.2f,(val/4),0.2f);
            mRigidbody.AddForceAtPosition(mMotorPositions[i].TransformDirection(_forceVector)  , mMotorPositions[i].position);
            
            
            
        }
    }
}
