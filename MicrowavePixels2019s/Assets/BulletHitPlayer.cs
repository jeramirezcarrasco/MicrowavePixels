using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletHitPlayer : MonoBehaviour
{
    public float DestroyObjectTimer;
    public Animator animator;
    public GameObject Impact;


    private void OnTriggerEnter2D(Collider2D hitInfo)
    {   
        if (hitInfo.gameObject.tag == "Player")
        {
            PlayerLife PlayerLife = hitInfo.GetComponent<PlayerLife>();
            PlayerLife.TakeDamageTrigger();
            Instantiate(Impact, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if(hitInfo.gameObject.tag == "Ground")
        {
            Instantiate(Impact, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }
}
