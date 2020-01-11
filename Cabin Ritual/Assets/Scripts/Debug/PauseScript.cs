using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PauseScript
{
    public static bool GamePaused = false;

    public static void Pause()
    {
        GamePaused = true;
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public static void Unpause()
    {
        GamePaused = false;
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public static void TogglePause()
    {
        if (GamePaused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }
}
