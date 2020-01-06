using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunScript
{
    [Header("Shotgun")]


    [Tooltip("Should this shotgun be fully automatic.")]
    [SerializeField]
    private bool FullAuto = false;

    [Tooltip("The amount of projectiles this shotgun fires per shot.")]
    [SerializeField]
    private int PelletCount = 5;

    [Tooltip("Should the projectile index follow the pellet count.")]
    [SerializeField]
    private bool UseLoopIndex = false;



    public override void Fire()
    {
        base.Fire();
        if (FullAuto)
        {
            StartCoroutine(StartFullAuto());
        }
        else
        {
            if (CanFire())
            {
                FireShotgun();
                StartFireDelay();
            }
        }
    }


    protected void FireShotgun()
    {
        bool FirstShot = true;
        for (int i = 0; i < PelletCount; ++i)
        {
            ShootBullet((UseLoopIndex) ? i : -1, FirstShot);
            FirstShot = false;
        }

        IncreaseSpread();
    }


    private IEnumerator StartFullAuto()
    {
        while (CanFire())
        {
            FireShotgun();
            yield return new WaitForSeconds(GetFireRate());
            Active = true;
        }
    }
}
