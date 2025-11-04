using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpraySound : MonoBehaviour
{
    //[SerializeField] WebTrigger trigger;
    [SerializeField] SoundManager soundManager;
    [SerializeField] TexturePainter texturePainter;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] GrabColor grabColor;
    bool soundOn;

    // Update is called once per frame
    void Update()
    {

        if (!soundOn && texturePainter.mode == Painter_BrushMode.PAINT)
        {
            if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
            {
                soundOn = true;
                soundManager.PlaySpraySound(true);
                var main = particleSystem.main;
                main.startColor = grabColor.board.color;
                particleSystem.Play();
            }
        }
        else if (soundOn && texturePainter.mode == Painter_BrushMode.PAINT)
        {
            if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
            {
                soundOn = false;
                soundManager.PlaySpraySound(false);
                particleSystem.Stop();
            }
        }

        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && texturePainter.mode == Painter_BrushMode.DECAL)
        {
            soundManager.PlayStickerSound();
        }

    }
}
