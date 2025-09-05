// using System.Collections;
// using System.Collections.Generic;
// using Unity.Services.Analytics;
// using UnityEngine;
// using UnityEngine.Analytics;

// public class GameAnalytics : Singleton<GameAnalytics>
// {
//     [SerializeField] private float sessionStartTime;
//     [SerializeField] private int deathCount;
//     [SerializeField] private bool gameCompleted;

//     private void Start()
//     {
//         // Inicia o tempo de sess�o quando o jogo come�a
//         sessionStartTime = Time.time;
//         deathCount = PlayerPrefs.GetInt("player_deaths", 0);
//         gameCompleted = false;
//     }

//     public void RecordDeath()
//     {
//         // Incrementa o n�mero de mortes
//         deathCount++;
//         PlayerPrefs.SetInt("player_deaths", deathCount);
//         AnalyticsEvent analyticsEvent = new AnalyticsEvent
//         {
//             GameVictoryBool = false,
//             SessionTimeFloat = Time.time - sessionStartTime,
//             PlayerDiedInt = deathCount
//         };

//         AnalyticsService.Instance.RecordEvent(analyticsEvent);


//         Debug.Log($"Morte registrada. Total: {deathCount}");
//         Debug.LogWarning($"GAME_ANALYTCS: PLAYER_DIED");
//     }

//     public void RecordGameCompletion()
//     {
//         if (!gameCompleted)
//         {
//             gameCompleted = true;

//             AnalyticsEvent analyticsEvent = new AnalyticsEvent
//             {
//                 GameVictoryBool = true,
//                 SessionTimeFloat = Time.time - sessionStartTime,
//                 PlayerDiedInt = deathCount
//             };

//             AnalyticsService.Instance.RecordEvent(analyticsEvent);

//             Debug.LogWarning($"GAME_ANALYTCS: GAME_COMPLETED");
//         }
//     }

//     protected override void OnApplicationQuit()
//     {
//         Debug.LogWarning($"GAME_ANALYTCS: SESSION_ENDED");
//         base.OnApplicationQuit();
//         AnalyticsEvent analyticsEvent = new AnalyticsEvent
//         {
//             GameVictoryBool = gameCompleted,
//             SessionTimeFloat = Time.time - sessionStartTime,
//             PlayerDiedInt = deathCount
//         };

//         AnalyticsService.Instance.RecordEvent(analyticsEvent);
//     }
// }