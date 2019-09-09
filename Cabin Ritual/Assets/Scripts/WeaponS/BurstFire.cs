using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstFire : GunScript
{
    [Tooltip("The amount of bullets that is shot.")]
    [SerializeField]
    private int BurstCount = 3;

    [Tooltip("How long the gun should wait before allowing to fire again.")]
    [SerializeField]
    private float BurstDelay = 0.1f;


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
            Active = false;

            for (int i = 0; i < BurstCount; ++i)
            {
                ShootBullet();

                new WaitForSeconds(GetFireRate());
            }
        }

        IncreaseSpread();
        FireDelay(BurstDelay);
    }
}
