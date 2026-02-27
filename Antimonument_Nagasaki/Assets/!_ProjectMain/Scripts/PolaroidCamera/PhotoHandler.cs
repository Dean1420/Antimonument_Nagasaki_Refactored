


using UnityEngine;
using System.Collections;
using Ftp;
using LocalStorageOperations;
using System.Collections.Generic;
using System;

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
        StartCoroutine(SpawnPolaroid());
        Debug.Log("POLAROID >>> successfully rendered and spawned");

        //UploadPolaroid();
        Debug.Log($"POLAROID >>> successfully uploaded");

    }



    private void RenderCurrentImageOnPolaroid()
    {
        Transform quadTransform = polaroid.transform.Find("Quad");
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



    private IEnumerator SpawnPolaroid()
    {
        yield return SpawnEffects();
        MovePolaroidToCamera();
        yield return PolaroidSpawnAnimation();

    }



    private IEnumerator PolaroidSpawnAnimation()
    {
        polaroid.transform.Rotate(90f, 90f, 0f);
        Vector3 stepDistance = -1 * polaroidSpawnPosition.forward * 0.015f;
        float stepDelay = 0.1f;
        for (int i = 0; i < 10; i++)
        {
            polaroid.transform.localPosition += stepDistance;
            yield return new WaitForSeconds(stepDelay);
        }
    }



    private void MovePolaroidToCamera()
    {
        polaroid.transform.position = polaroidSpawnPosition.position;
        polaroid.transform.rotation = polaroidSpawnPosition.rotation;
    }



    private IEnumerator SpawnEffects()
    {
        cameraShutter.Play();

        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        cameraFlash.SetActive(false);
    }

    void UploadPolaroid()
    {
        LocalStorageOperations.Image.SaveTextureAsJpg(currentImage, "!_ProjectMain/Scripts/PolaroidCamera/Images/", "Test.jpg");
        
        byte[] currentImageJpg = currentImage.EncodeToJPG();
        string fileType = ".jpg";

        Dictionary<string, string> credentials = LoadCredentials();

        string timestamp = DateTime.Now.ToString("yyyy.MM.dd_HH.mm");
        string filename = "file_";
        Ftp.FtpHandler.uploadFile(
            credentials["username"],
            credentials["password"],
            credentials["url"],
            credentials["remoteDirectory"],
            timestamp + filename + fileType,
            currentImageJpg);

    }



    private Dictionary<string, string> LoadCredentials()
    {
        string pathToCredentials = "!_ProjectMain/Scripts/PolaroidCamera/credentials.txt";
        string separator = ":";
        return TextFile.LoadLinesByKeyValue(pathToCredentials, separator);
    }

}
