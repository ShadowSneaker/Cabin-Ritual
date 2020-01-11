using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ArcadeUI : MonoBehaviour
{
    public Text PointsDisplay;
    public PlayersPoints Player;


    private void Start()
    {
        PointsDisplay.text = "Points : 0";
    }


    public void Update()
    {
        PointsDisplay.text = "Points : " + Player.PointsAquired;
    }


    public void OpenCloseUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
