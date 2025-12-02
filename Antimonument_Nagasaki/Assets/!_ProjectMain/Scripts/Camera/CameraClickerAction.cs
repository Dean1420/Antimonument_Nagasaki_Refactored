using UnityEngine;

public class CameraClickerAction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
    }
}
