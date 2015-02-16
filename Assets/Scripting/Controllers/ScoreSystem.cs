using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ScoreSystem:MonoBehaviour
{
    public static ScoreSystem Instance { get; private set; }


    public TextMesh ScoreText;

    public float Score { get; private set; }

    private void Start()
    {
        Instance = this;
    }


    private void Update()
    {
        if (PlayerIsOnRoad)
        {
            if (PlayerDoingHighSpeed)
            {

                Score += 3*Time.deltaTime*(PlayerSpeed/70f);
            }
        }
        ScoreText.text = Score.ToString("F0");

    }

    public void CarOvertook()
    {
        if (PlayerIsOnRoad)
        {
            Score += 50;
        }
    }


    public bool PlayerDoingHighSpeed
    {
        get { return PlayerSpeed > 65; }
    }

    private bool PlayerIsOnRoad
    {
        get { return PlayerX > -9.3f && PlayerX < 9.7f; }
    }


    private float PlayerSpeed
    {
        get { return PlayerCarBehaviour.Instance.rigidbody.velocity.z; }
    }


    private float PlayerX
    {
        get { return PlayerCarBehaviour.Instance.transform.position.x; }
    }
}
