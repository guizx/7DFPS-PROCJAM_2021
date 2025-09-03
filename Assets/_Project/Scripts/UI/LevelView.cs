using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    public LevelModel levelModel;
    public Text titleText, timerText, scoreText, bestScoreText;
    public GameObject pauseWindow, gameOverWindow, levelFinishedWindow;
    public Slider levelDurationSlider;
    // Start is called before the first frame update

    public ResultGame timeResult;
    public ResultGame scoreResult;
    public ResultGame enemiesResult;

    public List<ResultGame> results = new List<ResultGame>();

    public AudioSource audioSource;

    public void Initialize()
    {
        titleText.text = levelModel.title;
        scoreText.text = "0";
        if (levelModel.bestScore > 0) bestScoreText.text = "Best: " + levelModel.bestScore.ToString();
        else bestScoreText.text = "";

        levelDurationSlider.maxValue = levelModel.levelTime;
        levelDurationSlider.value = levelModel.timeRemaining;

        results.Clear();
        results.Add(timeResult);
        results.Add(scoreResult);
        results.Add(enemiesResult);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = levelModel.score.ToString();
    }

    public void DisplayTime()
    {
        levelDurationSlider.value = levelModel.timeRemaining;
        float minutes = Mathf.FloorToInt(levelModel.timeRemaining / 60);
        float seconds = Mathf.FloorToInt(levelModel.timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Pause()
    {
        pauseWindow.SetActive(true);
    }

    public void Resume()
    {
        pauseWindow.SetActive(false);
    }

    public void GameOver()
    {
        //gameOverWindow.SetActive(true);
        levelFinishedWindow.SetActive(true);
        StartCoroutine(ShowResultsCoroutine(results));

    }

    public void LevelFinished()
    {
        levelFinishedWindow.SetActive(true);
        StartCoroutine(ShowResultsCoroutine(results));
    }


    private IEnumerator ShowResultsCoroutine(List<ResultGame> results)
    {
        float minutes = Mathf.FloorToInt(levelModel.levelCounter / 60);
        float seconds = Mathf.FloorToInt(levelModel.levelCounter % 60);
        timeResult.text.SetText("TIME - " + string.Format("{0:00}:{1:00}", minutes, seconds));
        scoreResult.text.SetText("SCORE - " + levelModel.score.ToString());
        enemiesResult.text.SetText("ENEMIES KILLED - " + levelModel.enemiesKilled.ToString());

        for (int i = 0; i < results.Count; i++)
        {
            results[i].canvasGroup.alpha = 0;
        }

        yield return new WaitForSecondsRealtime(0.5f);

        for (int i = 0; i < results.Count; i++)
        {

            yield return new WaitForSecondsRealtime(0.3f);
            results[i].canvasGroup.alpha = 1;
            audioSource.Play();
        }
    }
}


[System.Serializable]
public class ResultGame
{
    public TextMeshProUGUI text;
    public CanvasGroup canvasGroup;
}