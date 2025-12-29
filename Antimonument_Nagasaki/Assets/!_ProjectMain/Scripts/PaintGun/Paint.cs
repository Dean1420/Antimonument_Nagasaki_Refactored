/* using System;
using System.Runtime.CompilerServices;
using NUnit.Framework.Constraints; */
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

    private bool continuePainting = false;

    void Start()
    {
    particles.Stop();
    }

    void Update()
    {
        if (continuePainting && (Time.frameCount % 15 == 0))
        {
            PaintOnRenderTexture();
        }
    }

    public void TogglePaintPointer()
    {

    }

    public void StartPainting()
    {
        EnableEffects();
        PaintOnRenderTexture();
        continuePainting = true;
    }

    private void PaintOnRenderTexture()
    {
        ShootRaycast();

        if (!CheckIfPaintable())
        {
        }
        ApplyPaint();
    }

    private void ApplyPaint()
    {
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

        Texture mainTexture = targetRenderer.material.mainTexture;

        Debug.Log($"PAINT_GUN >>> Hit object: {raycastHit.transform.name}");

        if (mainTexture == null)
        {
            mainTexture = targetRenderer.material.GetTexture("_BaseMap");
        }

        if (mainTexture == null)
        {
            Debug.LogError($"PAINT_GUN >>> No texture found");

            return;
        }

        if (mainTexture is RenderTexture renderTexture)
        {
            RenderTexture.active = renderTexture;
            copyTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            copyTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            copyTexture.Apply();
            RenderTexture.active = null;
        }
        else if (mainTexture is Texture2D texture2D)
        {
            copyTexture = new Texture2D(texture2D.width, texture2D.height, texture2D.format, false);
            copyTexture.SetPixels(texture2D.GetPixels());
            copyTexture.Apply();
        }
        else
        {
            Debug.LogError($"PAINT_GUN >>> Unsupported texture type: {mainTexture.GetType().Name}");

        }
    }

    private void ShootRaycast()
    {
        Vector3 origin = particles.transform.position;
        Vector3 direction = particles.transform.forward;
        Ray cameraRay = new Ray(origin, direction);

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
        continuePainting = false;
    }
}
