using UnityEngine;
using System;
using System.Collections;

public class AdMob_Cars : MonoBehaviour, IEventSubscriber
{
#if UNITY_ANDROID

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        switch (EventName)
        {
            case "gui.screen.game":
                StartCoroutine(IntersitialLoop());
                break;

            case "level.load.menu":
                StopAllCoroutines();
                break;

            case "level.restart":
                StopAllCoroutines();
                break;
        }
    }

    #endregion

    IEnumerator IntersitialLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(150f);
            if (PlayerPrefs.GetInt("inapp.car2") == 0 && PlayerPrefs.GetInt("inapp.car3") == 0)
                RequestInter();
        }
    }

    void RequestInter()
    {
        if (PlayerPrefs.GetInt("inapp.car2") == 0 && PlayerPrefs.GetInt("inapp.car3") == 0)
        {
            AdMobAndroid.requestInterstital("ca-app-pub-9255742339770963/3186411494");
            if (GoogleAnalytics.instance)
                GoogleAnalytics.instance.LogScreen("ad.requestInterstital");
        }
    }
    
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        EventController.SubscribeToAllEvents(this);

        AdMobAndroid.init("pub-9255742339770963");

        // call only interestial banner
        RequestInter();
    }

    void OnDestroy()
    {
        EventController.UnsubscribeToAllEvents(this);
    }
    
    void OnEnable()
    {
        AdMobAndroidManager.interstitialReceivedAdEvent += interstitialReceivedAdEvent;
    }
    
    
    void OnDisable()
    {
        AdMobAndroidManager.interstitialReceivedAdEvent -= interstitialReceivedAdEvent;
    }
    
    void interstitialReceivedAdEvent()
    {
        if (AdMobAndroid.isInterstitalReady())
        {
            AdMobAndroid.displayInterstital();
            if (GoogleAnalytics.instance)
                GoogleAnalytics.instance.LogScreen("ad.showInterstital");
        }
    }
    #endif
}
