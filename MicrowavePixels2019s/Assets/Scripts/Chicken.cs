using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
	[SerializeField] float initialSpeed = 0.1f;
	[SerializeField] int initialDirection = 0;
	[SerializeField] float eggDelaySec = 5f;
	[SerializeField] GameObject egg;
	private float speed;
	private int direction;
	private int eggTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
		speed = initialSpeed;
		//decide initial direction
		if (initialDirection == 0 && Random.Range(0, 2) == 1)
		{
			initialDirection = 1;
		}
		else initialDirection = -1;
		direction = initialDirection;
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		//move
		transform.position += new Vector3(speed * direction, 0, 0);
		//timers
		eggTimer++;
		float timerMax = 300f;
		float timerSMax = 150f;
		if (eggTimer >= 240)
		{
			timerMax = (eggDelaySec * 50f) - 25f + Random.Range(0f, 51f);
			timerSMax = (eggDelaySec/2 * 50f) - 25f + Random.Range(0f, 51f);
		}
		if (eggTimer >= timerSMax)
		{
			if (Random.value > 0.9)
			{
				ToggleStopMoving();
			}
		}
		if (eggTimer >= timerMax)
		{
			Instantiate(egg, transform.position, transform.rotation);
			eggTimer = 0;
		}
	}

	private void ToggleStopMoving()
	{
		if (speed > 0)
		{
			speed = 0;
		}
		else speed = initialSpeed;
	}

	//switch direction if something is hit
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.isTrigger)
		{ direction = -direction; }
	}
}
