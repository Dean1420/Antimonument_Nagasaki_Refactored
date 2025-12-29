using UnityEngine;
using UnityGLTF;
using GLTF.Schema;
using System.IO;

public class ExportGLB : MonoBehaviour
{
    public Transform[] sceneRoot;

    public async void Export()
    {
        /* string relativePath = "!_ProjectMain/Scripts/GLB/Exported";
        string fullPath = Path.Combine(Application.dataPath, relativePath);

        var exportOptions = new ExportContext();
        var exporter = new GLTFSceneExporter(sceneRoot, exportOptions);
        exporter.SaveGLTFandBin(fullPath, "export");
        
        Debug.Log("Exported to: " + fullPath); */

        string relativePath = "!_ProjectMain/Scripts/GLB/Exported";
        string fullPath = Path.Combine(Application.dataPath, relativePath);
        
       /*  var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        var sceneRoot = new Transform[rootObjects.Length];
        
        for (int i = 0; i < rootObjects.Length; i++)
        {
            sceneRoot[i] = rootObjects[i].transform;
        } */
        
        ExportContext exportOptions = new ExportContext();
        GLTFSceneExporter exporter = new GLTFSceneExporter(sceneRoot, exportOptions);
        exporter.SaveGLB(fullPath, "export");
        
        Debug.Log("GLB >>> Exported to: " + fullPath);
    }
}
