using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChromaticEffect : MonoBehaviour
{
    private Volume volume;
    private float decreaseSpeed = 10f;

    void Awake()
    {
        volume = GetComponent<Volume>();
    }

    void Start()
    {
        CinemachineShake.Instance.OnShake += PlayEffectOnShake;
    }

    void Update()
    {
        if (volume.weight > 0)
        {
            volume.weight -= decreaseSpeed * Time.deltaTime;
        }
    }

    private void PlayEffectOnShake(float intensity)
    {
        volume.weight = intensity;
    }
}
