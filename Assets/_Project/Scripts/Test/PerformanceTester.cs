using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PerformanceTester : MonoBehaviour
{
    public GameObject[] cubes;
    public int size;
    public InputField input;

    private void Start() {
        //cubes = GameObject.FindGameObjectsWithTag("Enemy");
    }
    public void ActivateCubes(){
        string text = input.text;
        int.TryParse(text, out size);
        for (int i = 0; i < size; i++)
        {
            cubes[i].SetActive(true);
        }
    }

    public void Restart(){
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
