using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class SongLoadTest : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(GetCampaigns());
    }

    IEnumerator GetCampaigns(){
        DirectoryInfo mainDirectory = new DirectoryInfo(Application.streamingAssetsPath + "/Music/");
        DirectoryInfo[] directories = mainDirectory.GetDirectories ();
        var files = directories[0].GetFiles();
        var file = files[0];
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(file.FullName, AudioType.MPEG))
        {
            yield return www.SendWebRequest();
            audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
            audioSource.Play();
        }
    }
}
