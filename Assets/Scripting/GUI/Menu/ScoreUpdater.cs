using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class ScoreUpdater : MonoBehaviour 
{
    void Start()
    {
        if (PlayerPrefs.HasKey("Score"))
            GetComponent<TextMesh>().text = string.Format("BEST SCORE: {0}", PlayerPrefs.GetInt("Score"));
        else
            GetComponent<TextMesh>().text = string.Format("BEST SCORE: {0}", 0);
    }
}
