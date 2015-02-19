using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour, IEventSubscriber
{
	private bool _deathScreen;
	private bool paused = false;

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

                if (!_deathScreen)
                {
                    Time.timeScale = 0;
					Time.fixedDeltaTime = 0.02f * Time.timeScale;
					paused = true;
                }
                GoogleAnalytics.Log("screen.main.pause");
                break;

            case "gui.screen.game.show":
            //    Time.timeScale = 1;
				paused = false;
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
                               _deathScreen = true;
                Time.timeScale = 0.2f;

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

	void Update()
	{
		if (paused == false && Time.timeScale < 1)
		{
			Time.timeScale += 0.002f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
		}

		if (Time.timeScale > 1)
		{
			paused = true;
			Time.timeScale = 1;
		}

	}

    #endregion
}
