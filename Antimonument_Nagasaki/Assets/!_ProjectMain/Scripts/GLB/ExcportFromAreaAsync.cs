using System.Collections; // -----Change here
using UnityEngine;
using UnityGLTF;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
public class ExcportFromAreaAsync : MonoBehaviour
{



    [SerializeField] private Transform boundingBox;

    public void Export()
    {
        StartCoroutine(ExportCoroutine());
    }

    private IEnumerator ExportCoroutine()
    {
        yield return null;

        if (boundingBox == null)
        {
            Debug.LogError("GLB >>> Bounding box not assigned!");
            yield break;
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
            yield break;
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

       //Run export on background thread
        yield return new WaitForEndOfFrame();
        System.Threading.Tasks.Task.Run(() =>
        {
            GLTFSceneExporter exporter = new GLTFSceneExporter(new Transform[] { tempRoot.transform }, new ExportContext());
            exporter.SaveGLB(fullPath, "export_area");
        }).Wait();

        // Restore original parents
        for (int i = 0; i < objectsInBox.Count; i++)
        {
            objectsInBox[i].SetParent(originalParents[i], true);
        }

        DestroyImmediate(tempRoot);

        Debug.Log("GLB >>> Exported to: " + fullPath + "/export_area.glb");
    }

}
