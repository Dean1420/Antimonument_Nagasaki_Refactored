using System;
using System.Runtime.CompilerServices;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Paint : MonoBehaviour
{
    public Color color;
    public float radius = 10;
    public float paintDistance = 100;
    public ParticleSystem particles;

    private Vector2 pixelUV;
    private Texture2D copyTexture;
    private Renderer targetRenderer;
    private RaycastHit raycastHit;

    void Start()
    {

    }

    void Update()
    {
        // render the paint pointer   
    }

    public void TogglePaintPointer()
    {

    }

    public void StartPainting()
    {
        EnableEffects();

        ShootRaycast();

        if (!CheckIfPaintable())
        {
            return;
        }

        CopyTexture();

        pixelUV = raycastHit.textureCoord;
        pixelUV.x *= copyTexture.width;
        pixelUV.y *= copyTexture.height;
        Debug.Log($"PAINT_GUN >>> painted at: ({pixelUV.x}, {pixelUV.y}) with color: {color}");

        PaintCircleOnTexture(pixelUV.x, pixelUV.y, radius, color);

        // Apply the changes and update the material
        copyTexture.Apply();
        targetRenderer.material.SetTexture("_MainTex", copyTexture);

    }


    private void CopyTexture()
    {
        targetRenderer = raycastHit.transform.GetComponent<Renderer>();

        Texture originalTexture = targetRenderer.material.GetTexture("_MainTex");

        copyTexture = new Texture2D(originalTexture.width, originalTexture.height);
        Graphics.CopyTexture(originalTexture as Texture2D, copyTexture);        
    }

    private void ShootRaycast()
    {
        float x = Screen.width / 2;
        float y = Screen.height / 2;
        float z = 0;
        Vector3 raycastDirection = new Vector3(x, y, z);
        Ray cameraRay = Camera.main.ScreenPointToRay(raycastDirection);

        if (!Physics.Raycast(cameraRay, out raycastHit, paintDistance))
        {
            Debug.Log("PAINT_GUN >>> hit: null");
            return;
        }
        Debug.Log($"PAINT_GUN >>> hit: {raycastHit.collider.gameObject.name}");
    }

    private void EnableEffects()
    {
        if (!particles.isPlaying)
        {
            particles.Play();
        }
    }


    private bool CheckIfPaintable()
    {
        if (raycastHit.collider.gameObject.name == "Ground")
        {
            return false;
        }

        return true;
    }


    private void PaintCircleOnTexture(float x, float y, float radius, Color c)
    {
        float area = radius * radius;

        for (int width = 0; width < copyTexture.width; width++)
        {
            for (int height = 0; height < copyTexture.height; height++)
            {
                // check if out of bounds
                if (x < 0 || x >= copyTexture.width || y < 0 || y >= copyTexture.height)
                {
                    continue;
                }
                //using circle equation to check if pixel is stil inside the circle to be painted
                float dx = x - width;
                float dy = y - height;
                float distanceSquared = dx * dx + dy * dy;

                bool isOnCircle = distanceSquared < area;
                if (isOnCircle)
                {
                    copyTexture.SetPixel(width, height, color);
                }
            }
        }
    }

    public void StopPainting()
    {
        particles.Stop();
    }
}
