using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public bool pause;
    public LevelInfo levelInfo;
    public LevelModel levelModel;
    public LevelView levelView;
    public AudioPeer audioPeer;
    public AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;

        levelInfo = GameObject.FindGameObjectWithTag("LevelInfo").GetComponent<LevelInfo>();
        //Debug.Log(levelInfo.data.title + " loaded in game scene");
        Debug.Log("Levelinfo title " + levelInfo.data.title);
        Debug.Log("Levelinfo index " + levelInfo.data.index);
        Debug.Log("Levelinfo song " + levelInfo.data.song.name);
        Debug.Log("Levelinfo time " + levelInfo.data.levelTime);
        AudioSource audioSource = audioPeer.GetComponent<AudioSource>();
        audioSource.clip = levelInfo.data.song;
        //Resume();
        audioSource.Play();

        levelModel.title = levelInfo.data.title;
        levelModel.levelTime = levelInfo.data.levelTime;
        levelModel.timeRemaining = levelModel.levelTime;
        levelModel.timeIsRunning = true;

        //Debug.Log(levelModel.title + " level model is completed with + " + levelModel.levelTime + "seconds in time!");
        levelView.Initialize();

        //Destroy(levelInfo.gameObject);
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

        if(Input.GetKeyDown(KeyCode.Escape)){// && !pause){
            Pause();
        }

        if(levelModel.timeRemaining <= 0) LevelFinished();
    }
    public void AddScore(int score){
        levelModel.score += score;
    }

    void LevelFinished(){
        pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;
        levelView.LevelFinished();
    }

    public void GameOver(){
        pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;
        levelView.GameOver();
    }
    public void BackToMenu(){
        pause = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        levelModel.timeIsRunning = true;
        Time.timeScale = 1.0f;
        Destroy(levelInfo.gameObject);
        SceneManager.LoadScene("BackToMenu", LoadSceneMode.Single);
    }
    public void Pause(){
        audioPeer._audioSource.Pause();
        pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;
        levelView.Pause();
        levelModel.timeIsRunning = false;
    }

    public void Resume(){
        audioPeer._audioSource.Play();
        pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        levelModel.timeIsRunning = true;
        Time.timeScale = 1.0f;
        levelView.Resume();
        
    }

    public void Main(){
        //SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void Exit(){
        Application.Quit();
    }
}
