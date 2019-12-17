using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    


   void Update()
   {        
       
            NextScene();
        
   }

    void NextScene()
    {
        if (Input.GetKeyDown("space"))
        {            
            Debug.Log("Pressed");
           
            SceneManager.LoadScene("MainMenu");
        }
    }    
}
