using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject ButtonList;
    public void OpenClsoeDropDown()
    { 
        ButtonList.SetActive(!ButtonList.activeSelf);
    }

    public void OpenArcadeMode()
    {
        Debug.Log("Button running");
        SceneManager.LoadScene("Arcade");
    }

}
