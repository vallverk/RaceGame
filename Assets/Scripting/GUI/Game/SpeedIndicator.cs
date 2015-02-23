using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedIndicator : MonoBehaviour {

	Image myImage;
	float playerMaxSpeed;
	GameObject playerCar;

	void Start () 
	{
		myImage = GetComponent<Image>();
		playerMaxSpeed = GameObject.FindWithTag("PlayerCar").GetComponent<CarDriver>().MaxSpeed;
		playerCar = GameObject.FindWithTag("PlayerCar");
	}
	
	void Update()
	{
		myImage.fillAmount = playerCar.rigidbody.velocity.z / playerMaxSpeed;
	}
}
