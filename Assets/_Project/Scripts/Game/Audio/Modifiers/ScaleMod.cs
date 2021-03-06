using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMod : Modifier
{
    Vector3 baseScale, modifiedScale;
    public bool heightOnly;
    // Start is called before the first frame update
    void Start()
    {
        baseScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Modify();
        if(heightOnly) modifiedScale = new Vector3(baseScale.x, modifier + baseScale.y, baseScale.z);
        else modifiedScale = new Vector3(modifier + baseScale.x, modifier + baseScale.y, modifier + baseScale.z);
        //else modifiedScale = new Vector3((AudioPeer.audioBandBuffer[(int)rangeToFollow] * multiplier) + baseScale.x, (AudioPeer.audioBandBuffer[(int)rangeToFollow] * multiplier) + baseScale.y, (AudioPeer.audioBandBuffer[(int)rangeToFollow] * multiplier) + baseScale.z);
        //if(!float.IsNaN(modifiedScale.x)) transform.localScale = modifiedScale;
        //transform.localScale = baseScale * modifier * multiplier;
        transform.localScale = modifiedScale;
    }
}
