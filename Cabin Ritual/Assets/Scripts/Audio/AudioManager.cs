using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;

            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;

            s.source.loop = s.loop;
        }
    }

    // takes a string for the name of the sound you want playing
    public void Play(string name)
    {
        // in the sound array find the sound that has the same name as the name parameter
        Sound S = Array.Find(sounds, sound => sound.Name == name);

        // if the sound has been miss spelt then it wont play the sound (stops a null refernce error)
        if (S == null)
        {
            Debug.LogWarning("Sound: " + name + "not found (possibly misspelt");
            return;
        }
            

        S.source.Play();
    }
}
