﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAuto : GunScript
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void Shoot()
    {
        base.Shoot();

        if (CanFire())
        {
            Shoot();
            IncreaseSpread();

            Active = false;
            FireDelay(GetFireRate());
        }
    }
}
