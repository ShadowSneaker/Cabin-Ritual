using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOn : MonoBehaviour
{
    public GameObject Light;
    private bool LightActivation;

    void start()
    {
        LightActivation = false;
    }
    //public void TurnOnLight()
    //{
    //    //Controller temp = FindObjectOfType<Controller>();
    //    //if (temp.ReturnLookingAt())
    //    //{
    //    if (Input.GetKey("E"))
    //    {
    //        Light.SetActive(true);
    //    }
    //    //}
    //}

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //if (Input.GetButtonDown("E"))
            //{
                Debug.Log("running");
                Light.SetActive(true);
            //}
        }
    }
}
