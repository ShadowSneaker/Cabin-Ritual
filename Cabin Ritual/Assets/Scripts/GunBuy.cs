using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBuy : MonoBehaviour
{
    // the minumum amount a Gun costs
    public int GunMin = 500;
    public int GunAmmoCost = 250;

    //(byron)
    // the price in which the gun increase my each time one is brought
    public int GunIncrement = 100;


    //For when it is not a door to open (Samuel edit)
    public GameObject WallGun;
    public GameObject Weapon;

    // byron
    // a list of the gameobjects that are the guns used (instead of adding each one as a seperate gameobject)
    //public GameObject[] Guns;
    //
    //i think to solve the problem have a enum here that holds the gun name that can be purchased 
    //public List<GunScript> guntype = new List<GunScript>();
    
    bool WeaponOn;

    private GunScript Ammo;


    void Start()
    {
        //object ChildObj = Transform.FindObjectOfType("Weapon Holder");
        Weapon.SetActive(false);

        // iterates through the array of guns to disable them all at the begining
        //for(int i = 0;  i < Guns.Length; i++)
        //{
        //    guntype[i] = Guns[i].GetComponent<GunScript>();
        //
        //    Guns[i].SetActive(false);
        //}

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




    ///////////////////////////////////////////////
    //beyond this point is byrons gun pick up and ammo buy functions. they are the same as above but made to handle multiple guns
    // these functions cant be tested until the player has multiple guns within the prefab for testing. since they are the same as above then it should work fine 
    // i will test these once two working guns are added to the prefab (i dont want to mess with the prefab just in case)


    //public void GunPickUp()
    //{
    //
    //    switch(guntype)
    //    {
    //        
    //    }
    //
    //    if (WeaponOn == false)
    //    {
    //
    //        Controller temp = FindObjectOfType<Controller>();
    //        PointsSystem TempPoint = FindObjectOfType<PointsSystem>();
    //
    //        if (TempPoint.GetPlayerPointsAquired() < (GunMin))
    //        {
    //            if (temp.ReturnLookingAt())
    //            {
    //                GetComponent<InteractableObject>().ScreenText = "Gun Cost: " + ((GunMin));
    //            }
    //        }
    //        else
    //        {
    //            TempPoint.GetPlayerPoints().RemovePoints((GunMin));
    //            TempPoint.ZombiedoorNumber += 1;
    //
    //            WeaponOn = true;
    //            Weapon.SetActive(true);
    //        }
    //    }
    //
    //    else
    //    {
    //        AmmoBuy();
    //    }
    //}
    //
    //
    //public void AmmoBuy()
    //{
    //    if (WeaponOn == true)
    //    {
    //
    //        Controller temp = FindObjectOfType<Controller>();
    //        PointsSystem TempPoint = FindObjectOfType<PointsSystem>();
    //
    //        if (TempPoint.GetPlayerPointsAquired() < (GunAmmoCost))
    //        {
    //            if (temp.ReturnLookingAt())
    //            {
    //                GetComponent<InteractableObject>().ScreenText = "Gun Ammo Cost: " + ((GunAmmoCost));
    //            }
    //        }
    //        else
    //        {
    //            TempPoint.GetPlayerPoints().RemovePoints((GunAmmoCost));
    //            TempPoint.ZombiedoorNumber += 1;
    //
    //
    //
    //            GetComponent<InteractableObject>().ScreenText = "Ammo brought";
    //
    //
    //
    //            //GetComponent<WeaponSwitching>().enabled = true;
    //
    //            Debug.Log("gun brought");
    //            //Turns the item off when the object brought when not a door (Samuel edit)
    //            //Barrier.gameObject.SetActive(false);
    //            //break;
    //
    //        }
    //
    //    }
    //}

}





