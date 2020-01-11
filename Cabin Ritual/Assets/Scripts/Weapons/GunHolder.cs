using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHolder : MonoBehaviour
{
    [Tooltip("A list of weapons this object has.")]
    [SerializeField]
    List<GunScript> Weapons = new List<GunScript>();

    [Tooltip("The current weapon selected (represented by its index in the weapon list).")]
    [SerializeField]
    int CurrentWeapon;

    [Tooltip("A reference to the hand that holds the weapon.")]
    [SerializeField]
    private Transform Hand = null;


    public void Start()
    {
        bool First = true;
        for (int i = 0; i < Weapons.Count; ++i)
        {
            if (Weapons[i])
            {
                Weapons[i] = Instantiate<GunScript>(Weapons[i], Hand);
                Weapons[i].SetOwner(this);

                if (First)
                {
                    First = false;
                    Weapons[i].gameObject.SetActive(true);
                }
                else
                {
                    Weapons[i].gameObject.SetActive(false);
                }
            }
        }
    }


    // Swaps the current weapon with the inputted index.
    public void SwapTo(int Index)
    {
        Index = Mathf.Clamp(Index, 0, Weapons.Count);
        if (Weapons[Index])
        {

            // Play "put away" animation for the current eqquiped weapon.
            //Weapons[CurrentWeapon].enabled = false;
            Weapons[CurrentWeapon].gameObject.SetActive(false);

            CurrentWeapon = Index;
            Weapons[CurrentWeapon].gameObject.SetActive(true);
            // Play "take out" animation for the new weapon.
        }
    }


    // Goes up through the weapon list and selects the next weapon in the list.
    // If there is no next weapon in the list, it will select the first index.
    public void SwapUp()
    {
        SwapTo((CurrentWeapon + 1 > Weapons.Count - 1) ? 0 : CurrentWeapon + 1);
    }


    // Goes down through the weapon list and selects the previous weapon in the list.
    // If there is no previous weapon in the list, it will select the last index.
    public void SwapDown()
    {
        SwapTo((CurrentWeapon - 1 < 0) ? Weapons.Count - 1 : CurrentWeapon - 1);
    }


    public GunScript GetHeldWeapon()
    {
        return Weapons[CurrentWeapon];
    }


    public GunScript GetWeapon(int Index)
    {
        return Weapons[Index];
    }


    // Checks to see if an inputted gun exists in this gun holder.
    // @param Gun - The gun prefab to check.
    // @return - Returns the index of the gun. Returns -1 if the gun does not exist.
    public int GunExists(GunScript Gun)
    {
        for (int i = 0; i < Weapons.Count; ++i)
        {
            if (Weapons[i])
            {
                if (Weapons[i].name.Contains(Gun.name))
                {
                    return i;
                }
            }
        }
        return -1;
    }


    // Resets the ammo of a weapon based on the index.
    // @param Index - The weapon index to reset.
    public void ResetIndex(int Index)
    {
        Weapons[Index].Reset();
    }


    // Adds a specified gun into this gun holder.
    // @note - Automatically swaps to the new gun.
    // @param Gun - The gun prefab to spawn.
    // @param IncreaseHolderSize - Should the Weapons array increase if it is already full.
    // @return - The index that this gun was placed in.
    public int InsertGun(GunScript Gun, bool IncreaseHolderSize = false)
    {
        // Check for empty slots
        // If there are empty spots create the gun there.
        // If not then create the gun and replace the currently selected gun with the created gun.

        GunScript GunInstance = Instantiate<GunScript>(Gun, Hand);
        int Empty;
        if (HasEmptySlot(out Empty))
        {
            Weapons[Empty] = GunInstance;
            SwapTo(Empty);
            return Empty;
        }
        else
        {
            if (IncreaseHolderSize)
            {
                Weapons.Add(GunInstance);
                return Weapons.Count - 1;
            }
            else
            {
                Weapons[CurrentWeapon] = GunInstance;
                return CurrentWeapon;
            }
        }
    }


    public bool HasEmptySlot(out int Slot)
    {
        for (int i = 0; i < Weapons.Count; ++i)
        {
            if (!Weapons[i])
            {
                Slot = i;
                return true;
            }
        }
        Slot = -1;
        return false;
    }
}
