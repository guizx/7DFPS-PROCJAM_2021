using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier : MonoBehaviour
{
    public float modifier = 1.0f;
    public float multiplier = 1.0f;
    public bool smooth;
    public Range rangeToFollow;
    public enum Range{
        SubBass = 0,
        Bass = 1,
        LowMidRange = 2,
        Midrange1 = 3,
        Midrange2 = 4,
        UpperMidrange = 5,
        Presence = 6,
        Brilliance = 7,
        Sum = 8
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Modify()
    {
        switch (rangeToFollow)
        {
            case Range.Sum:
            break;
            if(smooth) modifier = AudioPeer.bandBufferSum;
            else modifier = AudioPeer.bandSum;

            default:
            if(smooth) modifier = AudioPeer.audioBandBuffer[(int)rangeToFollow];
            else modifier = AudioPeer.audioBand[(int)rangeToFollow];
            break;
        }
    }
}
