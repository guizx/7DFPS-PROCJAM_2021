using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public LevelData data;
    public MainController mainController;
    public LevelInfo infoPrefab;
    private void Start() {
        mainController = GameObject.Find("MainController").GetComponent<MainController>();   
    }

    public void Click(){
        Instantiate(infoPrefab).GetComponent<LevelInfo>().data = data;
        mainController.PlayLevel();
    }
}
