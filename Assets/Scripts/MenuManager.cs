using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TMP_Text lastScore;
    [SerializeField] TMP_Text highScore;

    void Start()
    {
        HandleText(lastScore, "Last score");
        HandleText(highScore, "Highscore");
    }

    void HandleText(TMP_Text tmptext, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            tmptext.text = $"{key}: {PlayerPrefs.GetInt(key)}";
        }
        else
        {
            tmptext.text = "";
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
