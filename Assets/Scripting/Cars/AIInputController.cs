using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CarDriver))]
public class AIInputController : MonoBehaviour 
{
    public float TargetX = 0;
    public float TargetZ = 1;

    public float TargetAcceleration = 1;

    private CarDriver _driver;
    
    void Awake()
    {
        _driver = GetComponent<CarDriver>();

        _driver.CurrentAcceleration = TargetAcceleration;

    }

    void Update()
    {
        RaycastHit hit;
        if (rigidbody.SweepTest(transform.forward,out hit,15))
        {
            _driver.CurrentAcceleration = - (2 -Mathf.Clamp(hit.distance/5,0,2));
#if UNITY_EDITOR
            //Debug.DrawLine(transform.position+transform.up, transform.position+transform.up+transform.forward*10, Color.red);
#endif
        } else
        {
            _driver.CurrentAcceleration = TargetAcceleration;
#if UNITY_EDITOR
            //Debug.DrawLine(transform.position+transform.up, transform.position+transform.up+transform.forward*10,Color.green);
#endif
        }

        Vector3 target = new Vector3(TargetX, transform.position.y, transform.position.z + 50 * Mathf.Sign(TargetZ));
        float steer = transform.InverseTransformPoint(target).x;
        _driver.CurrentWheelsSteer = Mathf.Clamp(steer/30,-1,1);
    }

    void OnTriggerEnter()
    {
        EventController.PostEvent("car.ai.needdestroy", gameObject);
    }
}
