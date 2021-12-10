using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent (typeof (AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource _audioSource;
    public static float[] _samples = new float[512]; //this divs the waves of amplitude coming from the song into this number
    float[] _freqBand = new float[8]; //this groups and avareges ranges of samples
    float[] _bandBuffer = new float[8]; //this smooth bands numbers through the frames
    float[] _bufferDecrease = new float[8];
    float[] _freqBandHighest = new float[8];
    [SerializeField] public static float[] audioBand = new float[8];
    [SerializeField] public static float[] audioBandBuffer = new float[8];
    [SerializeField] public static float bandSum, bandBufferSum;
    [SerializeField] public static float amplitude, amplitudeBuffer;
    float amplitudeHighest;
    public float audioProfile;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        AudioProfile(audioProfile);
    }

    void AudioProfile(float audioProfile){
        for (int i = 0; i < 8; i++)
        {
            _freqBandHighest [i] = audioProfile;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        SumBands();
        GetAmplitude();
    }

    void GetAmplitude(){
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;
        for (int i = 0; i < 8; i++)
        {
            currentAmplitude = audioBand[i];
            currentAmplitudeBuffer = audioBandBuffer[i];
        }

        if(currentAmplitude > amplitudeHighest){
            amplitudeHighest = currentAmplitude;
        }
        amplitude = currentAmplitude / amplitudeHighest;
        amplitudeBuffer = currentAmplitudeBuffer / amplitudeHighest;

    }

    void CreateAudioBands(){
        for (int i = 0; i < 8; i++)
        {
            if (_freqBand[i] > _freqBandHighest[i]){
                _freqBandHighest[i] = _freqBand[i];
            }
            audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
            audioBandBuffer [i] = (_bandBuffer[i] / _freqBandHighest[i]);
        }
    }

    void GetSpectrumAudioSource(){
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    void BandBuffer(){
        for (int i = 0; i < 8; i++)
        {
            if(_freqBand[i] > _bandBuffer[i]){
                _bandBuffer[i] = _freqBand[i];
                _bufferDecrease[i] = 0.005f;
            }
            if(_freqBand[i] < _bandBuffer[i]){
                _bandBuffer[i] -= _bufferDecrease[i];
                _bufferDecrease[i] *=1.2f;
            }
        }
    }

    void MakeFrequencyBands(){
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            
            if(i == 7 ){
                sampleCount +=2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                    count++;
            }

            average /= count;
            _freqBand [i] = average * 10;
        }
    }

    void SumBands(){
        bandSum = audioBand.Sum();
        bandBufferSum = audioBandBuffer.Sum();
    }
}
