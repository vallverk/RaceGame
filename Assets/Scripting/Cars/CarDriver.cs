using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CarDriver : MonoBehaviour 
{
    public Transform FirstPersonCameraPoint;

    public Renderer[] BreakingHeadLights;

    public Transform VisualLFWheel;
    public Transform VisualLBWheel;
    public Transform VisualRFWheel;
    public Transform VisualRBWheel;

    public WheelCollider PhysicLFWheel;
    public WheelCollider PhysicLBWheel;
    public WheelCollider PhysicRFWheel;
    public WheelCollider PhysicRBWheel;

    public float MaxMotorTorque = 50;
    public float MaxBrakeTorque = 50;
    public float MaxWheelsSteer = 15;
    public float MaxSpeed = 150;

    public Vector3 COM = Vector3.zero;

    [Range(-1,1)]
    public float CurrentAcceleration = 0;
    [Range(-1,1)]
    public float CurrentWheelsSteer = 0;

    public float throttle;

    public float Speed { get { return PhysicRBWheel.rpm * 0.10472f * PhysicRBWheel.radius; } }

    public bool CheckGrounded()
    {
        return PhysicRFWheel.isGrounded && 
            PhysicLFWheel.isGrounded &&
            PhysicLFWheel.isGrounded &&
            PhysicLBWheel.isGrounded;
    }

    void Start()
    {
        gameObject.rigidbody.centerOfMass = COM;
    }

    void Update()
    {
        UpdatePhysics(CurrentAcceleration,CurrentWheelsSteer);
        UpdateVisuals();

        if (BreakingHeadLights.Length > 0)
        {
            if (CurrentAcceleration >= 0 && BreakingHeadLights[0].enabled)
                foreach (Renderer r in BreakingHeadLights)
                    r.enabled = false;

            if (CurrentAcceleration < 0 && !BreakingHeadLights[0].enabled)
                foreach (Renderer r in BreakingHeadLights)
                    r.enabled = true;
        }
    }

    void UpdatePhysics(float Accel, float Steer)
    {
        // some magic...
        float s = Steer * MaxWheelsSteer;// * Mathf.Clamp01(2f - Speed / MaxSpeed);
        PhysicLFWheel.steerAngle = s;
        PhysicRFWheel.steerAngle = s;
        //print(s);

        if (Accel == 0)
        {
            PhysicRBWheel.motorTorque = 0;
            PhysicLBWheel.motorTorque = 0;
            PhysicRBWheel.brakeTorque = MaxBrakeTorque;
            PhysicLBWheel.brakeTorque = MaxBrakeTorque;
        } else
        {
            PhysicRBWheel.brakeTorque = 0;
            PhysicLBWheel.brakeTorque = 0;
            
            if (Speed < MaxSpeed)
            {
                PhysicRBWheel.motorTorque = Accel * MaxMotorTorque;
                PhysicLBWheel.motorTorque = Accel * MaxMotorTorque;
            } else
            {
                PhysicRBWheel.motorTorque = 0;
                PhysicLBWheel.motorTorque = 0;
            }
        }
    }

    void UpdateVisuals()
    {
        if (VisualLFWheel == null)
            return;

        VisualLFWheel.localRotation = Quaternion.Euler(new Vector3(0, 180+CurrentWheelsSteer*MaxWheelsSteer,0));
        VisualRFWheel.localRotation = Quaternion.Euler(new Vector3(0, 180+CurrentWheelsSteer*MaxWheelsSteer,0));

        VisualLBWheel.Rotate(new Vector3(1, 0, 0), PhysicLBWheel.rpm * 360.0f);
        VisualLFWheel.Rotate(new Vector3(1, 0, 0), PhysicLFWheel.rpm * 360.0f);
        VisualRBWheel.Rotate(new Vector3(1, 0, 0), PhysicRBWheel.rpm * 360.0f);
        VisualRFWheel.Rotate(new Vector3(1, 0, 0), PhysicRFWheel.rpm * 360.0f);
    }
}
