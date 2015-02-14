using System;
using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters;
using Random = UnityEngine.Random;

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



    private bool BlinkingLeft;
    private bool BlinkOn;

    private bool _setVelocity;


    void Awake()
    {
        _driver = GetComponent<CarDriver>();

        _driver.CurrentAcceleration = TargetAcceleration;

        StartCoroutine(TestTimer());

        ActivateSignals(LeftSignals, false);
        ActivateSignals(RightSignals, false);
    }

    void Update()
    {
        UpdateMovement();
        UpdateLineChanging();
    }

    private void UpdateMovement()
    {
        if (!_setVelocity)
        {
            rigidbody.velocity += new Vector3(0,0, Forward ? -15f : 15f);
            _setVelocity = true;
        }

        RaycastHit hit;
        if (rigidbody.SweepTest(transform.forward, out hit, 15))
        {
            _driver.CurrentAcceleration = -(2 - Mathf.Clamp(hit.distance/5, 0, 2));
        }
        else
        {
            _driver.CurrentAcceleration = TargetAcceleration;
        }


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
    }

    private void UpdateLineChanging()
    {
        if (_canChangeDir)
        {
            var result = Random.Range(0f, 1f);
            if (result > 0.25f)
            {
                StartCoroutine(StartChangeLane());
            }
            else
            {
                StartCoroutine(TestTimer());
            }
        }
    }

    private IEnumerator StartChangeLane()
    {
        _canChangeDir = false;

        BlinkingLeft = Random.value > 0.5f;
        float x = BlinkingLeft ? 3 : 7;
        if (Forward) x *= -1;

        if (Math.Abs(x - TargetX) < 0.1f)
        {
            yield return null;
        }
        else
        {
            int blinksCount = 2 + Random.Range(0, 1);

            ActivateSignals(LeftSignals, false);
            ActivateSignals(RightSignals, false);

            for (int i = 0; i < blinksCount; i++)
            {
                ActivateSignals(GetSignals(), true);
                yield return new WaitForSeconds(0.2f);
                ActivateSignals(GetSignals(), false);
                yield return new WaitForSeconds(0.2f);
            }

            TargetX = x;
            _canMove = true;

            StartCoroutine(TestTimer());

            for (int i = 0; i < blinksCount*2; i++)
            {
                ActivateSignals(GetSignals(), true);
                yield return new WaitForSeconds(0.2f);
                ActivateSignals(GetSignals(), false);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    private Renderer[] GetSignals()
    {
        return BlinkingLeft ? LeftSignals : RightSignals;
    }    
    private Renderer[] GetOppositeSignals()
    {
        return BlinkingLeft ? RightSignals : LeftSignals;
    }

    private void ActivateSignals(Renderer[] signals, bool activate)
    {
        foreach (var signal in signals)
        {
            if (signal == null)
            {
                Debug.Log(gameObject.name);
            }
            signal.enabled = activate;
        }
    }

    void OnTriggerEnter()
    {
        EventController.PostEvent("car.ai.needdestroy", gameObject);
    }

    private IEnumerator TestTimer()
    {
        _canChangeDir = false;
        yield return new WaitForSeconds(0.5f + Random.Range(0f,2f));
        _canChangeDir = true;
    }
}
