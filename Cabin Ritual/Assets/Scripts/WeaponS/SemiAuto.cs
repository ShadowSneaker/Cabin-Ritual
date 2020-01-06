using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAuto : GunScript
{
    public override void Fire()
    {
        base.Fire();
        if (CanFire())
        {
            ShootBullet();
            StartFireDelay();
        }
    }
}
