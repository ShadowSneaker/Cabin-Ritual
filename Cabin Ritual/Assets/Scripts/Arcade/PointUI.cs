﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointUI : MonoBehaviour
{
    //players current points
    public PlayersPoints Points;

    // the text to display points
    public Text PointsText;

    // the text to display kills
    public Text KillsText;

    // a text to display the ingame times
    public Text InGameTime;

    // Start is called before the first frame update
    void Start()
    {
        Points = FindObjectOfType<PlayersPoints>();
    }

    // Update is called once per frame
    void Update()
    {
        PointsText.text = "Points : " + Points.PointsAquired;
        KillsText.text = "Kills : " + Points.KillCount;
        InGameTime.text = "Survival Time : " + Time.fixedTime + "Seconds";

        StaticArcadeInfo.Kills = Points.KillCount;
        StaticArcadeInfo.Score = Points.PointsAquired;
        StaticArcadeInfo.Time = Time.fixedTime;

    }


    public void OpenClosePointUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
