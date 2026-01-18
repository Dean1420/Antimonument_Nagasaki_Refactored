using UnityEngine;
using UnityGLTF;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

public class ExportFromArea : MonoBehaviour
{
    [SerializeField] private Transform boundingBox;

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
            if (col.transform != boundingBox)
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
}