using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting1 : MonoBehaviour
{

    [SerializeField] private Transform shootPos;
    //objectPooler objectPool;

    [SerializeField] private float startTimeBtwShot;
    [SerializeField] private float timeBtwShot;
    public Transform Player;
    public float GunAimSpeed;
    public float attackSpeedSeconds;
    public GameObject Gun;
    public GameObject Bullet;
    public float Spread;
    private bool GunOn;
    private bool Busy;
    public float WakeUp;
    public float BulletGap;
    public float BulletSpeedGap;


    private void Awake()
    {
        //objectPool = objectPooler.Instance;
    }

    private void Start()
    {
        
        Player = GameObject.FindWithTag("Player").transform;
        GunOn = false;
    }

    private void Update()
    {
        //if(timeBtwShot < 0.2)
        //{
        //    objectPooler.SpawnFromPool("Bullets", shootPos.position, transform.rotation);
        //    timeBtwShot = startTimeBtwShot;
        //}
        //else
        //{
        //    timeBtwShot -= Time.deltaTime;
        //}

    }
    
    public void startShooting()
    {

        //Debug.Log("GunON");

        if (!GunOn && !Busy)
        {
            GunOn = true;
            StartCoroutine(Shoot());
        }


    }

    public void endShooting()
    {
        GunOn = false;
        //Debug.Log("GunOFF");
    }

    public void Point()
    {
        Vector2 direction = Player.position - Gun.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Gun.transform.rotation = Quaternion.Slerp(Gun.transform.rotation, rotation, GunAimSpeed * Time.deltaTime);
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(WakeUp);
        while(GunOn)
        {
            //GameObject bullet = objectPool.SpawnFromPool("Bullets", Gun.transform.position, Gun.transform.rotation);
            GameObject bullet = (GameObject)Instantiate(Bullet, Gun.transform.position, Gun.transform.rotation);
            bullet.transform.Rotate(0, 0, Random.Range(-Spread, Spread));
            BulletMove bulletMove = bullet.GetComponent<BulletMove>();
            bulletMove.speed = bulletMove.speed + Random.Range(-BulletSpeedGap,0);
            float Gap = attackSpeedSeconds + Random.Range(0, BulletGap);
            Busy = true;
            yield return new WaitForSeconds(Gap);
            Busy = false;
        }

    }

}
