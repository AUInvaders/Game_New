using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class EnemyLaser1 : MonoBehaviour
{
    public FireRateBoss _fireRateBoss;
    public FireBoss _fireBoss;
    public EnemyLaser1()
    {
        _fireRateBoss = new FireRateBoss();
        _fireBoss = new FireBoss(this);
    }

    public Transform FirePoint;
    public GameObject EnemyBulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    { 
        Shoot();
    }

    // Update is called once per frame
    void Shoot()
    {
        if (_fireRateBoss.canShoot1 == true)
        {            
            _fireRateBoss.canShoot1 = false;
            StartCoroutine(_fireBoss.ShootDelayBoss());

            Instantiate(EnemyBulletPrefab, FirePoint.position, FirePoint.rotation);
        }
    }
}

public class FireBoss
{
    private EnemyLaser1 _enemyLaser1;
    public FireBoss(EnemyLaser1 enemyLaser1)
    {
        _enemyLaser1 = enemyLaser1;
    }
    public IEnumerator ShootDelayBoss()
    {
        yield return new WaitForSeconds(_enemyLaser1._fireRateBoss.delayInSeconds1);
        _enemyLaser1._fireRateBoss.canShoot1 = true;
        
    }
}

public class FireRateBoss
{
    public FireRateBoss()
    {
        canShoot1 = true;
        delayInSeconds1 = 1;
    }
    public bool canShoot1;
    public float delayInSeconds1;
}
