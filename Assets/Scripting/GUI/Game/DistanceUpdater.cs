using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class DistanceUpdater : MonoBehaviour, IEventSubscriber
{
    TextMesh _text;

	void Start () 
    {
        _text = GetComponent<TextMesh>();
        EventController.Subscribe("update.gui.distance", this);
	}

    void OnDestroy()
    {
        EventController.Unsubscribe(this);
    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        if (EventName == "update.gui.distance")
        {
            _text.text = string.Format("{0}",((int)Sender.GetComponent<PlayerCarBehaviour>().Distance).ToString());
        }
    }

    #endregion
}
