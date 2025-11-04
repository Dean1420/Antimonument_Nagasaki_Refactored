using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Polaroid : MonoBehaviour
{

    public RenderTexture renderTexture;
    public Camera cam;
    public Texture2D tex2d;


    

    

    // Start is called before the first frame update
    void Start()
    {
        tex2d = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void PhotoTake()
    {
        StartCoroutine(MakePhoto());

    }

    public IEnumerator MakePhoto()
    {



        RenderTexture mRt = new RenderTexture(renderTexture.width, renderTexture.height, renderTexture.depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        mRt.antiAliasing = renderTexture.antiAliasing;
        tex2d = new Texture2D(mRt.width, mRt.height, TextureFormat.ARGB32, false);

        cam.targetTexture = mRt;
        cam.Render();
        RenderTexture.active = mRt;



        Rect regionToRead = new Rect(0, 0, mRt.width, mRt.height);
        tex2d.ReadPixels(regionToRead, 0, 0);
        RenderTexture.active = null;
        tex2d.Apply();

        this.GetComponent<MeshRenderer>().material.mainTexture = tex2d;

         cam.targetTexture = renderTexture;
        cam.Render();
        RenderTexture.active = renderTexture;
        DestroyImmediate(mRt);
        yield return new WaitForEndOfFrame();
    }
}
