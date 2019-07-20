using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
	[SerializeField] float dmgCoolMax = 50;
	private int dmgCoolTimer = 50;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		print("water trigger enter");
		if (collision.gameObject.tag == "Player")
		{
			PlayerLife PlayerLife = collision.GetComponent<PlayerLife>();
			PlayerLife.TakeDamageTrigger();
			dmgCoolTimer = 0;
		}
	}

	//private void OnTriggerStay2D(Collider2D collision)
	//{
	//	print("water trigger stay");
	//	if(collision.gameObject.tag == "Player" && dmgCoolTimer >= dmgCoolMax)
	//	{
	//		PlayerLife PlayerLife = collision.GetComponent<PlayerLife>();
	//		PlayerLife.TakeDamageTrigger();
	//		dmgCoolTimer = 0;
	//	}
	//}

    // Update is called once per frame
    void FixedUpdate()
    {
		if (dmgCoolTimer < dmgCoolMax)
		{
			dmgCoolTimer++;
		}
    }
}
