using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCredits : MonoBehaviour
{
	[SerializeField] float scrollSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
		transform.Translate(new Vector3(0, scrollSpeed, 0));
    }
}
