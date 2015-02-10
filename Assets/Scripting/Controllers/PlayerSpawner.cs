using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour 
{
    public int CarIndex = 0;
    public int ColorIndex = 0;

    public GameObject[] CarPrefabs;

    public GroundController Ground;
    public CameraController Camera;

    void Awake()
    {
        if (PlayerPrefs.HasKey("CarModel"))
            CarIndex = PlayerPrefs.GetInt("CarModel");

        if (PlayerPrefs.HasKey("CarColor"))
            ColorIndex = PlayerPrefs.GetInt("CarColor");

        GameObject car = GameObject.Instantiate(CarPrefabs [CarIndex], Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
        Ground.CharCar = car.transform;
        Camera.Target = car.transform;
        ColorPicker col = car.GetComponent<ColorPicker>();
        col.CarColor = (ColorPicker.Colors)ColorIndex;
        col.UpdateColor();
#if !UNITY_EDITOR
        car.GetComponent<ScreenIC>().enabled = true;
#endif
    }
}
