using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizerTest : MonoBehaviour
{
    public float[] bandsVis, bandBufferVis;
    public float bandSumVis, bandBufferSumVis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bandsVis = AudioPeer.audioBand;
        bandBufferVis = AudioPeer.audioBandBuffer;
        bandSumVis = AudioPeer.bandSum;
        bandBufferSumVis = AudioPeer.bandBufferSum;
    }
}
