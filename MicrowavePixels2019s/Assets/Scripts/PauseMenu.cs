using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{

    public static bool Ispause = false;
    public AudioMixer audioMixer;

    public GameObject PauseMenuUI;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Ispause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Ispause = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0.00001f;
        Ispause = true;
    }

    public void Volume(float i)
    {
        audioMixer.SetFloat("Volume", i);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
