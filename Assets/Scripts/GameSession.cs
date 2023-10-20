using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] float score = 0;
    [SerializeField] float levelReloadDelay = 1f;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    //Singleton pattern
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void ProcessPlayerDeath() {
        if(playerLives > 1) {
            TakeLife();
        }
        else {
            ResetGameSession();
        }
    }

    public void AddToScore(float scoreIncrement) {
        score += scoreIncrement;
        scoreText.text = score.ToString();
    }

    void TakeLife() {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadLevel(currentSceneIndex, false));

        livesText.text = playerLives.ToString();
    }

    void ResetGameSession() {
        StartCoroutine(LoadLevel(0, true));
    }

    private IEnumerator LoadLevel(int index, bool refreshSession) {
        yield return new WaitForSecondsRealtime(levelReloadDelay);
        
        //Refresho la sessione
        if(refreshSession) {
            FindObjectOfType<ScenePersist>().ResetScenePersist();
            SceneManager.LoadScene(index);
            Destroy(gameObject);
        }
        else {
            SceneManager.LoadScene(index);
        }
    }
}
