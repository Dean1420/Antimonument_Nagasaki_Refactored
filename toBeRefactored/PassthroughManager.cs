using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class PassthroughManager : MonoBehaviour
{
    public OVRPassthroughLayer passthrough;
    public OVRInput.Button button;
    public OVRInput.Controller controller;
    public Material skyboxStart;
    public Material skyboxPano;
    public GameObject ground; 

    // Start is called before the first frame update
    // Update is called once per frame

    private void Start()
    {
        //Zu Beginn kein Passthrough 
        Debug.Log(passthrough.hidden);
        passthrough.hidden = !passthrough.hidden;
    }

    void Update()
    {
        if (OVRInput.GetDown(button, controller))
        {
            ChangePassthrough();
        }

    }

    private void ChangePassthrough()
    {
        bool OnOff = !passthrough.hidden;
        passthrough.hidden = OnOff;
        
        if(OnOff)
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
