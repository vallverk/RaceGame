using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour, IEventSubscriber
{
    void Awake()
    {
        EventController.SubscribeToAllEvents(this);
    }

    void Start()
    {
        EventController.PostEvent("gui.hide", null);
        EventController.PostEvent("gui.screen.game.show", null);
        GoogleAnalytics.Log("screen.game.start");
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
            case "gui.screen.pause":
                Time.timeScale = 0;
                GoogleAnalytics.Log("screen.main.pause");
                break;

            case "gui.screen.game.show":
                Time.timeScale = 1;
                break;

            case "level.restart":
                Time.timeScale = 1;
                GoogleAnalytics.Log("screen.main.restart");
                Application.LoadLevel(Application.loadedLevel);
                break;

            case "level.load.menu":
                Time.timeScale = 1;
                GoogleAnalytics.Log("screen.main.loadMenu");
                Application.LoadLevel("Menu");
                break;

            case "car.player.death":
                int cs = (int)Sender.GetComponent<PlayerCarBehaviour>().Distance;
                int os = PlayerPrefs.HasKey("Score")?PlayerPrefs.GetInt("Score"):0;
                PlayerPrefs.SetInt("Score",Mathf.Max(cs,os));
                GoogleAnalytics.Log("screen.main.playerDeath");
                EventController.PostEvent("gui.hide",null);
                EventController.PostEvent("gui.screen.findistance",null);
                EventController.PostEvent("gui.screen.pause",null);
                break;
        }
    }

    #endregion
}
