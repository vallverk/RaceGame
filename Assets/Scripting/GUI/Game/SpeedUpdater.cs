using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class SpeedUpdater : MonoBehaviour, IEventSubscriber
{
	TextMesh _text;
	
	void Start () 
	{
		_text = GetComponent<TextMesh>();
		EventController.Subscribe("update.gui.speed", this);
	}
	
	void OnDestroy()
	{
		EventController.Unsubscribe(this);
	}
	
	#region IEventSubscriber implementation
	
	public void OnEvent(string EventName, GameObject Sender)
	{
		if (EventName == "update.gui.speed")
		{
			_text.text = string.Format("{0}",((int)Sender.GetComponent<CarDriver>()._rigidbody.velocity.z).ToString());
		}
	}
	
	#endregion
}