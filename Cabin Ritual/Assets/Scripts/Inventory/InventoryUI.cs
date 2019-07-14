using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    // this function is used to open and close the inventory ui panel 
   public bool Close_OpenUI()
   {
         gameObject.SetActive(!gameObject.activeSelf);
         return gameObject.activeInHierarchy;
   }
}
