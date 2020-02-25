using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualization : MonoBehaviour
{
    public AudioSource _audioSource;
    [SerializeField]

    public int SampleSize = 1024;
    public static float[] _samples = new float[1024];

    public static float[] _Spectrum = new float[1024];

    public static float[] frequencyBand = new float[8];
    public static float[] _BandBuffer = new float[8];
    float[] BufferDecrease = new float[8];
    public float sampleRate;


    public float rmsValue;
    public float dbValue;
    public float pitchValue;

    #region input settings

    //麥克風輸入
    public bool usemicrophone;
    public AudioClip m_audioclip;
    public string SelectedDevice;

    public AudioMixerGroup _mixerGroupMicrophone, _mixerGroupMaster;

    #endregion


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (usemicrophone)
        {
            if (Microphone.devices.Length > 0)
            {
                SelectedDevice = Microphone.devices[0].ToString();
                _audioSource.outputAudioMixerGroup = _mixerGroupMicrophone;
                _audioSource.clip = Microphone.Start(SelectedDevice, true, 10, AudioSettings.outputSampleRate);
            }
            else
            {
                Debug.Log("microphone is");
                usemicrophone = false;
            }

        }
        if (!usemicrophone)
        {
            _audioSource.outputAudioMixerGroup = _mixerGroupMaster;
            _audioSource.clip = m_audioclip;
        }
        _audioSource.Play();


        sampleRate = AudioSettings.outputSampleRate;
    }

    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBand();
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetOutputData(_samples, 0);
        // 抓ＲＭＳ
        int i = 0;
        float sum = 0;
        for (; i < SampleSize; i++)
        {
            sum = _samples[i] * _samples[i];
        }
        rmsValue = Mathf.Sqrt(sum / SampleSize);

        // 抓ＤＢ值 （分貝）
        dbValue = 20 * Mathf.Log10(rmsValue / 0.1f);

        // 抓音高
        float maxV = 0;
        var maxN = 0;
        for (i = 0; i < SampleSize; i++)
        { // find max 
            if (!(_Spectrum[i] > maxV) || !(_Spectrum[i] > 0.0f))
                continue;

            maxV = _Spectrum[i];
            maxN = i; // maxN is the index of max
        }
        float freqN = maxN; // pass the index to a float variable
        if (maxN > 0 && maxN < sampleRate - 1)
        { // interpolate index using neighbours
            var dL = _Spectrum[maxN - 1] / _Spectrum[maxN];
            var dR = _Spectrum[maxN + 1] / _Spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        pitchValue = freqN * (sampleRate / 2) / SampleSize; // convert index to frequency
        // 抓音譜
        _audioSource.GetSpectrumData(_Spectrum, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBand()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                count++;
            }
            average /= count;
            frequencyBand[i] = average * 10;
        }
    }

    void BandBuffer()
    {
        for (int g = 0; g < 8; ++g)
        {
            if (frequencyBand[g] > _BandBuffer[g])
            {
                _BandBuffer[g] = frequencyBand[g];
                BufferDecrease[g] = 0.005f;
            }
            if (frequencyBand[g] < _BandBuffer[g])
            {
                _BandBuffer[g] -= frequencyBand[g];
                BufferDecrease[g] *= 1.2f;
            }
        }

    }
}
