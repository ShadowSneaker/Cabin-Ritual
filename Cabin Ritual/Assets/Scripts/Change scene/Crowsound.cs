using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowsound : MonoBehaviour
{
    public AudioClip PressSound;

    private bool Playing;

    void Update()
    {
        if(Input.anyKeyDown)
        {
             Playing = true;            
             StartCoroutine(PlayingSound());

                
        }
    }

    IEnumerator PlayingSound()
    {
       
        yield return new WaitForSeconds(120);
        AudioSource.PlayClipAtPoint(PressSound, Vector3.zero);

        Playing = false;
    }
}
