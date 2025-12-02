


using UnityEngine;
using System.Collections;


public class PhotoHandler : MonoBehaviour
{

    [Header("Photo Data Model")]
    [SerializeField] private GameObject polaroid;
    [SerializeField] private RenderTexture cameraView;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private Transform polaroidSpawnPosition;
    private Texture2D currentImage;

    [Header("Camera Effects")]
    [SerializeField] private GameObject cameraFlash;
    [SerializeField] private float flashTime;
    [SerializeField] private AudioSource cameraShutter;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }



    // Update is called once per frame
    void Update()
    {
    }



    public void CreatePolaroid()
    {
        UpdateCurrentImage();
        RenderCurrentImageOnPolaroid();
        Debug.Log("rendered");
        // camera effects

        // start polaraoid spawning
        //StartCoroutine(SpawnPolaroid());
    }



    private void RenderCurrentImageOnPolaroid()
    {
        Transform quadTransform = polaroid.transform.Find("Quad");
        Debug.Log(quadTransform);
        MeshRenderer renderer = quadTransform.GetComponent<MeshRenderer>();
        Material mat = new Material(Shader.Find("Unlit/Texture"));
        mat.mainTexture = currentImage;
        renderer.material = mat;
    }



    // get current image as texture from render texture
    private void UpdateCurrentImage()
    {
        RenderTexture.active = cameraView;
        currentImage = new Texture2D(
            cameraView.width,
            cameraView.height,
            TextureFormat.ARGB32,
            false
            );

        currentImage.ReadPixels(
            new Rect(0, 0, cameraView.width, cameraView.height),
            0,
            0
        );

        currentImage.Apply();
        RenderTexture.active = null;
    }

    IEnumerator SpawnPolaroid()
    {
        //cameraShutter.Play();
        //cameraFlash.SetActive(true);
        //yield return new WaitForSeconds(flashT);
        //cameraFlash.SetActive(false);
        //TakePolaroid();
        return null;
    }

    void UploadPolaroid()
    {
        // get current image displayed on polaroid

        // prepare image for ftp

        // get ftp credentials

        // upload image
    }

}
