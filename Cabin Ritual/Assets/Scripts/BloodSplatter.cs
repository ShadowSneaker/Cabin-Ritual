using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodSplatter : MonoBehaviour
{
    public Image Image;
    public float MinAlpha;
    public bool Hit;

    private void Start()
    {
        Image = GetComponent<Image>();
        Image.enabled = false;
    }

    private void Update()
    {
        if ((Hit) && (Image.color.a > 0))
        {
            Image.enabled = true;
            Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, Image.color.a - Time.deltaTime);
        }
        else if (Hit)
        {
            Hit = false;
            Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, 1);
            Image.enabled = false;
        }
    }
}
