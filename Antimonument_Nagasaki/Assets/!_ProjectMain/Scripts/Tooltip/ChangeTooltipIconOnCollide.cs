using UnityEngine;
using UnityEngine.UI;

public class ChangeTooltipOnCollide : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   [SerializeField] private Transform tooltip;
    [SerializeField] private Sprite replacementSprite;
    [SerializeField] private bool replacementState;

    private Sprite originalSprite;
    private bool originalState;
    private Image tooltipImage;

    void Start()
    {
        tooltipImage = tooltip.GetComponentInChildren<Image>();
        originalSprite = tooltipImage.sprite;
        originalState = tooltip.gameObject.activeSelf;
    }

    void OnTriggerEnter(Collider other)
    {
        tooltipImage.sprite = replacementSprite;
        tooltip.gameObject.SetActive(replacementState);
        Debug.Log("TOOLTIP >>> " + originalSprite.name + " changed to " + replacementSprite.name);
    }

    void OnTriggerExit(Collider other)
    {
        tooltipImage.sprite = originalSprite;
        tooltip.gameObject.SetActive(originalState);
        Debug.Log("TOOLTIP >>> " + replacementSprite.name + " changed to " + originalSprite.name);
    }
}
