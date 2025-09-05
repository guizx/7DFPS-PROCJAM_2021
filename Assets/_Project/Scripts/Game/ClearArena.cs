using System;
using UnityEngine;

public class ClearArena : MonoBehaviour
{
    private void Awake()
    {
        LevelController.OnBossSpawned += HandleOnBossSpawned;
    }

    private void OnDestroy()
    {
                LevelController.OnBossSpawned -= HandleOnBossSpawned;

    }

    private void HandleOnBossSpawned()
    {
        Destroy(gameObject);
    }
}
