using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstFire : GunScript
{
    [Header("Burst Fire Gun")]


    [Tooltip("The amount of bullets this gun fires per shot.")]
    [SerializeField]
    private int BurstCount = 3;

    [Tooltip("Should each bullet in the burst be linked to the projectile index (so the first bullet shot will use ProjectileIndex 1, the second bullet will use ProjectileIndex2 and so on).")]
    [SerializeField]
    private bool IndexFollowBurst = false;

    [Tooltip("The time between each bullet shot (In seconds).")]
    [SerializeField]
    private float BurstDelay = 0.1f;



    public override void Fire()
    {
        base.Fire();
        if (CanFire())
        {
            StartCoroutine(StartBurst());
        }
    }


    private IEnumerator StartBurst()
    {
        for (int i = 0; i < BurstCount; ++i)
        {
            ShootBullet((IndexFollowBurst) ? i : -1);
            IncreaseSpread();
            yield return new WaitForSeconds(BurstDelay);
        }

        StartFireDelay();
    }
}
