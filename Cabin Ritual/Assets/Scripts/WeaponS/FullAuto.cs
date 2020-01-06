using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullAuto : GunScript
{
    public override void Fire()
    {
        base.Fire();
        StartCoroutine(LoopFire());
    }


    private IEnumerator LoopFire()
    {
        while (CanFire())
        {
            ShootBullet();
            IncreaseSpread();
            yield return new WaitForSeconds(GetFireRate());
            Active = true;
        }
    }
}
