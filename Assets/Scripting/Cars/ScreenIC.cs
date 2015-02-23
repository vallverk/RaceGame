using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CarDriver))]
public class ScreenIC : MonoBehaviour, IEventSubscriber 
{
	private bool IsAccelControl = true;
	private CarDriver _driver;
	private bool IsSteerLeft = false;
	private bool IsSteerRight = false;
	private bool IsAutoAccel = false;
	private float _steer;

    void Awake()
    {
        EventController.SubscribeToAllEvents(this);
        _driver = GetComponent<CarDriver>();
    }

    void OnDestroy()
    {
        EventController.UnsubscribeToAllEvents(this);
    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        switch (EventName)
        {
            case "input.screen.acceleration.down":
                _driver.CurrentAcceleration = 1;
                break;

            case "input.screen.acceleration.up":
                _driver.CurrentAcceleration = 0;
                break;

            case "input.screen.breaking.down":
                _driver.CurrentAcceleration = -1;
                break;
                
            case "input.screen.breaking.up":
                _driver.CurrentAcceleration = 0;
                break;

            case "input.screen.time.down":
                Time.timeScale = 0.5f;
				Time.fixedDeltaTime = 0.02f * Time.timeScale;
                break;

            case "input.screen.time.up":
                Time.timeScale = 1.0f;
                break;
			
		case "input.button.left.pressed":
			IsSteerLeft = true;
			break;
			
		case "input.button.left.released":
			IsSteerLeft = false;
			break;
			
		case "input.button.right.pressed":
			IsSteerRight = true;
			break;
			
		case "input.button.right.released":
			IsSteerRight = false;
			break;

		case "input.button.nitro.pressed":
			_driver.Nitro = true;
			_driver.NitroPoints--;
			break;

		case "input.button.nitro.released":
			_driver.Nitro = false;
			break;

		}
    }

    #endregion

    void Update()
    {

        float steer = ConstantsStorage.I.ControlSensetivity.Evaluate(Input.acceleration.x);

        if (IsAccelControl)
			_driver.CurrentWheelsSteer = Mathf.Clamp(Input.acceleration.x * steer, -1, 1);
		else
		{

		if (IsSteerLeft && !IsSteerRight)
		{
			if (_steer == 0)
				_steer = -0.1f;
			else
				_steer -= (1 - Mathf.Abs(_steer));
		}
		else if (!IsSteerLeft && IsSteerRight)
		{
			if (_steer == 0)
				_steer = 0.1f;
			else
				_steer += (1 - Mathf.Abs(_steer));
		}
		else
		{
			if (_steer > -0.1f && _steer < 0.1f)
				_steer = 0;
			else if (_steer < 0)
				_steer += (1 - Mathf.Abs(_steer));
			else if (_steer > 0)
				_steer -= (1 - Mathf.Abs(_steer));
		}

		}

		if (IsAutoAccel)
		{
			_driver.CurrentAcceleration = 1;
		}
	}

}
