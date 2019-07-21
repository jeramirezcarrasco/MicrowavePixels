using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : MonoBehaviour
{
    [HideInInspector] public GameObject[] turrets;

    [Range(1, 5)] public float turretRangeMultiplier = 2;
    [Range(1, 20)] public int alertDuration = 5;

    Controller2D controller;
    private float caughtEggCount;

    // Start is called before the first frame update
    void Start()
    {
        turrets = GameObject.FindGameObjectsWithTag("Turret");
        controller = GetComponent<Controller2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CaughtEgg();
    }

    private void CaughtEgg()
    {
        if (controller.caughtEgg) caughtEggCount += Time.deltaTime;

        if (caughtEggCount >= alertDuration && controller.canResetTurretRange)
        {
            caughtEggCount = Mathf.Epsilon;
            foreach (GameObject patrol in controller.patrols)
                patrol.GetComponent<EnemyAI_1>().onAlert = false;
            controller.caughtEgg = false;
            ResetTurretRange();
            controller.canResetTurretRange = false;
        }

        if (controller.resetEggCount)
        {
            controller.resetEggCount = false;
            caughtEggCount = Mathf.Epsilon;
        }
    }

    public void IncreaseTurretRange()
    {
        foreach (GameObject turret in turrets)
        {
            turret.GetComponent<LineOfSight>().range *= turretRangeMultiplier;
            turret.GetComponent<LineOfSightVisual>().viewRadius *= turretRangeMultiplier;
        }
    }

    public void ResetTurretRange()
    {
        foreach (GameObject turret in turrets)
        {
            turret.GetComponent<LineOfSight>().range /= turretRangeMultiplier;
            turret.GetComponent<LineOfSightVisual>().viewRadius /= turretRangeMultiplier;
        }
    }
}
