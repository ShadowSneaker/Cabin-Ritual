using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveInteraction : MonoBehaviour
{
    [Tooltip("the item required for the graves specific interaction")]
    public Item ItemRequired;


    public void GraveEvent()
    {
        Controller temp = FindObjectOfType<Controller>();

        if (temp.ReturnLookingAt())
        {
            if (temp.GetPlayerInv().EquipedItem == ItemRequired)
            {

                GetComponent<InteractableObject>().ScreenText = "sucess";
                Instantiate(ItemRequired.ItemDropped, new Vector3(ItemRequired.DropXPos, ItemRequired.DropYPos, ItemRequired.DropZpos), Quaternion.identity);
                temp.GetPlayerInv().RemoveItem(ItemRequired);
            }
            else
            {
                // this function will change the flavour text of the object to say its locked and may need a key. 
                GetComponent<InteractableObject>().ScreenText = "i may need a shovel here";
            }
        }
    }
}
