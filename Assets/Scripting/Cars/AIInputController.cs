using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CarDriver))]
public class AIInputController : MonoBehaviour 
{
    public float TargetX = 0;
    public float TargetZ = 1;

    public float TargetAcceleration = 1;

    public bool Forward;

    public Renderer[] RightSignals;
    public Renderer[] LeftSignals;

    private CarDriver _driver;
    private bool _canChangeDir;
    private bool _canMove;


    private float _steer;
    void Awake()
    {
        _driver = GetComponent<CarDriver>();

        _driver.CurrentAcceleration = TargetAcceleration;

        StartCoroutine(TestTimer());

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




//        Vector3 target = new Vector3(TargetX, transform.position.y, transform.position.z + 50 * Mathf.Sign(TargetZ));
//        float steer = transform.InverseTransformPoint(target).x;
//        _driver.CurrentWheelsSteer = Mathf.Clamp(steer/30,-1,1);


        if (_canMove)
        {
            if (Mathf.Abs(transform.position.x - TargetX) < 0.15f)
            {
                _steer = 0;
                _canMove = false;
            }
            else if (TargetX < transform.position.x)
            {
                _steer = Mathf.Lerp(_steer, -1, Time.deltaTime*0.25f);
            }
            else
            {
                _steer = Mathf.Lerp(_steer, 1, Time.deltaTime*0.25f);
            }
        }

        _driver.CurrentWheelsSteer = _steer;

        if (_canChangeDir)
        {
            var result = Random.Range(0f, 1f);
            if (result > 0.4f)
            {
                StartCoroutine(StartChangeLane());
                float x = Random.value > 0.5f ? 3 : 7;
                if (Forward) x *= -1;
                TargetX = x;
                _canMove = true;
            }
            StartCoroutine(TestTimer());
        }

    }

    private IEnumerator StartChangeLane()
    {
        
    }

    void OnTriggerEnter()
    {
        EventController.PostEvent("car.ai.needdestroy", gameObject);
    }

    private IEnumerator TestTimer()
    {
        _canChangeDir = false;
        yield return new WaitForSeconds(1.5f + Random.Range(0f,2f));
        _canChangeDir = true;
    }
}
