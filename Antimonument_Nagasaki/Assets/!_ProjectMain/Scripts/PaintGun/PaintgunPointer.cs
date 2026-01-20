using UnityEngine;

public class PaintgunPointer : MonoBehaviour
{
    public Transform pointer;
    public Transform pointerDirection;
    public float maxDistance = 100f;
    public float pointerSize = 1f;


    private bool pointerIsActive = false;

    void Start()
    {
        pointer.localScale = Vector3.one * pointerSize;
    }

    void Update()
    {
        if (pointerIsActive)
        {
            RepositionPointer();
        }
    }

    private void RepositionPointer()
    {
        Vector3 origin = pointerDirection.position;
        Vector3 direction = pointerDirection.forward;
        Ray ray = new Ray(origin, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            pointer.position = hit.point;
            pointer.rotation = Quaternion.LookRotation(hit.normal);
        }
    }

    public void TogglePointer()
    {
        pointerIsActive = !pointerIsActive;
        pointer.gameObject.SetActive(pointerIsActive);
    }
}
