using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//The Script to Pickup Objects on the Workbench
public class GrabWorkbenchObject : MonoBehaviour
{
    [SerializeField] SoundManager soundManager;
    public TexturePainter TexPaint;

    //Variables to hold references to game objects in the scene
    GameObject lastobj;
    public GameObject GunPaint;
    public GameObject GunPaintHand;
    public GameObject ColorPicker;
    public GameObject SwordTable;
    public GameObject SwordCutHand;


    public GameObject HandTable;

    public GameObject RHand;
    public GameObject LHand;

    BoxCollider Handbox;

    //Transformation positions
    Vector3 Handtf = new Vector3(-0.0186817646f, -0.0702131391f, 0.0399602056f);
    Vector3 Painttf = new Vector3(0.0260000005f, -0.0900000036f, 0.170000002f);
    Vector3 Swordtf;


    //string TagSchnecke = "CutSchnecke";
    //string TagLoewe = "CutLoewe";
    //string TagJueng = "CutJuengling";
    string TagGrab = "Interactable";
    string TagPaint = "Paintable";
    //string paintable = "Paintable";





    // Start is called before the first frame update
    void Start()
    {
        Handbox = this.GetComponent<BoxCollider>();
        Handbox.center = Handtf;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PickItUp()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        lastobj = other.gameObject;

        //Switch to Paintmode and activate/deactivate relevant objects
        if (lastobj == GunPaint)

        {
            GunPaint.SetActive(false);
            GunPaintHand.SetActive(true);
            ColorPicker.SetActive(true);

            SwordTable.SetActive(true);
            SwordCutHand.SetActive(false);

            Handbox.center = Painttf;
            TexPaint.Init();

            HandTable.SetActive(true);

            //Protest.SetActive(false);

            DontGrabIt(TagGrab);
            
            
            DrawIt(TagPaint);

            soundManager.PlayGrabSound();

            //DrawIt(TagSchnecke);
            //DrawIt(TagLoewe);
            //DrawIt(TagJueng);


        }

        //Switch to cut Mode
        if (lastobj == SwordTable)

        {
            

            SwordTable.SetActive(false);
            SwordCutHand.SetActive(true);


            GunPaint.SetActive(true);
            GunPaintHand.SetActive(false);
            ColorPicker.SetActive(false);

            

            HandTable.SetActive(true);
            //Protest.SetActive(false);

            DontGrabIt(TagGrab);
            CutIt(TagPaint);

            soundManager.PlayGrabSound();

            //CutIt(TagSchnecke);
            //CutIt(TagLoewe);
            //CutIt(TagJueng);
        }

        //Switch to Hand/Normal mode
        if (lastobj == HandTable)
        {
            RHand.SetActive(true);
            LHand.SetActive(true);

            HandTable.SetActive(false);

            GunPaint.SetActive(true);
            GunPaintHand.SetActive(false);
            ColorPicker.SetActive(false);

            SwordTable.SetActive(true);
            SwordCutHand.SetActive(false);

            

            GrabIt(TagPaint);

            soundManager.PlayGrabSound();

            //GrabIt(TagSchnecke);
            //GrabIt(TagLoewe);
            //GrabIt(TagJueng);
            //GrabIt(paintable);
        }
    }


    //Methods to give specific tags


    //Make the Statue Sliceable with Physical attributes (gravity)
    public void CutIt(string TagString)
    {

        GameObject[] ObjectCut = GameObject.FindGameObjectsWithTag(TagString);
        for (int i = 0; i < ObjectCut.Length; i++)
        {
            if (ObjectCut[i].GetComponent<Rigidbody>() != null)
            {

                ObjectCut[i].GetComponent<MeshCollider>().convex = true;
                ObjectCut[i].GetComponent<Rigidbody>().isKinematic = false;
                ObjectCut[i].GetComponent<Rigidbody>().drag = 1;
                ObjectCut[i].GetComponent<Rigidbody>().angularDrag = 1;


                ObjectCut[i].layer = LayerMask.NameToLayer("Sliceable");

                if(ObjectCut[i].GetComponent<OVRGrabbable>() != null)
                Destroy(ObjectCut[i].GetComponent<OVRGrabbable>());

            }

            }
        }

    //Make the Statue Paintable without Convex and with Kinematics
        public void DrawIt(string TagString)
    {
        GameObject[] ObjectDraw = GameObject.FindGameObjectsWithTag(TagString);
        for (int i = 0; i < ObjectDraw.Length; i++)
        {
            if (ObjectDraw[i].GetComponent<Rigidbody>() != null)
            {

                ObjectDraw[i].GetComponent<MeshCollider>().convex = false;
                ObjectDraw[i].GetComponent<Rigidbody>().isKinematic = true;
                ObjectDraw[i].GetComponent<Rigidbody>().drag = 1;
                ObjectDraw[i].GetComponent<Rigidbody>().angularDrag = 1;

                ObjectDraw[i].layer = LayerMask.NameToLayer("Sliceable");

                if(ObjectDraw[i].GetComponent<OVRGrabbable>() != null)
                Destroy(ObjectDraw[i].GetComponent<OVRGrabbable>());

            }

        }
    }

    ////Make the Statue Grabable with floating Physics
    public void GrabIt(string TagString)
    {
        GameObject[] ObjectGrab = GameObject.FindGameObjectsWithTag(TagString);
        for (int i = 0; i < ObjectGrab.Length; i++)
        {
            if (ObjectGrab[i].name == "SlicedUp" || ObjectGrab[i].name == "SlicedLow")
            {
                ObjectGrab[i].tag = "Interactable";
                //soundManager.PlayGrabSound();

                if (ObjectGrab[i].GetComponent<Rigidbody>() != null)
                {
                    ObjectGrab[i].GetComponent<MeshCollider>().convex = true;
                    ObjectGrab[i].GetComponent<Rigidbody>().isKinematic = false;
                    ObjectGrab[i].GetComponent<Rigidbody>().drag = float.PositiveInfinity;
                    ObjectGrab[i].GetComponent<Rigidbody>().angularDrag = float.PositiveInfinity;

                    ObjectGrab[i].gameObject.AddComponent<OVRGrabbable>();
                    ObjectGrab[i].gameObject.GetComponent<OVRGrabbable>().enabled = true;
                    ObjectGrab[i].gameObject.GetComponent<OVRGrabbable>().Initialize(ObjectGrab[i].GetComponent<Collider>());
                    
                }
            }

        }
    }

    //////Make the Statue Paintable again
    public void DontGrabIt(string TagString)
    {
        GameObject[] ObjectDontGrab = GameObject.FindGameObjectsWithTag(TagString);
        for (int i = 0; i < ObjectDontGrab.Length; i++)
        {
            if (ObjectDontGrab[i].name == "SlicedUp" || ObjectDontGrab[i].name == "SlicedLow")
            {
                ObjectDontGrab[i].tag = "Paintable";

                if(ObjectDontGrab[i].GetComponent<OVRGrabbable>() != null)
                Destroy(ObjectDontGrab[i].GetComponent<OVRGrabbable>());


            }

        }
    }





}
