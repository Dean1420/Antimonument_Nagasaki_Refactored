using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpraySound : MonoBehaviour
{
    //[SerializeField] WebTrigger trigger;
    [SerializeField] SoundManager soundManager;
    [SerializeField] TexturePainter texturePainter;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] GrabColor grabColor;
    bool soundOn;

    public InputActionProperty triggerAction;

    // Update is called once per frame
    void Update()
    {

        if (!soundOn && texturePainter.mode == Painter_BrushMode.PAINT)
        {
            if (triggerAction.action.IsPressed())
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
            if (triggerAction.action.WasReleasedThisFrame())
            {
                soundOn = false;
                soundManager.PlaySpraySound(false);
                particleSystem.Stop();
            }
        }

        if (triggerAction.action.WasPressedThisFrame() && texturePainter.mode == Painter_BrushMode.DECAL)
        {
            soundManager.PlayStickerSound();
        }

    }
}
