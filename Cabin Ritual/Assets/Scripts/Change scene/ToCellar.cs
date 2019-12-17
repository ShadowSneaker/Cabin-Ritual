using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToCellar : MonoBehaviour
{
    
    public void Cellar()
    {

        Controller temp = FindObjectOfType<Controller>();
        if (temp.ReturnLookingAt())
        {
            SceneManager.LoadScene("Area2 - Basement");
        }
    }
}
