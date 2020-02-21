using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualization : MonoBehaviour
{
    public AudioSource _audioSource;

    //麥克風輸入
    public bool usemicrophone;
    public AudioClip m_audioclip;
    public string SelectedDevice;

    public AudioMixerGroup _mixerGroupMicrophone, _mixerGroupMaster;

    public static float[] _samples = new float[512];
    public void Start()
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
                Debug.Log("microphone is null");
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
        if (_audioSource.clip != null)
        {
            GetSpectrumAudioSource();
        }
        if(_audioSource.clip = null)
        {
            Debug.Log("_audioSource.clip = null");
        }

        Debug.Log(Microphone.IsRecording(SelectedDevice).ToString());
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }
}
