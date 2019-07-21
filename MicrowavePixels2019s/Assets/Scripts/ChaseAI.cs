using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAI : MonoBehaviour {

    Transform player;
    public int ChaseSpeed;
    public Transform GroundDetection1;
    public int DownViewDepth;

    SpriteRenderer sr;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindWithTag("Player").transform;
        sr = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame

    public void Follow()
    {
        RaycastHit2D ground = Physics2D.Raycast(GroundDetection1.position, Vector2.down, DownViewDepth);

        if (player.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
            if (!sr.flipY) sr.flipY = true;
        }
        else if (player.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            if (sr.flipY) sr.flipY = false;
        }

        if (ground.collider == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), ChaseSpeed * Time.deltaTime);
        }

        
    }
}
