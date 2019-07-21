using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
	[SerializeField] float initialSpeed = 0.1f;
	[SerializeField] int initialDirection = 0;
	[SerializeField] float eggDelaySec = 5f;
	[SerializeField] GameObject egg;
	[SerializeField] float amountOfStopsInAnEggDelay = 1f;
	private float speed;
	private int direction;
	private int eggTimer = 0;
	private int fixedUpdates = 0;

    [HideInInspector] public GameObject[] turrets;

    [Range(1, 5)] public float turretRangeMultiplier = 2;
    [Range(1, 20)] public int eggDuration = 5;

    Controller2D controller;
    private float caughtEggCount;

    //private bool alertedEnemies = false;

    // Start is called before the first frame update
    void Start()
    {
		speed = initialSpeed;
		//decide initial direction
		if (initialDirection == 0 && Random.Range(0, 2) == 1)
		{
			initialDirection = 1;
		}
		else if (initialDirection == 0)
		{
			initialDirection = -1;
			SpriteFlip();
		}
		direction = initialDirection;

        turrets = GameObject.FindGameObjectsWithTag("Turret");
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller2D>();
    }

	// Update is called once per frame
	void FixedUpdate()
    {
        fixedUpdates++;
        //move
        if (fixedUpdates % 2 == 0)
        {
            transform.position += new Vector3(speed * direction, 0, 0);
        }
        //timers
        eggTimer++;
        float timerMax = 300f;
        float timerSMax = 150f;
        //set the timer max and timer to stop max to a random variation
        if (eggTimer >= 240)
        {
            timerMax = (eggDelaySec * 50f) - 25f + Random.Range(0f, 51f);
        }
        else if (eggTimer >= 120)
        {
            timerSMax = (eggDelaySec / amountOfStopsInAnEggDelay * 50f) - 25f + Random.Range(0f, 51f);
        }
        //toggle stop moving
        if (eggTimer >= timerSMax)
        {
            if (Random.value > 0.6)
            {
                ToggleStopMoving();
            }
        }
        //lay the egg
        if (eggTimer >= timerMax)
        {
            Instantiate(egg, transform.position, transform.rotation);
            eggTimer = 0;
        }

        if (controller.eggHit) print("egg is hit (from chicken)");

        CaughtEgg();
    }

    private void CaughtEgg()
    {
        if (controller.caughtEgg)
        {
            caughtEggCount += Time.deltaTime;
            //if (!alertedEnemies)
            //{
            //    alertedEnemies = true;
            //}
        }

        if (caughtEggCount >= eggDuration && controller.canResetTurretRange)
        {
            print("inside");
            print($"caughtEggCount before change: {caughtEggCount}");
            //alertedEnemies = false;
            caughtEggCount = Mathf.Epsilon;
            print($"caughtEggCount after change: {caughtEggCount}");
            foreach (GameObject enemy in controller.enemies) enemy.GetComponent<EnemyAI_1>().onAlert = false;
            controller.caughtEgg = false;
            ResetTurretRange();
            controller.canResetTurretRange = false;
        }

        if (controller.resetEggCount)
        {
            print("resetting egg count");
            controller.resetEggCount = false;
            caughtEggCount = Mathf.Epsilon;
        }
    }

    private void ToggleStopMoving()
	{
		if (speed > 0)
		{
			speed = 0;
		}
		else
		{
			speed = initialSpeed;
		}
	}

	//switch direction if something is hit
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == 9 || collision.gameObject.layer == 12)
		{
			direction = -direction;
			SpriteFlip();
		}
	}
	private void SpriteFlip()
	{
		if (!GetComponent<SpriteRenderer>().flipX)
		{
			GetComponent<SpriteRenderer>().flipX = true;
		} else GetComponent<SpriteRenderer>().flipX = false;
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
