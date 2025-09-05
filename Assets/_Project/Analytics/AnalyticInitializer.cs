// using System.Collections;
// using System.Collections.Generic;
// using Unity.Services.Analytics;
// using Unity.Services.Core;
// using UnityEngine;

// public class AnalyticInitializer : MonoBehaviour
// {
//     private async void Start()
//     {
//         try
//         {
//             // Inicializa os servi�os do Unity
//             await UnityServices.InitializeAsync();

//             if (UnityServices.State == ServicesInitializationState.Initialized)
//             {
//                 Debug.Log("Unity Analytics iniciado com sucesso.");
//                 AnalyticsService.Instance?.StartDataCollection();
//             }
//         }
//         catch (System.Exception e)
//         {
//             Debug.LogError($"Erro ao inicializar o Unity Services: {e.Message}");
//         }
//     }
// }