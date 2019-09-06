using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoting : MonoBehaviour
{
    public GameObject Bullet;
    public float Speed = 100f;

    void update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject instBullet = Instantiate(Bullet, transform.position, Quaternion.identity) as GameObject;
            Rigidbody instBulletRigid = instBullet.GetComponent<Rigidbody>();
            instBulletRigid.AddForce(Vector3.forward * Speed);
            
        }
    }
}
