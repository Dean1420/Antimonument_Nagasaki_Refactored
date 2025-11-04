using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System;


public class ScreenShotController : MonoBehaviour
{
    byte[] imageData;
    string newFileName;
    bool takingScreenshot;
    public Camera cam;
    string filePath;
    //public ExportGLBManager GLBExporter;
    public RenderTexture renderTexture;
    public Texture2D tex2d;

    public GameObject test;

    public bool testBool = false;
    void Start()
    {
        tex2d = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
    }

    void Update()
    {
        /*if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
        {
            SaveTexture();
        }*/

        /*if (Input.GetKeyDown("space"))
        {
            SaveTexture();
        }*/
    }

    public IEnumerator TakeCapture()
    {
        yield return new WaitForEndOfFrame();

        Debug.Log("take capture");

        string fileName = "MrArt_" + ".jpg";
        string filePath = Path.Combine(Application.persistentDataPath + "/");
        string currentTime = System.DateTime.Now.ToString();
        currentTime = currentTime.Replace("/", "-");
        currentTime = currentTime.Replace(":", "-");
        newFileName = fileName + currentTime + ".jpg";

        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        imageData = tex.EncodeToJPG();

        // Wait for 3 seconds
        StartCoroutine("DelayedUploadScreenshot");
    }


    public void SaveTexture()
    {
        RenderTexture mRt = new RenderTexture(renderTexture.width, renderTexture.height, renderTexture.depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        mRt.antiAliasing = renderTexture.antiAliasing;
        Texture2D tex = new Texture2D(mRt.width, mRt.height, TextureFormat.ARGB32, false);
        // ReadPixels looks at the active RenderTexture.
        cam.targetTexture = mRt;
        cam.Render();
        RenderTexture.active = mRt;

        tex.ReadPixels(new Rect(0, 0, mRt.width, mRt.height), 0, 0);
        RenderTexture.active = null;
        tex.Apply();

        //string fullPath = "C:\\Users\\soren\\OneDrive\\Desktop\\SavedTextures\\";
        //string fileNameNeu = GLBExporter.GenerateUniqueFileName("");
        //Debug.Log("testing filename genaration" + GLBExporter.parentObject.name);
        //string fileName = "MrArt_" + ".png";
        if (!Application.isEditor)
            filePath = Path.Combine(Application.persistentDataPath + "/");
        else
            filePath = System.IO.Directory.GetCurrentDirectory() + "\\Assets\\PhotosTaken\\";
        
        DateTime currentDateTime = DateTime.Now;
        string formattedDateTime = currentDateTime.ToString("yyyy.MM.dd_HH.mm");
        /*string currentTime = System.DateTime.Now.ToString();
        currentTime = currentTime.Replace("/", "-");
        currentTime = currentTime.Replace(":", "-");*/
        newFileName = "file_" + formattedDateTime + ".jpg";

        if (!System.IO.Directory.Exists(filePath))
            System.IO.Directory.CreateDirectory(filePath);


        imageData = tex.EncodeToJPG();

        StartCoroutine("DelayedUploadScreenshot");

        if (Application.isEditor)
        {
            System.IO.File.WriteAllBytes(filePath + newFileName, imageData);
            Debug.Log("<color=orange>Saved Successfully!</color>" + filePath + newFileName);
        }


        DestroyImmediate(tex);
        cam.targetTexture = renderTexture;
        cam.Render();
        RenderTexture.active = renderTexture;
        DestroyImmediate(mRt);
        //GLBExporter.DoUpload();
        Debug.Log("DoUpload");
    }
    
    IEnumerator DelayedUploadScreenshot()
    {
        yield return new WaitForSeconds(3f);

        Task uploadTask = UploadScreenshot();

        yield return new WaitUntil(() => uploadTask.IsCompleted); // Wait for the upload task to complete

        if (uploadTask.Exception != null)
        {
            Debug.LogError("Screenshot upload failed: " + uploadTask.Exception.Message); 
        }
        else
        {
            Debug.Log("Screenshot uploaded successfully");
        }
    }

    public async Task UploadScreenshot()
    {
        //refresh token: 2v-IpchGczUAAAAAAAAAAQiqDmGznqpmoHZj-PHA30VYbjhRP6qtSUkf0mUMXCUL
        /*using (var dbx = new DropboxClient("2v-IpchGczUAAAAAAAAAAQiqDmGznqpmoHZj-PHA30VYbjhRP6qtSUkf0mUMXCUL", "82e2u9vdkwo9opy"))
        {
            var fileName = Path.GetFileName(newFileName);
            byte[] fileContent = imageData;
            Debug.Log("dropbox upload");

            var uploadResult = await dbx.Files.UploadAsync(
                "/" + fileName,
                WriteMode.Overwrite.Instance,
                body: new MemoryStream(fileContent));
        }*/

        //Upload to FTP-Server
        string username = "mrart2025-upload";
        string password = "xirxys-5viqxa-qyfcAc";
        string ftpServerUrl = "ftp://vsvr.hosting.medien.hs-duesseldorf.de";
        string remoteDirectory = "";
        try
         {
            // Erstelle FTP-Anmeldeinformationen
            NetworkCredential credentials = new NetworkCredential(username, password);
 
            // Erstelle FTP-Anfrage
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServerUrl + "/" + remoteDirectory + "/" + Path.GetFileName(newFileName));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = credentials;
            request.EnableSsl = true;
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;
            
            byte[] fileContent = imageData;
            Stream ftpStream = request.GetRequestStream();
            ftpStream.Write(fileContent, 0, fileContent.Length);
            ftpStream.Close();
            Debug.Log("imagedata written in bytes " + fileContent.Length); 
 
            // Erhalte die Serverantwort
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Debug.Log("Upload abgeschlossen. Serverantwort: " + response.StatusDescription);
            response.Close();
         }
         catch (Exception e)
         {
            Debug.Log("Fehler beim Upload: " + e.Message);
         }
    }
}