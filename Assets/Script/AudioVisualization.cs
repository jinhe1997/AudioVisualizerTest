using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualization : MonoBehaviour
{
    public AudioSource _audioSource;
    [SerializeField]
    public static float[] _samples = new float[512];
    public static float[] frequencyBand = new float[8];
    public static float[] _BandBuffer = new float[8];
    float[] BufferDecrease = new float[8];

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
    }

    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBand();


    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
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
