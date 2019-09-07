using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    // with this you can pick the door type in the inspector and the event will change depending on it
    public enum DoorType { StandardDoor, CabinDoorLocked, StandardLockedDoor, GardenShedDoor, FullLock, ArcadeDoor};

    // the state in which the door is in
    public enum DoorState { open, closed };
    
    [Tooltip("the type of door this is")]
    public DoorType Door;

    [Tooltip("what state is the door in")]
    public DoorState doorState;

    [Tooltip("does the door start locked")]
    public bool locked;

    [Tooltip("what item is required for this door to be opened")]
    public Item ItemRequired;

    // the minumum amount a door costs
    public int ZombieDoorMin = 500;

    //For when it is not a door to open (Samuel edit)
    public GameObject Barrier;


    void Start()
    {
        Barrier.gameObject.SetActive(true);
    }

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
                    switch(doorState)
                    {
                        case (DoorState.open):
                            {
                                GetComponent<Animation>().Play("close");
                                doorState = DoorState.closed;
                                break;
                            }
                        case (DoorState.closed):
                            {
                                GetComponent<Animation>().Play("open");
                                doorState = DoorState.open;
                                break;
                            }
                    }
                    
                    
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
                                locked = true;
                                GetComponent<InteractableObject>().ScreenText = "the Key has broken";
                                temp.GetPlayerInv().RemoveItem(ItemRequired);
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
            case (DoorType.StandardLockedDoor):
                {
                    if(locked)
                    {
                        Controller temp = FindObjectOfType<Controller>();
                        if(temp.ReturnLookingAt())
                        {
                            if(temp.GetPlayerInv().EquipedItem == ItemRequired)
                            {
                                locked = false;
                                GetComponent<InteractableObject>().ScreenText = "Unlocked";
                                temp.GetPlayerInv().RemoveItem(ItemRequired);
                            }
                            else
                            {
                                GetComponent<InteractableObject>().ScreenText = "locked needs " + ItemRequired.name;
                            }
                        }
                    }
                    else if (!locked)
                    {
                        switch (doorState)
                        {
                            case (DoorState.open):
                                {
                                    GetComponent<Animation>().Play("close");
                                    doorState = DoorState.closed;
                                    break;
                                }
                            case (DoorState.closed):
                                {
                                    GetComponent<Animation>().Play("open");
                                    doorState = DoorState.open;
                                    break;
                                }
                        }
                    }

                    break;
                }
            case (DoorType.GardenShedDoor):
                {
                    if (locked)
                    {
                        Controller temp = FindObjectOfType<Controller>();
                        if (temp.ReturnLookingAt())
                        {
                            if (temp.GetPlayerInv().EquipedItem == ItemRequired)
                            {
                                locked = false;
                                GetComponent<InteractableObject>().ScreenText = "the vines have been cut off";
                                temp.GetPlayerInv().RemoveItem(ItemRequired);
                            }
                            else
                            {
                                GetComponent<InteractableObject>().ScreenText = "the door is covered in vines and needs a " + ItemRequired.name;
                            }
                        }
                    }
                    else if (!locked)
                    {
                        switch (doorState)
                        {
                            case (DoorState.open):
                                {
                                    GetComponent<Animation>().Play("close");
                                    doorState = DoorState.closed;
                                    break;
                                }
                            case (DoorState.closed):
                                {
                                    GetComponent<Animation>().Play("open");
                                    doorState = DoorState.open;
                                    break;
                                }
                        }
                    }
                    break;
                }
            case (DoorType.FullLock):
                {
                    Controller temp = FindObjectOfType<Controller>();
                    if (temp.ReturnLookingAt())
                    {
                        GetComponent<InteractableObject>().ScreenText = "the door wont budge!!";
                    }
                        break;
                }
            case (DoorType.ArcadeDoor):
                {
                    Controller temp = FindObjectOfType<Controller>();
                    PointsSystem TempPoint = FindObjectOfType<PointsSystem>();

                    if(locked)
                    {

                        if(TempPoint.GetPlayerPointsAquired() < (ZombieDoorMin + (TempPoint.ZombiedoorNumber * 100)))
                        {
                            if (temp.ReturnLookingAt())
                            {
                                GetComponent<InteractableObject>().ScreenText = "the Door Costs : " + ((ZombieDoorMin + (TempPoint.ZombiedoorNumber * 100)).ToString());
                            }
                        }
                        else
                        {
                            TempPoint.GetPlayerPoints().RemovePoints((ZombieDoorMin + (TempPoint.ZombiedoorNumber * 100)));
                            TempPoint.ZombiedoorNumber += 1;

                            GetComponent<InteractableObject>().ScreenText = "Door unlocked";


                            //Turns the item off when the object brought when not a door (Samuel edit)
                            Barrier.gameObject.SetActive(false);

                            switch (doorState)
                            {
                                case (DoorState.open):
                                    {
                                        GetComponent<Animation>().Play("close");
                                        doorState = DoorState.closed;
                                        break;
                                    }
                                case (DoorState.closed):
                                    {
                                        GetComponent<Animation>().Play("open");
                                        doorState = DoorState.open;
                                        break;
                                    }
                            }

                            


                            locked = false;
                        }              


                        



                        break;
                    }
                    else
                    {
                        GetComponent<Animator>().Play("open");
                        doorState = DoorState.open;
                        break;
                    }

                    





                   
                }
        }
    }

}
