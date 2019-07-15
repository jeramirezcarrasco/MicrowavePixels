using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{

    public float speed;
    public Rigidbody2D rb;
    public GameObject Impact;

    // Start is called before the first frame update
    void Start()
    {
        //rb.velocity = new Vector2(0,0);
        rb.velocity = transform.right * speed;

        Destroy(gameObject, 5);
        //Invoke("Disappear", 5);
    }
    
    //private void OnTriggerEnter2D(Collider2D other){
    //    if (other.gameObject.CompareTag("Player")){
    //        PidgeonMovement.Instance.GetDamage();
    //    }
    //}

    //void Disappear()
    //{
    //    gameObject.SetActive(false);
    //}

    //private void OnTriggerEnter2D(Collider2D hitInfo)
    //{
    //    Enemy enemy = hitInfo.GetComponent<Enemy>();
    //    if (enemy != null)
    //    {
    //        enemy.TakeDamage();
    //        Instantiate(Impact, transform.position, transform.rotation);
    //        Destroy(gameObject);
    //    }
    //    Enemy2 enemy2 = hitInfo.GetComponent<Enemy2>();
    //    if (enemy2 != null)
    //    {
    //        enemy2.TakeDamage();
    //        Instantiate(Impact, transform.position, transform.rotation);
    //        Destroy(gameObject);
    //    }
    //    if (hitInfo.gameObject.tag == "Bullet")
    //    {
    //        Instantiate(Impact, transform.position, transform.rotation);
    //        Destroy(gameObject);
    //    }

    //}

}
