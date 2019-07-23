using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
	[SerializeField] float pullSpeed = 1;
	[SerializeField] bool pullX;
	[SerializeField] bool pullY;
	private Vector3 pullVelocity;
	private bool pullingY = false;
	private bool pullingX = false;

	private int ix = 0;
	[SerializeField] int ixMax = 4;
	private int iy = 0;
	[SerializeField] int iyMax = 4;
	private Vector3 translateAmount;
	private int buttCoolTimer = 0;
	[SerializeField] float coolTimerLimit = 50;

	private GameObject player;
	private Player pScript;
	private float defFall;
	private float defMove;
	private Animator buttAnim;
	private Collider2D[] cols = new Collider2D[10];
	private int currentColliderOverlapCount;
	[SerializeField] ContactFilter2D playerFilter;

	// Start is called before the first frame update
	void Start()
    {
		buttAnim = transform.GetChild(0).GetComponent<Animator>();

		player = GameObject.FindGameObjectWithTag("Player");
		pScript = player.GetComponent<Player>();
		//gets default values for variables
		defFall = pScript.fallSpeed;
		defMove = pScript.moveSpeed;
		currentColliderOverlapCount = transform.GetChild(0).GetComponent<BoxCollider2D>().OverlapCollider(playerFilter, cols);

		if (pullY)
		{
			pullVelocity.y += pullSpeed;
		}
		if (pullX)
		{
			pullVelocity.x += pullSpeed;
		}
	}

    // Update is called once per frame
    void Update()
    {
		//prevents the player from moving or falling while pulling
		if ((pullingX && Mathf.Abs(player.transform.position.x - transform.position.x) > 1) ||
			(pullingY && player.transform.position.y < transform.position.y - 1))
		{
			pScript.fallSpeed = 10;
			pScript.moveSpeed = 0;
		}
		else
		{
			pScript.fallSpeed = defFall;
			pScript.moveSpeed = defMove;
		}
		//check if the player touches the button
		int c = transform.GetChild(0).GetComponent<BoxCollider2D>().OverlapCollider(playerFilter, cols);

		if (c > currentColliderOverlapCount)
		{
			if (pullY && buttCoolTimer >= coolTimerLimit)
			{ 
				pullingY = true;
				buttCoolTimer = 0;
			}
			if (pullX && buttCoolTimer >= coolTimerLimit)
			{
				pullingX = true;
				buttCoolTimer = 0;
			}
			buttAnim.SetTrigger("BttnPressed");
		}
	}
	private void FixedUpdate()
	{
		//button cooldown timer
		buttCoolTimer++;
		if (buttCoolTimer >= coolTimerLimit)
		{
			if (!transform.GetChild(0).GetComponent<SpriteRenderer>().flipX)
			{
				transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
			}
		}
		else if (transform.GetChild(0).GetComponent<SpriteRenderer>().flipX)
		{
			transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
		}

		//calculates the amount to translate the player and translates the player
		if (pullingY && player.transform.position.y < transform.position.y - 1)
		{
			if (iy <= iyMax)
			{
				if (iy == 0)
				{
					translateAmount = pullVelocity * (transform.position.y - player.transform.position.y) / iyMax;
				} else if (iy == iyMax)
				{
					iy = 0;
					pullingY = false;
				}
				player.transform.Translate(translateAmount);
				iy++;
			}
		}
		if (pullingX && Mathf.Abs(player.transform.position.x - transform.position.x) > 1)
		{
			if (ix <= ixMax)
			{
				if (ix == 0)
				{
					translateAmount = pullVelocity * (transform.position.x - player.transform.position.x) / ixMax;
				}
				else if (ix == ixMax)
				{
					ix = 0;
					pullingX = false;
				}
				player.transform.Translate(translateAmount);
				ix++;
			}
		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			if(pullingY)
			{
				collision.transform.position = new Vector3(transform.position.x, transform.position.y - 0.8f, 0);
				pullingY = false;
			}
			if (pullingX)
			{
				pullingX = false;
			}
		}
	}
}
