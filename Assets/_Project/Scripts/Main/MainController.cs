using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.IO;
using System;
public class MainController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CampaignModel campaignModel;
    [SerializeField] MenuView menuView;
    public bool transition;

    void Start()
    {
        StartCoroutine(GetCampaigns());
        if(transition) menuView.SetState(MenuView.STATE.CampaignSelect);
        else menuView.SetState(MenuView.STATE.Start);
    }

    void Test(){
        DirectoryInfo mainDirectory = new DirectoryInfo(Application.streamingAssetsPath + "/Music/");
        DirectoryInfo[] directories = mainDirectory.GetDirectories ();
        foreach (var directory in directories)
        {
            Debug.Log("Folder name is " + directory.Name);

            var files = directory.GetFiles();
            foreach (var file in files)
            {
                if(file.FullName.EndsWith(".mp3")) Debug.Log("Music path is " + file);
            }
        }
    }
    
    IEnumerator GetCampaigns(){
        DirectoryInfo mainDirectory = new DirectoryInfo(Application.streamingAssetsPath + "/Music/");
        DirectoryInfo[] directories = mainDirectory.GetDirectories ();
        for (int i = 0; i < directories.Length; i++)
        {
            CampaignData campaign = new CampaignData();
            campaign.title = directories[i].Name;
            campaign.levels = new List<LevelData>();
            campaign.index = i;

            var files = directories[i].GetFiles();
            for (int j = 0; j < files.Length; j++)
            {
                if(files[j].FullName.EndsWith(".mp3")){
                    //Debug.Log("Song is " + file.Name);
                    LevelData level = new LevelData();
                    level.title = files[j].Name;
                    level.index = j;
                    using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(files[j].FullName, AudioType.MPEG))
                    {
                        yield return www.SendWebRequest();
                        level.song = DownloadHandlerAudioClip.GetContent(www);
                        level.levelTime = level.song.length;
                    }
                    campaign.levels.Add(level);
                }
            }
            campaignModel.campaigns.Add(campaign);
        }
    }

    public void Campaign(){
        menuView.SetState(MenuView.STATE.CampaignSelect);
    }

    public void PlayLevel(){
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void Exit(){
        Application.Quit();
    }
}
