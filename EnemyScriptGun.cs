using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptGun : MonoBehaviour
{
    float Speed = 200f;

    void Update()
    {
        transform.position += transform.up * Time.deltaTime * (Speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
