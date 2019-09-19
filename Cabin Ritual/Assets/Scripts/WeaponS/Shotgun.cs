using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunScript
{
    [Tooltip("Should the pellet index be randomized.")]
    [SerializeField]
    private bool RandomPelletes;

    [Tooltip("How many pellets should be fired per shot.")]
    [SerializeField]
    private int PelletCount = 5;

    private int Index = 0;

    private bool FirstShot = false;

    [Tooltip("Should each bullet follow the projectile index.")]
    [SerializeField]
    private bool UseLoopIndex = false;

    [Tooltip("Should this shotgun be fully automatic.")]
    [SerializeField]
    private bool FullAuto = false;

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

        if (FullAuto)
        {
            while (CanFire())
            {
                Fire();

                new WaitForSeconds(GetFireRate());
            }
        }
        else
        {
            if (CanFire())
            {
                Active = false;
                Fire();                
            }
        }
    }


    private void Fire()
    {
        //Index = GetProjectileIndex();
        for (int i = 0; i < PelletCount; ++i)
        {
            ShootBullet(0, FirstShot);
            FirstShot = false;
        }

        FirstShot = true;
        Active = true;

        IncreaseSpread();
    }
}
