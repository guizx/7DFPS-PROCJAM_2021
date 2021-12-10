using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionMod : Modifier
{
    Material material;
    Color color;
    
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
        color = material.color;
    }

    // Update is called once per frame
    void Update()
    {
        Modify();
        //if(smooth) Color color = new Color(AudioPeer.audioBandBuffer[(int)rangeToFollow], )
        material.SetColor("_EmissionColor", color * modifier);
    }
}
