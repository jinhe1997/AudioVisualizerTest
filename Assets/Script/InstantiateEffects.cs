using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateEffects : MonoBehaviour
{
    public GameObject EffectPrefab;
    public GameObject[] sample_Effects = new GameObject[512];
    public float _MaxScale;

    void Start()
    {
        for (int i = 0; i < 512; i++)
        {
            GameObject _intantanceSampleEffect = (GameObject)Instantiate(EffectPrefab);
            _intantanceSampleEffect.transform.position = this.transform.position;
            _intantanceSampleEffect.transform.parent = this.transform;
            _intantanceSampleEffect.name = "SampleEffect" + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            _intantanceSampleEffect.transform.position = Vector3.forward * 100;
            sample_Effects[i] = _intantanceSampleEffect;
        }
    }

    void Update()
    {
        for (int i = 0; i < 512; i++)
        {
            if (sample_Effects != null)
            {
                sample_Effects[i].transform.localScale = new Vector3(10, (AudioVisualization._samples [i] * _MaxScale) +2, 10);
            }
        }
    }
}
