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
    //[SerializeField] CampaignModel campaignModel;
    [SerializeField] MenuView menuView;
    public bool transition;

    void Start()
    {
        if(CampaignModel.campaigns.Count == 0) StartCoroutine(GetCampaigns());
        else menuView.GenerateScrolls();

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
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Generating campaigns...");
        DirectoryInfo mainDirectory = new DirectoryInfo(Application.streamingAssetsPath + "/Music/");
        DirectoryInfo[] directories = mainDirectory.GetDirectories ();
        for (int i = 0; i < directories.Length; i++)
        {
            CampaignData campaign = new CampaignData();
            campaign.title = directories[i].Name;
            campaign.levels = new List<LevelData>();
            campaign.index = i;

            var files = directories[i].GetFiles();
            Debug.Log("Found " + directories.Length + " folders");
            for (int j = 0; j < files.Length; j++)
            {
                if(files[j].FullName.EndsWith(".mp3")){
                    Debug.Log("Song is " + files[j].FullName);
                    LevelData level = new LevelData();
                    level.title = files[j].Name;
                    level.index = j;
                    /*/
                    using (var uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + files[j].FullName, AudioType.AUDIOQUEUE))
                    {
                        ((DownloadHandlerAudioClip)uwr.downloadHandler).streamAudio = true;
                        yield return uwr.SendWebRequest();
                        if (uwr.isNetworkError || uwr.isHttpError)
                        {
                            Debug.LogError(uwr.error);
                            yield break;
                        }
                        DownloadHandlerAudioClip dlHandler = (DownloadHandlerAudioClip)uwr.downloadHandler;
                        if (dlHandler.isDone)
                        {
                            AudioClip audioClip = dlHandler.audioClip;
                            if (audioClip != null)
                            {
                                level.song = DownloadHandlerAudioClip.GetContent(uwr);
                                Debug.Log("Playing song using Audio Source!");
                            }
                            else
                            {
                                Debug.Log("Couldn't find a valid AudioClip");
                            }
                        }
                        else
                        {
                            Debug.Log("The download process is not completely finished.");
                        }
                    }
                    //*/
                    /*/
                    using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + files[j].FullName, AudioType.MPEG))
                    {
                        Debug.Log("Trying to load song for " + files[j].FullName);
                        yield return www.SendWebRequest();

                        if (!String.IsNullOrEmpty(www.error)) {
                            Debug.Log(www.error);
                        }
                        else {
                            // Show results as text
                            Debug.Log(www.downloadHandler.text);
                        }
                        Debug.Log("Load successful!");
                        level.song = DownloadHandlerAudioClip.GetContent(www);
                        level.levelTime = level.song.length;
                    }
                    //*/
                    
                    /*/
                    WWW request = new WWW(files[j].FullName);
                    yield return request;
                    if(String.IsNullOrEmpty(request.error)) Debug.Log(request.error);
                    level.song = request.GetAudioClip();
                    level.levelTime = level.song.length;
                    //*/

                    //*/
                    var bytes = File.ReadAllBytes(files[j].FullName);;
                    while (bytes == null) yield return null;
                    level.song = NAudioPlayer.FromMp3Data(bytes);
                    level.levelTime = level.song.length;
                    //*/
                    
                    campaign.levels.Add(level);
                }
            }
            CampaignModel.campaigns.Add(campaign);
        }

        var debugLevel = CampaignModel.campaigns[0].levels[0];
        Debug.Log("debugLevel title " + debugLevel.title);
        Debug.Log("debugLevel index " + debugLevel.index);
        Debug.Log("debugLevel song " + debugLevel.song.name);
        Debug.Log("debugLevel time " + debugLevel.levelTime);
        menuView.GenerateScrolls();
    }

    public void BackToStart(){
        menuView.SetState(MenuView.STATE.Start);
    }

    public void Campaign(){
        menuView.SetState(MenuView.STATE.CampaignSelect);
    }
    
    public void CampaignSelect(int index){
        Debug.Log("Campaign " + index + " selected!");
        menuView.DisplayLevels(index);
        menuView.SetState(MenuView.STATE.LevelSelect);
    }

    public void PlayLevel(){
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void Exit(){
        Application.Quit();
    }
}
