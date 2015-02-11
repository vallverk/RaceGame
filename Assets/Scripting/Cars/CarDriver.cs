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

    private Rigidbody _rigidbody;

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
        _rigidbody = gameObject.rigidbody;
    }

    void Update()
    {
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

    void FixedUpdate()
    {
        UpdatePhysics();
    }

    void UpdatePhysics()
    {
        // some magic...
        float steer = CurrentWheelsSteer * MaxWheelsSteer;
        float acceleration = CurrentAcceleration * MaxMotorTorque;

        float angle = transform.localEulerAngles.y;

        angle = Mathf.Clamp(angle, 0, 360);


        if ((angle > 269 && angle < 359) || (angle >= 0 && angle < 90))
        {

        }
        else
        {
            acceleration = -acceleration;
        }

        var velocity = _rigidbody.velocity;


        velocity += new Vector3(0,0,acceleration*Time.fixedDeltaTime);

        velocity.z = Mathf.Clamp(velocity.z, -MaxSpeed, MaxSpeed);

        _rigidbody.velocity = velocity;

        _rigidbody.MovePosition(transform.position + new Vector3(steer*Time.fixedDeltaTime, 0, 0));
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
