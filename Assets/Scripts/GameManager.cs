using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isGameOver;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isGameOver == true)
        {
            RestartLevel();
        }
    }
    public void GameOver()
    {
        isGameOver = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }
}
