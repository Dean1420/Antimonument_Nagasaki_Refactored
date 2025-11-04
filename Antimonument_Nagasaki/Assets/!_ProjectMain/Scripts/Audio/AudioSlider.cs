using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSlider : MonoBehaviour
{
    [SerializeField]
    GameObject sliderSphere;
    [SerializeField]
    AudioSource audioSource;

    // Update is called once per frame
    void Update()
    {
        float posX = sliderSphere.transform.localPosition.x;
        audioSource.volume = posX;
    }
}
