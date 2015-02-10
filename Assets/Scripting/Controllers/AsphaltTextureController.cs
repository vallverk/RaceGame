using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AsphaltTextureController : MonoBehaviour 
{
	public void UpdateTex()
	{
        renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(0,-transform.position.z/10.0f));
	}
}
