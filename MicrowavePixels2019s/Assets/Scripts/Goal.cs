using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    BoxCollider2D playerCol;
    Collider2D goalCol;

    Controller2D playerController;

    int currentScene;

    // Start is called before the first frame update
    void Start()
    {
        playerCol = GameObject.FindGameObjectWithTag("Player").
            GetComponent<BoxCollider2D>();
        goalCol = GetComponent<Collider2D>();
        playerController = GameObject.FindGameObjectWithTag("Player").
            GetComponent<Controller2D>();
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (LandedOnGoal()) /*LoadNextLevel()*/ print("in goal");
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(currentScene + 1);
    }

    private bool LandedOnGoal()
    {
        if (playerCol.bounds.max.x > goalCol.bounds.min.x &&
            playerCol.bounds.min.x < goalCol.bounds.max.x &&
            playerCol.bounds.max.y > goalCol.bounds.min.y &&
            playerCol.bounds.min.y < goalCol.bounds.max.y)
            return true;
        else
            return false;
    }
}
