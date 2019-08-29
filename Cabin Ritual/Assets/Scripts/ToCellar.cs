using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToCellar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Cellar()
    {

        Controller temp = FindObjectOfType<Controller>();
        if (temp.ReturnLookingAt())
        {
            SceneManager.LoadScene("Area2 - Basement");
        }
    }
}
