using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour {
    
    public float range;//How far the enemy can see
    public int Fov; //Field of view(aka the angle the enemy can see)
    [SerializeField]
    Transform lineOfSightEnd; // This will reference a child transform INSIDE the enemy that dictates the lenght of sigh
    Transform player;
    public int CurrFov;
    public float CurrRange;

    BoxCollider2D baseCol;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        baseCol = GetComponentInParent<BoxCollider2D>();
        CurrFov = Fov;
        CurrRange = range;
    }

    public bool Spoted()
    {
        if (Vector2.Distance(player.position, transform.position) < CurrRange) //If the enemy are close
        {
            if (PlayerInFieldOfView()) //If the player is in the enemy FoV
            {
                if (!PlayerHiddenByObstacles())//If there is no obstacle betweent he player and the enemy
                {
                    //Debug.Log("Spoted");
                    return true;
                }
            }
        }
        return false;
    }

    bool PlayerInFieldOfView()
    {
        

        Vector2 directionToPlayer = player.position - transform.position; // represents the direction from the enemy to the player    
        Debug.DrawLine(transform.position, player.position, Color.magenta); // a line drawn in the Scene window equivalent to directionToPlayer

        Vector2 lineOfSight = lineOfSightEnd.position - transform.position; // the centre of the enemy's field of view, the direction of looking directly ahead
        Debug.DrawLine(transform.position, lineOfSightEnd.position, Color.red); // a line drawn in the Scene window equivalent to the enemy's field of view centre
        
        // calculate the angle formed between the player's position and the centre of the enemy's line of sight
        float angle = Vector2.Angle(directionToPlayer, lineOfSight);
        // if the player is within 65 degrees (either direction) of the enemy's centre of vision (i.e. within a 130 degree cone whose centre is directly ahead of the enemy) return true
        if (angle < CurrFov)
        {
            return true;
        }
            
        else
            return false;
    }

    bool PlayerHiddenByObstacles()
    {
        //Get disantnce between player and enemy
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        //Betweent he player and the enemy shoot a ray with the lenght of distanceToPlayer
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, player.position - transform.position, distanceToPlayer);
        Debug.DrawRay(transform.position, player.position - transform.position, Color.blue); // draw line in the Scene window to show where the raycast is looking

        //Checks all Ray hits 
        foreach (RaycastHit2D hit in hits)
        {
            // ignore the enemy's own colliders (and other enemies)
            if (hit.transform.tag == "Enemy" || hit.transform.tag == "Patrol")
                continue;

            // if anything other than the player is hit then it must be between the player and the enemy's eyes (since the player can only see as far as the player)
            if (hit.transform.tag != "Player")
            {
                return true;
            }
        }
        // if no objects were closer to the enemy than the player return false (player is not hidden by an object)
        return false;

    }
}

