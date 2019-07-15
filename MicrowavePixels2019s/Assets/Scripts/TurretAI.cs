using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour {

    public int LookSpeed;
    private LineOfSight lineofsight;
    private Shooting1 shooting1;
    private LineOfSightVisual lineOfSightVisual;
    int angle;
    bool left;
    public int AlertViewRange;
    public int MaxAngle;
    public int MinAngle;

    // Use this for initialization
    void Awake ()
    {
        lineOfSightVisual = GetComponent<LineOfSightVisual>();
        lineofsight = GetComponent<LineOfSight>();
        shooting1 = GetComponent<Shooting1>();

    }

    private void Start()
    {
        angle = 90;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!lineofsight.Spoted())
        {

            lineOfSightVisual.DrawFieldOfView();
            lineofsight.CurrFov = lineofsight.Fov;
            lineofsight.CurrRange = lineofsight.range;
            LookPatrol();
            shooting1.endShooting();

        }
        else if(lineofsight.Spoted())
        {
            lineOfSightVisual.viewMesh.Clear();
            lineofsight.CurrRange = AlertViewRange;
            lineofsight.CurrFov = 110;
            shooting1.startShooting();
            shooting1.Point();
        }

    }

    void LookPatrol()
    {
        if (left)
        {
            if (angle < MinAngle)
            {
                left = false;
            }
            angle -= 1;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, LookSpeed * Time.deltaTime);
        }
        else if (!left)
        {
            if (angle > MaxAngle)
            {
                left = true;
            }
            angle += 1;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, LookSpeed * Time.deltaTime);
        }
    }
}
