using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_1 : MonoBehaviour
{

    public float Maxspeed;
    public float DownView;
    float Speed;
    public float Acceleration;

    private bool MovRight = true;
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
        if (groundSlowDown.collider == false)
        {
            if (Speed > (Maxspeed/3))
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
        if (ground.collider == false)
        {
            if (MovRight)
            {
                transform.eulerAngles = new Vector3(0, 0, 180);

                MovRight = false;
                
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                MovRight = true;

            }

        }
    }

   
}

    
