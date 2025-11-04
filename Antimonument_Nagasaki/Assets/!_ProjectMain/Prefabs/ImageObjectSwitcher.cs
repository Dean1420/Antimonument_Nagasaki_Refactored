using UnityEngine;
using UnityEngine.Rendering;

public class ImageObjectSwitcher : MonoBehaviour
{
    public GameObject[] objectsToActivate; // Assign 6 objects in the inspector
    private GameObject activeObject;
    public GameObject startobject;

    // Call this from button and pass index from 0 to 5

    private void Start()
    {
    }
    public void ActivateObject(int index)
    {
        if (index < 0 || index >= objectsToActivate.Length) return;

        // Disable previously active
        if (activeObject != null)
            activeObject.SetActive(false);

        // Enable new one
        activeObject = objectsToActivate[index];
        activeObject.SetActive(true);
    }
    public void btnstart()
    {

        startobject.SetActive(false);
    }
}