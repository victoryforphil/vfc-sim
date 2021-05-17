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

    public double THRUST_SCALER = 5.0;
    
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
            Vector3 _forceVector = new Vector3( 0,(float)(motors[i] * THRUST_SCALER), 0);


            mRigidbody.AddForceAtPosition(mMotorPositions[i].TransformDirection(_forceVector)  , mMotorPositions[i].position);
        }
    }
}
