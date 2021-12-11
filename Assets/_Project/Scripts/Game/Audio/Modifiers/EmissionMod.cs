using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionMod : Modifier
{
    Material material;
    Color color, tweenedColor;
    
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
        color = material.color;
        tweenedColor = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        Modify();
        //if(smooth) Color color = new Color(AudioPeer.audioBandBuffer[(int)rangeToFollow], )
        material.SetColor("_EmissionColor", color * modifier);
    }

    public void TweenColor(){
        Debug.Log("Should be tweening");
        LeanTween.value( gameObject, SetTweenColor, color, tweenedColor, .25f ).setOnComplete(FinishTweenColor);
    }

    void SetTweenColor(Color c){
       material.SetColor("_EmissionColor", tweenedColor * modifier); 
    }

    void FinishTweenColor(){
        Debug.Log("even finished tweening");
        LeanTween.value( gameObject, SetTweenColor, tweenedColor, color, .25f);
    }
}
