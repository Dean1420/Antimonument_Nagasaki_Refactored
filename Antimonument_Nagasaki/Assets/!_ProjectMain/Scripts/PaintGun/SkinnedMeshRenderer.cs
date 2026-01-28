using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class SkinnedMeshCollider : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMesh;
    private Mesh bakedMesh;
    private MeshCollider meshCollider;
    private bool isPainting = false;
    private float updateInterval = 1f;
    private float lastUpdateTime = 0f;

    void Start()
    {
        skinnedMesh = GetComponent<SkinnedMeshRenderer>();
        bakedMesh = new Mesh();
        
        meshCollider = gameObject.GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }
    }

    void Update()
    {
        if (isPainting && Time.time - lastUpdateTime >= updateInterval)
        {
            skinnedMesh.BakeMesh(bakedMesh);
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = bakedMesh;
            lastUpdateTime = Time.time;
        }
    }

    public void StartPainting()
    {
        isPainting = true;
    }

    public void StopPainting()
    {
        isPainting = false;
    }
}