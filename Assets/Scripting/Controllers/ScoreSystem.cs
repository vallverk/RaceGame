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
        if (PlayerDoingHighSpeed)
        {

            Score += 3 * Time.deltaTime * (PlayerSpeed/70f);
        }

        ScoreText.text = Score.ToString("F0");
    }

    public void CarOvertook()
    {
        Score += 50;
    }


    public bool PlayerDoingHighSpeed
    {
        get { return PlayerSpeed > 65; }
    }


    private float PlayerSpeed
    {
        get { return PlayerCarBehaviour.Instance.rigidbody.velocity.z; }
    }

}
