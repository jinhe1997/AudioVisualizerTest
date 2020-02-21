using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualization : MonoBehaviour
{
    public AudioSource _audioSource;

    //麥克風輸入
    public bool usemicrophone;
    public AudioClip m_audioclip;

    public string SelectedDevice;

    public static float[] _samples = new float[512];
    public void Start()
    {
        if (usemicrophone)
        {
            if (Microphone.devices.Length > 0)
            {
                SelectedDevice = Microphone.devices[0].ToString();
                _audioSource.clip = Microphone.Start(SelectedDevice, true, 10, AudioSettings.outputSampleRate);
            }
            else
            {
                usemicrophone = false;
            }

        }
        if (!usemicrophone)
        {
            _audioSource.clip = m_audioclip;
        }
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GetSpectrumAudioSource();
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }
}
