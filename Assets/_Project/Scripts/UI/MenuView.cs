using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    //public CampaignModel CampaignModel;
    public Text campaignTitle;
    public GameObject LoadingScreen, StartScreen, CampaignScreen, LevelScreen, levelScrollsParent, campaignbutotnsParent, levelScrollPrefab, campaignButtonPrefab, levelButtonPrefab;
    public List<GameObject> levelsScrolls;
    public enum STATE{
        Start,
        CampaignSelect,
        LevelSelect
    }
    public STATE currentState;
    // Start is called before the first frame update
    public void SetState(STATE state){
        Debug.Log("setting state to " + state);
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

    public void GenerateScrolls(){
        Debug.Log("Menu generating scrolls...");
        for (int i = 0; i < CampaignModel.campaigns.Count; i++)
        {
            CampaignButton instance = Instantiate(campaignButtonPrefab, campaignbutotnsParent.transform).GetComponent<CampaignButton>();
            instance.Initialize(CampaignModel.campaigns[i]);
            levelsScrolls.Add(Instantiate(levelScrollPrefab, levelScrollsParent.transform));
        }
        for (int i = 0; i < levelsScrolls.Count; i++)
        {
            for (int j = 0; j < CampaignModel.campaigns[i].levels.Count; j++)
            {
                LevelButton instance = Instantiate(levelButtonPrefab, levelsScrolls[i].GetComponent<LevelScroll>().levelButtonsParent.transform).GetComponent<LevelButton>();
                instance.Initialize(CampaignModel.campaigns[i].levels[j]);
            }
            
        }
        LoadingScreen.SetActive(false);
    }

    public void DisplayLevels(int index){
        Debug.Log("Displaying levels " + index);
        campaignTitle.text = CampaignModel.campaigns[index].title;

        for (int i = 0; i < levelsScrolls.Count; i++)
        {
            if(i != index)levelsScrolls[i].SetActive(false);
            else levelsScrolls[i].SetActive(true);
        }
    }
}
