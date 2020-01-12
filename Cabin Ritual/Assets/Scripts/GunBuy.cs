using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBuy : InteractableObject
{
    [Tooltip("The price to purchase this gun.")]
    [SerializeField]
    private int Cost = 500;

    [Tooltip("The price to purchase ammo for this gun (Only used if the user already owns this gun.")]
    [SerializeField]
    private int AmmoCost = 250;

    [Tooltip("The prefab gun type to buy.")]
    [SerializeField]
    private GunScript GunPrefab = null;



    public void OnValidate()
    {
        ScreenText = "Press E to interact. Costs: " + Cost.ToString();
    }


    public override void Interact()
    {
        if (Interactable)
        {
            // Get the closest player (This is used incase we add multiplayer support).
            int Index = 0;
            float PrevDistance = Mathf.Infinity;
            GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < Players.Length; ++i)
            {
                float Distance = Vector3.Distance(transform.position, Players[i].transform.position);
                if (Distance < PrevDistance)
                {
                    Index = i;
                }
                PrevDistance = Distance;
            }

            GunHolder Holder = Players[Index].GetComponent<GunHolder>();
            PlayersPoints Points = Holder.GetComponent<PlayersPoints>();
            if (Holder)
            {
                int GunIndex = Holder.GunExists(GunPrefab);
                if (GunIndex == -1)
                {
                    if (Points.PointsAquired >= Cost)
                    {
                        // Add the gun to the gun holder.
                        Holder.InsertGun(GunPrefab);
                        Points.RemovePoints(Cost);

                        // This should really change to be a check when the player hovers the item but will work for now.
                        // This won't work in multiplayer.
                        ScreenText = "Press E to interact. Costs: " + AmmoCost.ToString();
                    }
                }
                else
                {
                    if (Points.PointsAquired >= AmmoCost)
                    {
                        Debug.Log("Ran");
                        // Refill the gun with ammo.
                        if (Holder.GetHeldWeapon().GetTotalAmmo() + Holder.GetHeldWeapon().GetCurrentAmmo() != Holder.GetHeldWeapon().GetMaximumAmmo())
                        {
                            Holder.ResetIndex(GunIndex);
                            Points.RemovePoints(AmmoCost);
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Object interacting with: " + this + " does not have a GunHolder script attached!");
            }
        }


        // Activate any other bound objects.
        base.Interact();
    }


    public override string GetFlavourText(Controller Player)
    {
        GunHolder Holder = Player.GetComponent<GunHolder>();
        if (Holder)
        {
            if (Holder.GunExists(GunPrefab) > -1)
            {
                ScreenText = "Press E to interact. Costs: " + AmmoCost.ToString();
            }
        }
        return base.GetFlavourText(Player);
    }



    /*
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

    public enum Guntype { pistol, Shotgun, SMG, AssultRifle, Revolver}

    public Guntype BuyableGun;

    bool WeaponOn;

    private GunScript Ammo;


    void Start()
    {
        //object ChildObj = Transform.FindObjectOfType("Weapon Holder");
        Weapon.SetActive(false);

        switch (BuyableGun)
        {
            case (Guntype.pistol):
                {
                    GunMin = 500;
                    GunAmmoCost = 250;
                    break;
                }
            case (Guntype.Shotgun):
                {
                    GunMin = 700;
                    GunAmmoCost = 300;
                    break;
                }
            case (Guntype.SMG):
                {
                    GunMin = 800;
                    GunAmmoCost = 200;
                    break;
                }
            case (Guntype.AssultRifle):
                {
                    GunMin = 700;
                    GunAmmoCost = 300;
                    break;
                }
            case (Guntype.Revolver):
                {
                    GunMin = 1000;
                    GunAmmoCost = 150;
                    break;
                }
        }

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
    }*/




    ///////////////////////////////////////////////
    //beyond this point is byrons gun pick up and ammo buy functions. they are the same as above but made to handle multiple guns
    // these functions cant be tested until the player has multiple guns within the prefab for testing. since they are the same as above then it should work fine 
    // i will test these once two working guns are added to the prefab (i dont want to mess with the prefab just in case)


    //public void GunPickUp()
    //{
    //
    //    
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





