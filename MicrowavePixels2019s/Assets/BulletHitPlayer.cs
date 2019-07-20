using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletHitPlayer : MonoBehaviour
{
    public float DestroyObjectTimer;
    public Animator animator;


    private void OnTriggerEnter2D(Collider2D hitInfo)
    {   
        if (hitInfo.gameObject.tag == "Player")
        {
            PlayerLife PlayerLife = hitInfo.GetComponent<PlayerLife>();
            PlayerLife.TakeDamageTrigger();
            animator.SetBool("Explode",true);
            Destroy(gameObject, DestroyObjectTimer);
        }
        else if(hitInfo.gameObject.tag == "Ground")
        {
            animator.SetBool("Explode", true);
            Destroy(gameObject, DestroyObjectTimer);
        }

    }
}
