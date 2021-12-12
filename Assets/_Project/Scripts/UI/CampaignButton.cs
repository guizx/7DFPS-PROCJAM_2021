using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignButton : MonoBehaviour
{
    public CampaignData data;
    public MainController mainController;
    public Text buttonText;
    public void Initialize(CampaignData data) {
        this.data = data;
        buttonText.text = data.title;
        mainController = GameObject.Find("MainController").GetComponent<MainController>();   
    }

    public void Click(){
        Debug.Log(data.title + " button clicked!");
        mainController.CampaignSelect(data.index);
    }
}
