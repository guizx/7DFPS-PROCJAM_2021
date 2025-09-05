using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public bool pause;
    public LevelData CurrentLevelData;
    public LevelInfo levelInfo;
    public LevelModel levelModel;
    public LevelView levelView;
    public AudioPeer audioPeer;
    public AudioClip audioClip;
    public bool canPause = true;
    public bool spawnedBoss = false;
    public GameObject boss;
    public GameObject bossPanel;
    public Transform player;
    public Transform cameraHold;
    public static Action OnBossSpawned;

    public AudioClip bossFightMusic;

    public SpawnerController SpawnController;
    public SpawnMod levelOneSpawner;
    public SpawnMod levelTwoSpawner;

    bool isGameStart;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
        Enemy.OnEnemyDied += HandleOnEnemyDied;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    public void StartGame()
    {
        levelInfo = GameObject.FindGameObjectWithTag("LevelInfo").GetComponent<LevelInfo>();
        CurrentLevelData = levelInfo.data;
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
        levelModel.enemiesKilled = 0;

        //Debug.Log(levelModel.title + " level model is completed with + " + levelModel.levelTime + "seconds in time!");
        levelView.Initialize();
        if (SpawnController == null)
            SpawnController = FindFirstObjectByType<SpawnerController>();

        if (CurrentLevelData.index == 0)
            SpawnController.SetSpawner(levelOneSpawner);
        else
            SpawnController.SetSpawner(levelTwoSpawner);

        SpawnController.gameObject.SetActive(true);

        isGameStart = true;
        PlayerHealth.dead = false;
        //Destroy(levelInfo.gameObject);
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyDied -= HandleOnEnemyDied;

    }

    private void HandleOnEnemyDied()
    {
        levelModel.enemiesKilled++;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameStart)
            return;
        levelModel.levelCounter += Time.deltaTime;

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

        if (PlayerHealth.dead)
            return;

        if ((Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame) && canPause)
        {// && !pause){
            Debug.Log("TENTANDO PAUSAR");
            if (!pause)
                Pause();
            else
                Resume();
        }

        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame && canPause)
        {// && !pause){
            Debug.Log("TENTANDO PAUSAR");

            if (!pause)
                Pause();
            else
                Resume();
        }


        if (levelModel.timeRemaining <= 0 && !spawnedBoss)
        {
            spawnedBoss = true;
            OnBossSpawned?.Invoke();
            StartCoroutine(SpawnBossCoroutine());
        }

    }

    public IEnumerator SpawnBossCoroutine()
    {
        yield return new WaitForSeconds(2f);

        // Música do boss
        var audioSource = audioPeer.GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = bossFightMusic;
        audioSource.Play();

        // Painel inicial
        bossPanel.SetActive(true);

        // Preparar player
        ResetPlayerPositionAndRotation();
        DisablePlayerControls();
        ResetPlayerVelocity();

        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);

        // Reforçar reset antes do painel sumir
        ResetPlayerPositionAndRotation();
        player.GetComponent<PlayerMovement>().StopAnimations();
        DisablePlayerControls();
        yield return new WaitForSeconds(3f);

        // Esconder painel e mostrar player
        bossPanel.SetActive(false);
        player.gameObject.SetActive(true);

        ResetPlayerVelocity();
        ResetPlayerPositionAndRotation();
        cameraHold.position = new Vector3(0f, 1.1f, 0f);
        cameraHold.rotation = Quaternion.Euler(Vector3.zero);

        yield return new WaitForSeconds(0.5f);

        // Ativar boss
        boss.SetActive(true);
        boss.transform.position = new Vector3(0, 2.9f, 7f);
        yield return new WaitForSeconds(0.5f);

        ResetPlayerVelocity();
        EnablePlayerControls();
    }

    private void ResetPlayerPositionAndRotation()
    {
        player.position = new Vector3(0f, 1.1f, 0f);
        player.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void ResetPlayerVelocity()
    {
        var rb = player.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
    }

    private void DisablePlayerControls()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerShoot>().enabled = false;
    }

    private void EnablePlayerControls()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerShoot>().enabled = true;
    }

    public void AddScore(int score)
    {
        levelModel.score += score;
    }

    public void StopAudio()
    {
        audioPeer.gameObject.SetActive(false);
    }

    public void LevelFinished()
    {
        canPause = false;
        audioPeer.gameObject.SetActive(false);
        pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;
        levelView.LevelFinished();
    }



    public void LevelFinished(float delay)
    {
        FirebaseAnalytics.Instance?.RecordGameCompletion();
        StartCoroutine(LevelFInishedCoroutine(delay));
    }

    private IEnumerator LevelFInishedCoroutine(float duration)
    {
        canPause = false;
        yield return new WaitForSeconds(duration);
        audioPeer.gameObject.SetActive(false);
        pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;
        levelView.LevelFinished();
    }

    public void GameOver()
    {
        FirebaseAnalytics.Instance?.RecordDeath();
        canPause = false;
        pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;
        levelView.GameOver();
    }

    public void BackToMenu()
    {
        pause = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        levelModel.timeIsRunning = true;
        Time.timeScale = 1.0f;
        Destroy(levelInfo.gameObject);
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
    public void Pause()
    {
        audioPeer._audioSource.Pause();
        pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;
        levelView.Pause();
        levelModel.timeIsRunning = false;
    }

    public void Resume()
    {
        audioPeer._audioSource.Play();
        pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        levelModel.timeIsRunning = true;
        Time.timeScale = 1.0f;
        levelView.Resume();

    }

    public void Replay()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void Main()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
