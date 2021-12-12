using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public LevelData data;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(data.title + " level info created!");
        DontDestroyOnLoad(this.gameObject);
    }
}
