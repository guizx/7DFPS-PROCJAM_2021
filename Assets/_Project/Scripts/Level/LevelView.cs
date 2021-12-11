using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    public LevelModel levelModel;
    public Text titleText, timerText, scoreText, bestScoreText;
    // Start is called before the first frame update
    public void Initialize()
    {
        titleText.text = levelModel.title;
        scoreText.text = "0";
        if(levelModel.bestScore > 0) bestScoreText.text = "Best: " + levelModel.bestScore.ToString();
        else bestScoreText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = levelModel.score.ToString();
    }

    public void DisplayTime()
    {
        float minutes = Mathf.FloorToInt(levelModel.timeRemaining / 60); 
        float seconds = Mathf.FloorToInt(levelModel.timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
