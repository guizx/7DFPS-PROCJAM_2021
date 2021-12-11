using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    public GameObject StartScreen, CampaignScreen, LevelScreen;
    public List<ScrollRect> levelScrolls;
    public enum STATE{
        Start,
        CampaignSelect,
        LevelSelect
    }
    public STATE currentState;
    // Start is called before the first frame update
    public void SetState(STATE state){
        currentState = state;
        StartScreen.SetActive(false);
        CampaignScreen.SetActive(false);
        LevelScreen.SetActive(false);
        switch (currentState)
        {
            
            case STATE.Start:
            StartScreen.SetActive(true);
            break;
            case STATE.CampaignSelect:
            CampaignScreen.SetActive(true);
            break;
            case STATE.LevelSelect:
            LevelScreen.SetActive(true);
            break;
        }
    }
}
