using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBoss : MonoBehaviour
{
    public int Health = 1000;
    

    private bool dead = false;
    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        if (Health <= 0)
        {
            DieBoss();
        }
    }

    void DieBoss()
    {
        if (dead == false)
        {
            ScoreManager.Instance.BossDeath();
            ScoreManager.Instance.AddPoint(50);
            //Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject);
        }
        dead = true;
        
    }
 
}
