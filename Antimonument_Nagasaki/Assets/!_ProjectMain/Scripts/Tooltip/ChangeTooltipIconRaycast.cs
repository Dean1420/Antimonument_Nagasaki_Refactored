using UnityEngine;
using UnityEngine.UI;

public class ChangeTooltipIconRaycast : MonoBehaviour
{
    public Transform tooltip;
    public Sprite replacementSprite;
    public bool replacementState;
    public Transform[] targetObjects;
    public Transform rayDirection;
    public float maxDistance = 100f;

    private Sprite originalSprite;
    private bool originalState;
    private Image tooltipImage;
    private bool active = false;
    private bool replacedInPreviousIteration = false;

    void Update()
    {
        if (active)
        {
            UpdateTooltip();
        }
    }

    private void UpdateTooltip()
    {
        Vector3 origin = rayDirection.position;
        Vector3 direction = rayDirection.forward;
        Ray ray = new Ray(origin, direction);


        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            ReplaceTooltipOrDefault(hit);
        }
        else
        {
            Reset();
            replacedInPreviousIteration = false;
        }
    }

    private void ReplaceTooltipOrDefault(RaycastHit hit)
    {
        bool replaceTooltip = HitReplaceTrigger(hit);

        if (replacedInPreviousIteration != replaceTooltip)
        {
            AdjustTooltip(replaceTooltip);
            replacedInPreviousIteration = replaceTooltip;
        }
    }

    private bool HitReplaceTrigger(RaycastHit hit)
    {
        foreach (Transform target in targetObjects)
        {
            if (target.transform.name == hit.collider.gameObject.name)
            {
                return true;
            }
        }

        return false;
    }

    private void AdjustTooltip(bool replaceTooltip)
    {
        if (replaceTooltip)
        {
            Replace();
        }
        else
        {
            Reset();
        }
    }

    void Start()
    {
        tooltipImage = tooltip.GetComponentInChildren<Image>();
        originalSprite = tooltipImage.sprite;
    }

    public void Replace()
    {
        tooltipImage.sprite = replacementSprite;
        tooltip.gameObject.SetActive(replacementState);
        // Debug.Log("TOOLTIP >>> " + originalSprite.name + " changed to " + replacementSprite.name);
    }

    public void Reset()
    {
        tooltipImage.sprite = originalSprite;
        tooltip.gameObject.SetActive(originalState);
        //  Debug.Log("TOOLTIP >>> " + replacementSprite.name + " changed to " + originalSprite.name);
    }

    public void ToggleRaycast()
    {
        originalState = tooltip.gameObject.activeSelf;
        active = true;
    }
}
