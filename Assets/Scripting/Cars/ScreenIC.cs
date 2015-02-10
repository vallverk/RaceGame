using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CarDriver))]
public class ScreenIC : MonoBehaviour, IEventSubscriber 
{
    private CarDriver _driver;

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
                break;

            case "input.screen.time.up":
                Time.timeScale = 1.0f;
                break;
        }
    }

    #endregion

    void Update()
    {
        _driver.CurrentWheelsSteer = Input.acceleration.x;
    }

}
