using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour, IEventSubscriber
{
	private bool _deathScreen;
	private int pausedCounter;
	private TextMesh counter;
	private bool FirstStart = true;

    void Awake()
    {
        EventController.SubscribeToAllEvents(this);

		counter = GameObject.Find("Text_Counter").GetComponent<TextMesh>();
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
					pausedCounter = -1;
                }
                GoogleAnalytics.Log("screen.main.pause");
                break;

            case "gui.screen.game.show":
				if (FirstStart)
				{
					FirstStart = false;
					pausedCounter = 1;
				}
				else
					pausedCounter = 200;
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
				Time.fixedDeltaTime = 0.02f * Time.timeScale;

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
		if (pausedCounter > 0)
		{
			pausedCounter--;
			counter.text = "" + pausedCounter/60;
			counter.color = new Color(1,1,1,1);
		}
		else if (pausedCounter != -1)
		{
			Time.timeScale = 1;
			counter.color = new Color(1,1,1,0);
		}

	}

    #endregion
}
