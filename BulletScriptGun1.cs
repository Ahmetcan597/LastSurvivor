using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScriptGun1 : MonoBehaviour
{
    float Speed = 200f;

    void Update()
    {
        transform.position += transform.up * Time.deltaTime * (Speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
