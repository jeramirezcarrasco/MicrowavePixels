using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinPlatform : MonoBehaviour
{
	private GameObject player;
	private BoxCollider2D col;

    // Start is called before the first frame update
    void Start()
    {
		player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
		if (player.transform.position.y >= transform.position.y)
		{
			if (gameObject.layer != 9)
			{
				gameObject.layer = 9;
			}
		}
		else
		{
			if (gameObject.layer != 0)
			{
				gameObject.layer = 0;
			}
		}
    }
}
