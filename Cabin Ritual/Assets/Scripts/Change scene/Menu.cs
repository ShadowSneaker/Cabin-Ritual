using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject Loading;

   void Update()
   {
        Loading.SetActive(false);
        NextScene();        
   }

    void NextScene()
    {
        if (Input.GetKeyDown("space"))
        {            
            Debug.Log("Pressed");

            Loading.SetActive(true);
           
            SceneManager.LoadScene("MainMenu");
        }
    }    
}
