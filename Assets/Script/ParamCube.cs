using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    public int _band;
    public float _startScale, _scaleMultiplier;
    public bool UseBuffer;


    void Update()
    {
        if (UseBuffer)
        {
            transform.localScale = new Vector3((AudioVisualization._BandBuffer[_band] * _scaleMultiplier) + _startScale, (AudioVisualization._BandBuffer[_band] * _scaleMultiplier) + _startScale, (AudioVisualization._BandBuffer[_band] * _scaleMultiplier) + _startScale);
        }
        if (!UseBuffer)
        {
            transform.localScale = new Vector3(Mathf.Abs(AudioVisualization.frequencyBand[_band] * _scaleMultiplier) * 0.01f + _startScale, Mathf.Abs(AudioVisualization.frequencyBand[_band] * _scaleMultiplier) * 0.01f, Mathf.Abs(AudioVisualization.frequencyBand[_band] * _scaleMultiplier) * 0.01f);

            // transform.localScale = new Vector3((AudioVisualization.frequencyBand[_band] * _scaleMultiplier) + _startScale, (AudioVisualization.frequencyBand[_band] * _scaleMultiplier) + _startScale, (AudioVisualization.frequencyBand[_band] * _scaleMultiplier) + _startScale);
        }
    }
}
