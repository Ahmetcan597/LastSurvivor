using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float enemyHealth = 80;
    public ParticleSystem enemyBlowUp;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        KillEnemy();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Bullet":
                enemyHealth -= 10;
                enemyBlowUp.Play();
                break;
        }
    }

    void KillEnemy()
    {
        if(enemyHealth <= 0)
        {
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score", 0) + 1);
            Destroy(transform.parent.gameObject);
        }
    }
}
