using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodSplatter : MonoBehaviour
{
    public Image Image;
    public bool Hit;

    private void Start()
    {
        Image.enabled = false;
    }

    private void Update()
    {
        if (Hit)
        {
            Image.enabled = true;
            //Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, Image.color.a - Time.deltaTime);
        }
        else
        {
            Image.enabled = false;
        }
    }
}
