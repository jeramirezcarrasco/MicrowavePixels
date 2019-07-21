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

    [HideInInspector] public bool onAlert = false;

    private bool MovRight = true, hitWall = false;
    public int AlertViewRange;

    public Transform GroundDetection1;
    public Transform GroundDetection2;
    private LineOfSight lineofsight;
    private Shooting1 shooting1;
    private ChaseAI chaseAI;
    private LineOfSightVisual lineOfSightVisual;
    private SpriteRenderer mySpriteRenderer;

    Transform playerTrans;

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
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        float Speed = Maxspeed;
    }
    void Update()
    {
        if (!lineofsight.Spoted() && !onAlert)
        {
            lineOfSightVisual.DrawFieldOfView();
            lineofsight.CurrFov = lineofsight.Fov;
            lineofsight.CurrRange = lineofsight.range;
            shooting1.endShooting();
            Patrol();
        }
        else if (lineofsight.Spoted() || onAlert)
        {
            MovRight = playerTrans.position.x > transform.position.x;
            lineOfSightVisual.viewMesh.Clear();
            lineOfSightVisual.viewMesh.Clear();
            lineofsight.CurrRange = AlertViewRange;
            lineofsight.CurrFov = 110;
            shooting1.startShooting();
            shooting1.Point();
            chaseAI.Follow();
        }

        DetectWall();
    }

    void Patrol()
    {
        transform.Translate(Vector2.right * Speed * Time.deltaTime);
        RaycastHit2D groundSlowDown = Physics2D.Raycast(GroundDetection2.position, Vector2.down, DownView);
        RaycastHit2D ground = Physics2D.Raycast(GroundDetection1.position, Vector2.down, DownView);

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
            hitWall = false;
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
                    GetComponent<BoxCollider2D>().bounds.max.x - 0.05f
                :
                    GetComponent<BoxCollider2D>().bounds.min.x + 0.05f,
                GetComponent<BoxCollider2D>().bounds.center.y
            ),
            MovRight ? Vector2.right : Vector2.left,
            Mathf.Infinity,
            obstacleMask
        );

        if (wallHit)
        {
            if (wallHit.distance <= 0.05f) hitWall = true;
        }
    }

}

    
