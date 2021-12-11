using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public LevelModel levelModel;
    public LevelView levelView;
    public AudioPeer audioPeer;
    public AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        audioClip = audioPeer.GetComponent<AudioSource>().clip;
        levelModel.title = audioClip.ToString();
        levelModel.levelTime = audioClip.length;
        levelModel.timeRemaining = levelModel.levelTime;
        levelModel.timeIsRunning = true;
        levelView.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelModel.timeIsRunning)
        {
            if (levelModel.timeRemaining > 0)
            {
                levelModel.timeRemaining -= Time.deltaTime;
                levelView.DisplayTime();
            }
            else
            {
                Debug.Log("Time has run out!");
                levelModel.timeRemaining = 0;
                levelModel.timeIsRunning = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            Pause();
        }
    }
    public void AddScore(int score){
        levelModel.score += score;
    }
    public void Pause(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;
        levelView.Pause();
        levelModel.timeIsRunning = false;
    }

    public void Resume(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
        levelView.Resume();
        levelModel.timeIsRunning = true;
    }

    public void Exit(){
        Application.Quit();
    }
}
