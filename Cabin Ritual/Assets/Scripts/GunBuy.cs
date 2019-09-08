using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBuy : MonoBehaviour
{
    // the minumum amount a Gun costs
    public int GunMin = 500;

    //For when it is not a door to open (Samuel edit)
    public GameObject WallGun;
    //public GameObject WeaponHolder;


    void Start()
    {
        //object ChildObj = Transform.FindObjectOfType("Weapon Holder");
    }

    // Update is called once per frame
    void Update()
    {
        GunPickUp();
    }

    public void GunPickUp()
    {
        Controller temp = FindObjectOfType<Controller>();
        PointsSystem TempPoint = FindObjectOfType<PointsSystem>();

        if (TempPoint.GetPlayerPointsAquired() < (GunMin + (TempPoint.ZombiedoorNumber * 100)))
        {
            if (temp.ReturnLookingAt())
            {
                GetComponent<InteractableObject>().ScreenText = "Gun Cost: " + ((GunMin + (TempPoint.ZombiedoorNumber * 100)).ToString());
            }
        }
        else
        {
            TempPoint.GetPlayerPoints().RemovePoints((GunMin + (TempPoint.ZombiedoorNumber * 100)));
            TempPoint.ZombiedoorNumber += 1;

            GetComponent<InteractableObject>().ScreenText = "Gun Aquired";            

            //WeaponHolder.Find("Weapon Holder").GetComponent<WeaponSwitching>().enabled = true;

            Debug.Log("gun brought");
            //Turns the item off when the object brought when not a door (Samuel edit)
            //Barrier.gameObject.SetActive(false);
            //break;

        }
    }
}
