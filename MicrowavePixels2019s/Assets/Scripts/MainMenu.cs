using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    //public void Options()
    //{
    //    SceneManager.LoadScene(2);
    //}

    public void Quit()
    {
        Application.Quit();
    }

	public void Credits()
	{
		SceneManager.LoadScene(7);
	}
}