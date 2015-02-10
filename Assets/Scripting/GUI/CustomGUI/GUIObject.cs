using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GUIObject : MonoBehaviour, IEventSubscriber
{
    protected virtual void AwakeProc() {}
    protected virtual void EventProc(string EventName, GameObject Sender) {}

    public string ShowOnEvent = "";

    void Awake()
    {
        AwakeProc();
        EventController.SubscribeToAllEvents(this);
    }

    public void OnEvent(string EventName, GameObject Sender)
    {
        EventProc(EventName, Sender);
        if (EventName == "gui.hide")
        {
            if (gameObject != null)
            {
                if (renderer)
                    renderer.enabled = false;
                if (collider)
                    collider.enabled = false;
            }
        } else if (EventName == ShowOnEvent)
        {
            if (renderer)
                renderer.enabled = true;
            if (collider)
                collider.enabled = true;
        }
    }

    protected virtual void OnDestroy()
    {
        EventController.UnsubscribeToAllEvents(this);
    }
}
