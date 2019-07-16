﻿using System.Collections;
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

	// Start is called before the first frame update
	void Start()
    {
		speed = initialSpeed;
		//decide initial direction
		if (initialDirection == 0 && Random.Range(0, 2) == 1)
		{
			initialDirection = 1;
		}
		else
		{
			initialDirection = -1;
			SpriteFlip();
		}
		direction = initialDirection;
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
		} else if (eggTimer >= 120)
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
		if (!collision.collider.isTrigger)
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
}
