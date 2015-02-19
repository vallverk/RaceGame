using UnityEngine;
using System.Collections;

public class RealTimeReflection : MonoBehaviour
{
		public Material material1;
		public Material material2;
		public float duration = 2.0F;

		private GameObject pCar;

		void Start() 
		{
			renderer.material = material1;

			pCar = GameObject.FindGameObjectWithTag("PlayerCar");
		}
		void Update() 
		{

		if (pCar.GetComponent<Rigidbody>().velocity.z > 100)
				duration = 10f;
		else if (pCar.GetComponent<Rigidbody>().velocity.z > 20)
			duration = (100-pCar.GetComponent<Rigidbody>().velocity.z)/3;
		else 
			duration = 100f;

			float lerp = Mathf.PingPong(Time.time, duration) / duration;
			renderer.material.Lerp(material1, material2, lerp);
		}

}

