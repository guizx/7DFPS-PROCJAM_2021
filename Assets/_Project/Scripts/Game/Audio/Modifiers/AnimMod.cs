using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimMod : Modifier
{
    public Animator anim;
    public string ParamName;
    public float threshold;
    public bool affectSpeed;
    public ParamType param;
    public enum ParamType{
        Trigger,
        Int,
        Float,
        Bool,
        JustSpeed
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Modify();
        if (anim == null)
            return;

        if(param == ParamType.Trigger && modifier > threshold)  anim.SetTrigger(ParamName);
        if(affectSpeed) anim.speed = modifier * multiplier;
    }
}
