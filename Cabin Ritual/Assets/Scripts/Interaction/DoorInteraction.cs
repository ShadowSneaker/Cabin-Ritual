using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    // with this you can pick the door type in the inspector and the event will change depending on it
    public enum DoorType { StandardDoor, CabinDoorLocked };

    //the actual enum for the person to edit
    public DoorType Door;

    //bool to determine if door is locked
    public bool locked;

    // if the door is locked what item is required to unlock it
    public Item ItemRequired;





    // the function that determines the event that will happen depening on the type of door it is
    public void DoorEvent()
    {
        //this function will have a switch in which will determine the event depending on the type of door chosen
        switch(Door)
        {
            case (DoorType.StandardDoor):
                {
                    // this is where the animation for the door to open will happen for now a debug log will be displayed
                    Debug.Log("standard door has been activated");
                    break;
                }
            case (DoorType.CabinDoorLocked):
                {
                    if(locked)
                    {
                        Controller temp = FindObjectOfType<Controller>();
                        if(temp.ReturnLookingAt())
                        {
                           if( temp.GetPlayerInv().EquipedItem == ItemRequired)
                           {
                                locked = false;
                                GetComponent<InteractableObject>().ScreenText = "Door has been unlocked";
                           }
                           else
                           {
                                // this function will change the flavour text of the object to say its locked and may need a key. 
                                GetComponent<InteractableObject>().ScreenText = "This Door is locked and may need a key";
                           }
                        }
                        
                    }
                    else if(!locked)
                    {
                        // this is where the animation for the door to open will happen for now a debug log will be displayed
                        Debug.Log("Cabin door unlocked");
                    }




                    break;
                }
        }
    }

}
