using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public LevelData data;
    public MainController mainController;
    public LevelInfo infoPrefab;
    public Text buttonText;
    public void Initialize(LevelData data) {
        this.data = data;
        buttonText.text = data.title;
        mainController = GameObject.Find("MainController").GetComponent<MainController>();   
    }

    public void Click(){
        Debug.Log("Button title " + data.title);
        Debug.Log("Button index " + data.index);
        Debug.Log("Button song " + data.song.name);
        Debug.Log("Button time " + data.levelTime);
        Instantiate(infoPrefab).GetComponent<LevelInfo>().data = data;
        mainController.PlayLevel();
    }
}
