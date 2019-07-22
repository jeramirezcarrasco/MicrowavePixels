using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BttnSlidingDoor : MonoBehaviour
{

    public GameObject Wall;
    private bool push;
    private Animator animator;
    public float MoveDistance;
    public float acceleration;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!push && collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("BttnPressed");
            push = true;
            while(MoveDistance > 0)
            {
                Wall.transform.Translate(Vector2.right * acceleration * Time.deltaTime);
                MoveDistance = MoveDistance - acceleration;

            }
        }
    }
}
