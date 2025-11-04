using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class PassthroughManager : MonoBehaviour
{
    public InputActionProperty toggleButton;
    public ARCameraManager cameraManager;
    public Material skyboxStart;
    public Material skyboxPano;
    public GameObject ground;
    
    private bool passthroughEnabled = false;

    private void Start()
    {
        // Start without passthrough
        if (cameraManager != null)
        {
            cameraManager.enabled = false;
        }
        passthroughEnabled = false;
    }

    private void Update()
    {
        if (toggleButton.action.WasPressedThisFrame())
        {
            ChangePassthrough();
        }
    }

    private void ChangePassthrough()
    {
        passthroughEnabled = !passthroughEnabled;
        
        if (cameraManager != null)
        {
            cameraManager.enabled = passthroughEnabled;
        }
        
        if (passthroughEnabled)
        {
            RenderSettings.skybox = skyboxPano;
            ground.SetActive(true);
        }
        else
        {
            RenderSettings.skybox = null;
            ground.SetActive(false);
        }
    }

    public void NewStatue(Material skyboxMaterial)
    {
        skyboxPano = skyboxMaterial;
    }

    public void Reset()
    {
        skyboxPano = skyboxStart;
    }
}
