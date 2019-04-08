using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
    public Transform sparksTrans;
    public float lifetime = 90;
    public void Init(string surfaceTag, Vector3 refl)
    {
        //sparksTrans.rotation = Quaternion.LookRotation(Vector3.Reflect(refl, transform.forward));

        Destroy(gameObject, lifetime);
    }
}
