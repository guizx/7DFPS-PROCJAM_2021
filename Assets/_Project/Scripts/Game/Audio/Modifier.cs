using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier : MonoBehaviour
{
    public float modifier = 1.0f, multiplier = 1.0f, limitMax = 1.0f, limitMin = 0.0f;
    public float[] allModifiers = new float[8];
    public bool smooth, multiply, limit;
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
        Sum = 8,
        Amplitude = 9,
        All = 10
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
            if(smooth) modifier = AudioPeer.bandBufferSum;
            else modifier = AudioPeer.bandSum;
            break;

            case Range.Amplitude:
            if(smooth) modifier = AudioPeer.amplitudeBuffer;
            else modifier = AudioPeer.amplitude;
            break;

            case Range.All:
            for (int i = 0; i < 8; i++)
            {
                if(smooth) allModifiers[i] = AudioPeer.audioBandBuffer[i];
                else allModifiers[i] = AudioPeer.audioBand[i];
            }
            break;

            default:
            if(smooth) modifier = AudioPeer.audioBandBuffer[(int)rangeToFollow];
            else modifier = AudioPeer.audioBand[(int)rangeToFollow];
            break;
        }

        if(multiply) modifier *= multiplier;
        if(limit && modifier > limitMax) modifier = limitMax;
        if(limit && modifier < limitMin) modifier = limitMin;
    }
}
