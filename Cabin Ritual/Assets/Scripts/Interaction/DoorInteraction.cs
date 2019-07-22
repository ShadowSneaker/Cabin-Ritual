using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : ActivatableObject
{
    // with this you can pick the door type in the inspector and the event will change depending on it
    public enum DoorType { StandardDoor, LockedDoor };

    //the actual enum for the person to edit
    public DoorType Door;

    //bool to determine if door is locked
    public bool locked;

    // if the door is locked what item is required to unlock it
    public string ItemRequired;

    // the function that determines the event that will happen depening on the type of door it is
    public void DoorEvent()
    {
        //this function will have a switch in which will determine the event depending on the type of door chosen
    }

}
