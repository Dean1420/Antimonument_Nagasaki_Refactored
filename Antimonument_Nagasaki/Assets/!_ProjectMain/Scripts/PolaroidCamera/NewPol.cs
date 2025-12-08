using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPol : MonoBehaviour
{
    public GameObject polParent;
    Material newMat;
    public GameObject texture;

    bool notOut = false;
    // Start is called before the first frame update
    void Start()
    { 
       polParent = GameObject.Find("PolCam/Camera_Canon/Polaroids");
       texture = GameObject.Find("Polaroid/Quad");
       newMat = new Material(Shader.Find("Standard"));
       this.transform.SetParent(polParent.transform);
       this.transform.localPosition = new Vector3(0,0,0);
       this.transform.localEulerAngles = new Vector3 (0,0,90);
       this.transform.GetChild(0).GetComponent<MeshRenderer>().material = newMat;
       this.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = texture.GetComponent<MeshRenderer>().material.mainTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (transform.localPosition.x >= -0.18 && !notOut)
            {
                transform.localPosition += new Vector3 (-0.0015f, 0,0);
                this.transform.localEulerAngles = new Vector3 (0,0,90);
            }
        else
        notOut = true;
       
        

    }
}
