using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class GrabColor : MonoBehaviour
{
    public Material board;      // Testboard Color

    public Material matColor;  //Material Color from gun

    public Material matLine;   //Material Color from RayCastLine


    public Material HighlightMat;  //Highlight if you Hover over Restore/Save/Load
    public Material NormalMat;

    /*public TextMeshPro text1;       // Restore Text
    public TextMeshPro text2;       //Save Text
    public TextMeshPro text3;
    public TextMeshPro text4;
    public TextMeshPro text5;
    public TextMeshPro text6;*/

    public GameObject Reset;
    public GameObject Save;
    public GameObject Load;
    public GameObject Mode;
    public GameObject Next;
    public GameObject Back;

    public GameObject ColorPicker;
    public GameObject StickerPicker;

    

    public int decalcounter;

    public bool canPaint = true;    //Paintable or not
    public bool resetPaint = false;  //Reset Painting
    public bool savePaint = false;  //save Painting
    public bool loadSave = false;   //load last save;
    public bool decalMode = false;
    

    public Texture2D textureToPickFrom;  // ColorPicker Texture
    
    

    /*[SerializeField] WebGrip GripTrigger;    //Controller GripTrigger 
    [SerializeField] WebGrip GripTriggerB;    //Controller GripTrigger 
    [SerializeField] WebTrigger TriggerTrigger;    // Controller Trigger
    [SerializeField] WebFloatX AxisTrigger;
    [SerializeField] WebFloatX RotateTrigger;*/

    [SerializeField] TexturePainter TexPaint;
    public int counterStat = 0;

    //[SerializeField] WebThumb ThumbButton;

    Vector3 rotateint = new Vector3 (0, 0 , 1);

    public LineRenderer laserLineRenderer;    //Raycastlinerenderer
    public float laserMaxLength = 5f;

    
    public float SizeValue = 0.3f;          // for the BrushSize



    public InputActionProperty triggerAction;
    public InputActionProperty handTriggerRightAction;
    public InputActionProperty handTriggerLeftAction;



    // Start is called before the first frame update
    void Start()
    {
        board.color = Color.red;
        matColor.color = board.color;
        matLine.color = board.color;

        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        laserLineRenderer.startWidth = 0.01f;            //Start Width from line
        laserLineRenderer.endWidth = 0.001f;                //End Width from line
                                                            //laserLineRenderer.startColor = Color.red;           
                                                            //laserLineRenderer.endColor = Color.green;
                                                            //laserLineRenderer.startColor = matColor.color;


        


    }

    // Update is called once per frame
    void Update()
    {
        RayCastPicker();

        /*if (ThumbButton.controller.GetButtonDown(WebXR.WebXRController.ButtonTypes.ButtonA))
        {
            counterStat++;

            if (counterStat > 2)
                counterStat = 0;

            if (counterStat == 0)
            {
                TexPaint.restoreMaterial();
                TexPaint.L�weStatue.SetActive(true);
                TexPaint.SchneckeStatue.SetActive(false);
                TexPaint.JuenglingStatue.SetActive(false);

                TexPaint.statue = TexPaint.L�weStatue;
                TexPaint.tex = TexPaint.L�weTex;
                TexPaint.tex2 = TexPaint.L�weTexTemp;

                RenderSettings.skybox = TexPaint.SkyBoxL�we;

            }

            if (counterStat == 1)
            {
                TexPaint.restoreMaterial();
                TexPaint.L�weStatue.SetActive(false);
                TexPaint.SchneckeStatue.SetActive(true);
                TexPaint.JuenglingStatue.SetActive(false);

                TexPaint.statue = TexPaint.SchneckeStatue;
                TexPaint.tex = TexPaint.SchneckeTex;
                TexPaint.tex2 = TexPaint.SchneckeTexTemp;

                RenderSettings.skybox = TexPaint.SkyBoxSchnecke;
            }

            if (counterStat == 2)
            {
                TexPaint.restoreMaterial();
                TexPaint.L�weStatue.SetActive(false);
                TexPaint.SchneckeStatue.SetActive(false);
                TexPaint.JuenglingStatue.SetActive(true);

                TexPaint.statue = TexPaint.JuenglingStatue;
                TexPaint.tex = TexPaint.JuenglingTex;
                TexPaint.tex2 = TexPaint.JuenglingTexTemp;
            }

            //statue.GetComponent<MeshRenderer>().material = baseMaterial;
            TexPaint.statue.GetComponent<MeshRenderer>().material.mainTexture = TexPaint.canvasTexture;
            //rendTex.GetComponent<MeshRenderer>().material = baseMaterial;
            TexPaint.rendTex.GetComponent<MeshRenderer>().material.mainTexture = TexPaint.tex;

            Debug.Log(counterStat);
        }*/
    }


    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        Vector3 endPosition = targetPosition + (length * direction);

        if (Physics.Raycast(ray, out raycastHit, length))
        {
            endPosition = raycastHit.point;
        }

        laserLineRenderer.SetPosition(0, targetPosition);
        laserLineRenderer.SetPosition(1, endPosition);
    }


    void RayCastPicker()
    {
        var ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.tag == "Painting" || hit.collider.tag == "Reset" || hit.collider.tag == "Save"  || hit.collider.tag == "Load"  || hit.collider.tag == "Mode" || hit.collider.tag == "Next" || hit.collider.tag == "Back")
            {
                canPaint = false;       //Dont Paint if these Collider
                ShootLaserFromTargetPosition(transform.position, this.transform.forward, laserMaxLength);       //ShootLaser if these Collider
                laserLineRenderer.enabled = true;

            }
            else
            {
                laserLineRenderer.enabled = false;
                canPaint = true;
            }

            if (hit.collider.tag == "Reset" || hit.collider.tag == "Save" || hit.collider.tag == "Load" || hit.collider.tag == "Mode" || hit.collider.tag == "Next" || hit.collider.tag == "Back")
            {
                matLine.color = Color.white;      //Change Color of Laser if these Colliders
            }
            else
            {
                matLine.color = matColor.color;
            }


            if (hit.collider.tag == "Reset")
            {
                Reset.gameObject.GetComponent<Renderer>().material = HighlightMat;  //Highlight
            }
            else
            {
                Reset.gameObject.GetComponent<Renderer>().material = NormalMat;
                Reset.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.white;  //Change Text back to white

            }

            if (hit.collider.tag == "Save")
            {
                Save.gameObject.GetComponent<Renderer>().material = HighlightMat;
                TexPaint.brushCursor.SetActive(false);
            }
            else
            {
                Save.gameObject.GetComponent<Renderer>().material = NormalMat;
                Save.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.white;
                TexPaint.brushCursor.SetActive(true);
                //text2.color = Color.white;
            }

            if (hit.collider.tag == "Load")
            {
                Load.gameObject.GetComponent<Renderer>().material = HighlightMat;
            }
            else
            {
                Load.gameObject.GetComponent<Renderer>().material = NormalMat;
                Load.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.white;
            }

            if (hit.collider.tag == "Mode")
            {
                Mode.gameObject.GetComponent<Renderer>().material = HighlightMat;
            }
            else
            {
                Mode.gameObject.GetComponent<Renderer>().material = NormalMat;
                Mode.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.white;
            }

            if (hit.collider.tag == "Next")
            {
                Next.gameObject.GetComponent<Renderer>().material = HighlightMat;
            }
            else
            {
                Next.gameObject.GetComponent<Renderer>().material = NormalMat;
                Next.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.white;
            }

            if (hit.collider.tag == "Back")
            {
                Back.gameObject.GetComponent<Renderer>().material = HighlightMat;
            }
            else
            {
                Back.gameObject.GetComponent<Renderer>().material = NormalMat;
                Back.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.white;
            }



            //ColorPicker
            /*if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
            {
                if (hit.collider.tag == "Painting")
                {
                    
                    Vector2 uv = hit.textureCoord;
                    uv.x *= textureToPickFrom.width;
                    uv.y *= textureToPickFrom.height;

                    //Debug.Log("Hit point: " + hit.point);
                    //Debug.Log("UV coordinates: " + uv); // output the UV coordinates to the console

                    Color pixelColor = textureToPickFrom.GetPixel((int)uv.x, (int)uv.y);
                    board.color = pixelColor;
                    matColor.color = pixelColor;
                    matLine.color = pixelColor;
                    //laserLineRenderer.startColor = pixelColor;

                    
                    //Debug.Log("Pixel color: " + pixelColor); 
                }
              

            }*/

            //COLOR PICKER
            if (triggerAction.action.WasPressedThisFrame())
            {

                if (hit.collider.tag == "Painting")
                {

                    Vector2 uv = hit.textureCoord;
                    uv.x *= textureToPickFrom.width;
                    uv.y *= textureToPickFrom.height;

                    //Debug.Log("Hit point: " + hit.point);
                    //Debug.Log("UV coordinates: " + uv); // output the UV coordinates to the console

                    Color pixelColor = textureToPickFrom.GetPixel((int)uv.x, (int)uv.y);
                    board.color = pixelColor;
                    matColor.color = pixelColor;
                    matLine.color = pixelColor;
                    //laserLineRenderer.startColor = pixelColor;


                    //Debug.Log("Pixel color: " + pixelColor); 
                }

                //Resetbutton for Texture
                if (hit.collider.tag == "Reset")
                {
                    resetPaint = true;
                    Reset.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.red;
                }
                //Savebutton for Texture
                if (hit.collider.tag == "Save")
                {
                    TexPaint.SaveTexture2D();
                    TexPaint.statue.GetComponent<MeshRenderer>().material.mainTexture = TexPaint.canvasTexture;
                    // savePaint = true;
                    Save.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.red;
                }

                //Load button for Texture
                if (hit.collider.tag == "Load")
                {
                    loadSave = true;
                    Load.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.red;
                }

                //change Paintmode
                if (hit.collider.tag == "Mode" && !decalMode)
                {
                    decalMode = true;
                    Mode.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.red;
                    Mode.gameObject.GetComponentInChildren<TextMeshPro>().text = "Sticker Mode";

                    ColorPicker.SetActive(false);
                    StickerPicker.SetActive(true);

                    Next.gameObject.SetActive(true);
                    Back.gameObject.SetActive(true);

                    Debug.Log("Decal");
                }
                else
                {
                    if (hit.collider.tag == "Mode")
                    {
                        decalMode = false;
                        Mode.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.red;
                        Mode.gameObject.GetComponentInChildren<TextMeshPro>().text = "Paint Mode";

                        ColorPicker.SetActive(true);
                        StickerPicker.SetActive(false);

                        Next.gameObject.SetActive(false);
                        Back.gameObject.SetActive(false);

                        Debug.Log("Brush");
                    }
                }
                //Next Sticker
                if (hit.collider.tag == "Next")
                {
                    Next.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.red;
                    decalcounter++;

                    if (decalcounter == 6)
                        decalcounter = 0;

                    Debug.Log(decalcounter);

                }
                //Previous Sticker
                if (hit.collider.tag == "Back")
                {
                    Back.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.red;
                    decalcounter--;

                    if (decalcounter == -1)
                        decalcounter = 5;

                    Debug.Log(decalcounter);


                }

            }


            //SizeBrush and rotate
            if (hit.collider.tag == "Paintable" || hit.collider.tag == "CutLoewe" || hit.collider.tag == "CutSchnecke" || hit.collider.tag == "CutJuengling")
            {
                TexPaint.brushCursor.SetActive(true);

                if (handTriggerRightAction.action.IsPressed())
                {
                    if (SizeValue <= 0.6f)
                    {
                        SizeValue += 0.0025f;
                        TexPaint.brushSize = SizeValue;
                        TexPaint.brushCursor.transform.localScale = Vector3.one * TexPaint.brushSize;
                    }
                    else
                    {
                        SizeValue = 0.05f;
                    }

                }

                /*if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
                {

                    if (SizeValue >= 0.05f)
                    {
                        SizeValue -= 0.0025f;
                        TexPaint.brushSize = SizeValue;
                        TexPaint.brushCursor.transform.localScale = Vector3.one * TexPaint.brushSize;
                    }
                    else
                    {
                        SizeValue = 0.05f;
                    }
                }*/

                if (handTriggerLeftAction.action.IsPressed() && TexPaint.mode == Painter_BrushMode.DECAL)

                {
                    TexPaint.brushrotate += rotateint;
                    TexPaint.brushCursor.transform.localEulerAngles = TexPaint.brushrotate;

                    
                }

               /* if (GripTriggerB.controller.GetButton(WebXR.WebXRController.ButtonTypes.Grip))

                {
                    TexPaint.brushrotate -= rotateint;
                    TexPaint.brushCursor.transform.localEulerAngles = TexPaint.brushrotate;
                    
                }*/

            }
            else
            {
                TexPaint.brushCursor.SetActive(false);
            }


        }
    }



}
