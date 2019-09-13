using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBuy : MonoBehaviour
{
    // the minumum amount a Gun costs
    public int GunMin = 500;
    public int GunAmmoCost = 250;

    //For when it is not a door to open (Samuel edit)
    public GameObject WallGun;
    public GameObject Weapon;

    bool WeaponOn;

    private GunScript Ammo;


    void Start()
    {
        //object ChildObj = Transform.FindObjectOfType("Weapon Holder");
        Weapon.SetActive(false);
    }   

    public void GunPickUp()
    {
        if (WeaponOn == false)
        {

            Controller temp = FindObjectOfType<Controller>();
            PointsSystem TempPoint = FindObjectOfType<PointsSystem>();

            if (TempPoint.GetPlayerPointsAquired() < (GunMin))
            {
                if (temp.ReturnLookingAt())
                {
                    GetComponent<InteractableObject>().ScreenText = "Gun Cost: " + ((GunMin));
                }
            }
            else
            {
                TempPoint.GetPlayerPoints().RemovePoints((GunMin));
                TempPoint.ZombiedoorNumber += 1;

                WeaponOn = true;
                Weapon.SetActive(true);
            }            
        }

        else
        {
            AmmoBuy();
        }
    }

    public void AmmoBuy()
    {
        if (WeaponOn == true)
        {
            
                Controller temp = FindObjectOfType<Controller>();
                PointsSystem TempPoint = FindObjectOfType<PointsSystem>();

                if (TempPoint.GetPlayerPointsAquired() < (GunAmmoCost))
                {
                    if (temp.ReturnLookingAt())
                    {
                        GetComponent<InteractableObject>().ScreenText = "Gun Ammo Cost: " + ((GunAmmoCost));
                    }
                }
                else
                {
                    TempPoint.GetPlayerPoints().RemovePoints((GunAmmoCost));
                    TempPoint.ZombiedoorNumber += 1;



                    GetComponent<InteractableObject>().ScreenText = "Ammo brought";



                    //GetComponent<WeaponSwitching>().enabled = true;

                    Debug.Log("gun brought");
                    //Turns the item off when the object brought when not a door (Samuel edit)
                    //Barrier.gameObject.SetActive(false);
                    //break;

                }
            
        }
    }
}


//(DoorType.ArcadeDoor):
//                {
//                    Controller temp = FindObjectOfType<Controller>();
//PointsSystem TempPoint = FindObjectOfType<PointsSystem>();
//
//                    if(locked)
//                    {
//
//                        if(TempPoint.GetPlayerPointsAquired() < (ZombieDoorMin + (TempPoint.ZombiedoorNumber* 100)))
//                        {
//                            if (temp.ReturnLookingAt())
//                            {
//                                GetComponent<InteractableObject>().ScreenText = "the Door Costs : " + ((ZombieDoorMin + (TempPoint.ZombiedoorNumber* 100)).ToString());
//                            }
//                        }
//                        else
//                        {
//                            TempPoint.GetPlayerPoints().RemovePoints((ZombieDoorMin + (TempPoint.ZombiedoorNumber* 100)));
//                            TempPoint.ZombiedoorNumber += 1;
//
//                            GetComponent<InteractableObject>().ScreenText = "Door unlocked";
//
//                            if(barrier)
//                            {
//                                //Turns the item off when the object brought when not a door (Samuel edit)
//                                Barrier.gameObject.SetActive(false);
//                                break;
//                            }
//                            else
//                            {
//                                switch (doorState)
//                                {
//                                    case (DoorState.open):
//                                        {
//                                            GetComponent<Animation>().Play("close");
//                                    doorState = DoorState.closed;
//                                            break;
//                                        }
//                                    case (DoorState.closed):
//                                        {
//                                            GetComponent<Animation>().Play("open");
//                                    doorState = DoorState.open;
//                                            break;
//                                        }
//                                }
//                            }
//                            
//
//                            
//
//                            
//
//
//                            locked = false;
//                        }              
//
//
//                        
//
//
//
//                        break;
//                    }
//                    else
//                    {
//                        GetComponent<Animator>().Play("open");
//                        doorState = DoorState.open;
//                        break;
//                    }
//
//                    
//




                   
              
