using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //handle to Text
    [SerializeField] private Text scoreText;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text restartText;
    [SerializeField] private Button restartButton;

    [SerializeField] private Image livesImage;
    [SerializeField] private Sprite[] liveSprites;

    private GameManager gameManager;

    void Start()
    {
        // assign text component to the handle
        scoreText.text = "Score: " + 0;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        
    }

    void Update()
    {
       
    }

    // method to update score

    public void UpdateScore(int playerScore)
    {
        scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        //display im sprite
        // give it a new one based on the currentlives index
        livesImage.sprite = liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        gameManager.GameOver();
        gameOverText.gameObject.SetActive(true);
        
#if UNITY_ANDROID
        restartButton.gameObject.SetActive(true);
        restartText.gameObject.SetActive(false);
#else
        restartButton.gameObject.SetActive(false);
        restartText.gameObject.SetActive(true);
#endif

        StartCoroutine(GameOverFlicker());
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
#if UNITY_ANDROID            
            gameOverText.text = "Game Over";
            yield return new WaitForSeconds(0.5f);            
            gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
#else
            restartText.gameObject.SetActive(true);
            gameOverText.text = "Game Over";            
            yield return new WaitForSeconds(0.5f);
            restartText.gameObject.SetActive(false);
            gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
#endif

        }
    }
}
