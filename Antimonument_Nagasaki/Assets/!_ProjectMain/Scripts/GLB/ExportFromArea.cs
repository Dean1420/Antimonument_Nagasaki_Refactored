using UnityEngine;
using UnityGLTF;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
using LocalStorageOperations;

public class ExportFromArea : MonoBehaviour
{
    [SerializeField] private Transform boundingBox;
    [SerializeField] private Transform[] ignoredObjects;

    public void Export()
    {
        if (boundingBox == null)
        {
            Debug.LogError("GLB >>> Bounding box not assigned!");
            return;
        }

        Vector3 halfExtents = boundingBox.lossyScale * 0.5f;

        Collider[] hitColliders = Physics.OverlapBox(
            boundingBox.position,
            halfExtents,
            boundingBox.rotation
        );

        List<Transform> objectsInBox = new List<Transform>();
        List<Transform> originalParents = new List<Transform>();

        foreach (Collider col in hitColliders)
        {
            if (col.transform == boundingBox) continue;

            bool isIgnored = System.Array.Exists(ignoredObjects, ignored => ignored == col.transform);
            if (isIgnored) continue;

            // Check if the object's actual position is inside the box
            Vector3 localPos = boundingBox.InverseTransformPoint(col.transform.position);
            bool isInsideBox = Mathf.Abs(localPos.x) <= 0.5f &&
                               Mathf.Abs(localPos.y) <= 0.5f &&
                               Mathf.Abs(localPos.z) <= 0.5f;

            if (isInsideBox)
            {
                objectsInBox.Add(col.transform);
                originalParents.Add(col.transform.parent);
            }
        }

        if (objectsInBox.Count == 0)
        {
            Debug.LogWarning("GLB >>> No objects found in bounding box!");
            return;
        }

        // Create temporary parent
        GameObject tempRoot = new GameObject("TempExportRoot");

        // Reparent objects
        foreach (Transform obj in objectsInBox)
        {
            obj.SetParent(tempRoot.transform, true);
        }

        Debug.Log($"GLB >>> Exporting {objectsInBox.Count} objects");

        string relativePath = "!_ProjectMain/Scripts/GLB/ExportedGLB";
        string fullPath = Path.Combine(Application.dataPath, relativePath);

        GLTFSceneExporter exporter = new GLTFSceneExporter(new Transform[] { tempRoot.transform }, new ExportContext());
        exporter.SaveGLB(fullPath, "export_area");

        // Restore original parents
        for (int i = 0; i < objectsInBox.Count; i++)
        {
            objectsInBox[i].SetParent(originalParents[i], true);
        }

        DestroyImmediate(tempRoot);

        Debug.Log("GLB >>> Exported to: " + fullPath + "/export_area.glb");
    }



    void UploadScene()
    {
        string relativePath = "!_ProjectMain/Scripts/GLB/ExportedGLB";
        string fullPath = Path.Combine(Application.dataPath, relativePath);
        string glbFilePath = Path.Combine(fullPath, "export_area.glb");

        if (!File.Exists(glbFilePath))
        {
            Debug.LogError("GLB >>> Export file not found at: " + glbFilePath);
            return;
        }

        byte[] glbBytes = File.ReadAllBytes(glbFilePath);
        string fileType = ".glb";

        Dictionary<string, string> credentials = LoadCredentials();

        string timestamp = System.DateTime.Now.ToString("yyyy.MM.dd_HH.mm");
        string filename = "scene_export_";

        Ftp.FtpHandler.uploadFile(
            credentials["username"],
            credentials["password"],
            credentials["url"],
            credentials["remoteDirectory"],
            timestamp + filename + fileType,
            glbBytes);

        Debug.Log("GLB >>> Uploaded to FTP: " + timestamp + filename + fileType);
    }



    private Dictionary<string, string> LoadCredentials()
    {
        string pathToCredentials = "!_ProjectMain/Scripts/PolaroidCamera/credentials.txt";
        string separator = ":";
        return TextFile.LoadLinesByKeyValue(pathToCredentials, separator);
    }
}