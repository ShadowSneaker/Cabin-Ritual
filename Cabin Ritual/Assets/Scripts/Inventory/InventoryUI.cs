using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    // this function is used to open and close the inventory ui panel 
   public void Close_OpenUI()
   {
        gameObject.SetActive(!gameObject.activeSelf);
   }
}
