using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenuHighScore : MonoBehaviour {
    public Text highScoreText;

    void Start() {
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore");
    }

}
