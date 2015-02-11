using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CarDriver : MonoBehaviour 
{
    public Transform FirstPersonCameraPoint;

    public bool IsPlayer;

    public Renderer[] BreakingHeadLights;

    public Transform Body;

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

    public AnimationCurve AccelarationCurve;

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
        AccelarationCurve = ConstantsStorage.I.AccelerationCurveSlow;
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
        float acceleration = CurrentAcceleration > 0
            ? CurrentAcceleration*MaxMotorTorque
            : CurrentAcceleration*MaxBrakeTorque;

        acceleration *= 0.5f;

        if (CurrentAcceleration > 0)
        {
            acceleration *= AccelarationCurve.Evaluate(_rigidbody.velocity.z);
        }

        float angle = transform.localEulerAngles.y;

        angle = Mathf.Clamp(angle, 0, 360);




        if ((angle > 269 && angle < 360) || (angle >= 0 && angle < 90))
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

        float rot = _rigidbody.velocity.z*Time.deltaTime;

        VisualLBWheel.Rotate(new Vector3(1, 0, 0), rot);
        VisualLFWheel.Rotate(new Vector3(1, 0, 0), rot);
        VisualRBWheel.Rotate(new Vector3(1, 0, 0), rot);
        VisualRFWheel.Rotate(new Vector3(1, 0, 0), rot);
        
        var angle = Body.localEulerAngles;

        var targetAngle = new Vector3(0, 10*CurrentWheelsSteer, 0);

        angle.y = Mathf.LerpAngle(angle.y, targetAngle.y, Time.deltaTime*7.5f);
        Body.localEulerAngles = angle;

    }
}
