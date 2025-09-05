using Proyecto26;
using UnityEngine;

public class FirebaseAnalytics : Singleton<FirebaseAnalytics>
{
    private string guest;
    private int deaths = 0;
    private float timer;

    private bool stopTimer = false;
    private bool gameVictory = false;
    [SerializeField] private float sessionStartTime;


    private void Start()
    {
        string randomName = $"Guest{Random.Range(0, 10000)}{System.Guid.NewGuid()}";

        if (!PlayerPrefs.HasKey("name_player"))
        {
            PlayerPrefs.SetString("name_player", randomName);
            guest = randomName;
        }
        else
        {
            guest = PlayerPrefs.GetString("name_player");
        }

        deaths = PlayerPrefs.GetInt("death_player", 0);
        sessionStartTime = Time.time;
        gameVictory = false;
    }

    public void AddNewRecord(bool victory)
    {
        Debug.Log("Trying add new record");
        gameVictory = victory;
        float sessionTime = Time.time - sessionStartTime;
        //string wave = SpawnManager.Instance?.GetWave();
        string victoryText = "";
        if (victory)
        {
            victoryText = "Victory";
        }
        else
        {
            victoryText = "Not Winner";
            //wave = SpawnManager.Instance?.GetWave();
        }

        User user = new User(guest, System.DateTime.Today.ToString(), victoryText, sessionTime, deaths);
        string url = "https://synth-beat-db-default-rtdb.firebaseio.com/.json";
        RestClient.Post(url, user, (e, value) =>
        {
            Debug.Log($"E:{e} Value:{value}");
        });
    }

    public void RecordDeath()
    {
        deaths++;
        PlayerPrefs.SetInt("death_player", deaths);
        AddNewRecord(victory: false);
        Debug.LogWarning("FIREBASE_ANALYTICS: PLAYER DEATH");
    }

    public void RecordGameCompletion()
    {
        AddNewRecord(victory: true);
        Debug.LogWarning("FIREBASE_ANALYTICS: GAME COMPLETION");
    }

    public void ClearDeaths()
    {
        PlayerPrefs.SetInt("death_player", 0);
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        ClearDeaths();
        AddNewRecord(victory: gameVictory);
        Debug.LogWarning("FIREBASE_ANALYTICS: PLAYER SESSION ENDED");

    }
}