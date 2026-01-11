using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

public class ChangeTooltip : MonoBehaviour
{
    [SerializeField] private string replacementText;
    [SerializeField] private Transform tooltip;
    private string originalText;
    private TextMeshProUGUI tooltipTextMesh;

    void Start()
    {
        tooltipTextMesh = tooltip.GetComponentInChildren<TextMeshProUGUI>();
        originalText = tooltipTextMesh.text;
    }

    public void replaceText()
    {
        tooltipTextMesh.text = replacementText;
        Debug.Log("TOOLTIP >>> " + originalText + "changed to " + replacementText);
    }

    public void resetTextToOriginal()
    {
        tooltipTextMesh.text = originalText;
        Debug.Log("TOOLTIP >>> " + replacementText + " changed to " + originalText);
    }
}
