using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrollCredits : MonoBehaviour
{
	[SerializeField] float scrollSpeed;
	[SerializeField] float yCreditsEnd;

    // Update is called once per frame
    void FixedUpdate()
    {
		transform.Translate(new Vector3(0, scrollSpeed, 0));
		print(GetComponent<RectTransform>().position.y);
		if (GetComponent<RectTransform>().position.y > yCreditsEnd) SceneManager.LoadScene(0);
    }
}
