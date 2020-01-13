using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScene : MonoBehaviour
{
    // text to display how long the player survived
    public Text TimeSurvived;

    //the integer to store the time the player survived
    public float TimePlayerSurvived;

    //the text to display how many points the player had
    public Text Points;

    // the text to display how many kills the player had
    public Text Kills;



    // Start is called before the first frame update
    void Start()
    {
        TimePlayerSurvived = StaticArcadeInfo.Time;
        Kills.text = "End Kills : " + StaticArcadeInfo.Kills;
        Points.text = "End Points : " + StaticArcadeInfo.Score;
        TimeSurvived.text = "Time Lasted : " + TimePlayerSurvived;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaodMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
