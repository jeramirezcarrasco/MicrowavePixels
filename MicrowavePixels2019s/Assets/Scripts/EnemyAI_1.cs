using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_1 : MonoBehaviour
{
    public LayerMask obstacleMask;

    public float Maxspeed;
    public float DownView;
    float Speed;
    public float Acceleration;

    private bool MovRight = true, hitWall = false;
    public int AlertViewRange;

    public Transform GroundDetection1;
    public Transform GroundDetection2;
    private LineOfSight lineofsight;
    private Shooting1 shooting1;
    private ChaseAI chaseAI;
    private LineOfSightVisual lineOfSightVisual;
    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        lineOfSightVisual = GetComponent<LineOfSightVisual>();
        lineofsight = GetComponent<LineOfSight>();
        shooting1 = GetComponent<Shooting1>();
        chaseAI = GetComponent<ChaseAI>();
    }

    private void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        float Speed = Maxspeed;
    }
    void Update()
    {
        if (!lineofsight.Spoted())
        {
            lineOfSightVisual.DrawFieldOfView();
            lineofsight.CurrFov = lineofsight.Fov;
            lineofsight.CurrRange = lineofsight.range;
            shooting1.endShooting();
            Patrol();
        }
        else if (lineofsight.Spoted())
        {
            lineOfSightVisual.viewMesh.Clear();
            lineOfSightVisual.viewMesh.Clear();
            lineofsight.CurrRange = AlertViewRange;
            lineofsight.CurrFov = 110;
            shooting1.startShooting();
            shooting1.Point();
            chaseAI.Follow();
        }
            
    }

    void Patrol()
    {
        transform.Translate(Vector2.right * Speed * Time.deltaTime);
        RaycastHit2D groundSlowDown = Physics2D.Raycast(GroundDetection2.position, Vector2.down, DownView);
        RaycastHit2D ground = Physics2D.Raycast(GroundDetection1.position, Vector2.down, DownView);

        DetectWall();

        if (groundSlowDown.collider == false)
        {
            if (Speed > (Maxspeed / 3))
            {
                Speed -= Acceleration;
            }
        }
        else
        {
            if (Speed < Maxspeed)
            {
                Speed += Acceleration;
            }
        }
        if (ground.collider == false || hitWall)
        {
            if (MovRight)
            {
                transform.eulerAngles = new Vector3(0, 0, 180);
                mySpriteRenderer.flipY = !mySpriteRenderer.flipY;
                MovRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                mySpriteRenderer.flipY = !mySpriteRenderer.flipY;
                MovRight = true;

            }

        }
    }

    private void DetectWall()
    {
        RaycastHit2D wallHit = Physics2D.Raycast (
            new Vector2 (
                MovRight ?
                    GetComponent<BoxCollider2D>().bounds.max.x
                :
                    GetComponent<BoxCollider2D>().bounds.min.x,
                GetComponent<BoxCollider2D>().bounds.center.y
            ),
            MovRight ? Vector2.right : Vector2.left,
            Mathf.Infinity,
            obstacleMask
        );

        if (wallHit)
        {
            if (wallHit.distance == 0) hitWall = true;
            else hitWall = false;
        }
    }

}

    
