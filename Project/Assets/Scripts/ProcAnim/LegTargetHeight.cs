using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegTargetHeight : MonoBehaviour
{
    public static LegTargetHeight instance;
    private Texture2D sampledTexture;

    public Transform minPoint, maxPoint;
    public RenderTexture renderTexture;
    public float heightFactor = 1;
    public float pullTextureInterval = 1;
    private float pullTextureTime = 0;

    void Awake()
    {
        instance = this;
        sampledTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.R16, false);
        
    }

    void Update()
    {
        pullTextureTime -= Time.deltaTime;
        if(pullTextureTime < 0)
        {
            pullTextureTime += pullTextureInterval;
            RenderTexture previousActiveTexture = RenderTexture.active;
            RenderTexture.active = renderTexture;
            sampledTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0, false);
            RenderTexture.active = previousActiveTexture;
        }
    }
    
    
    public Vector3 RaycastPoint(Vector3 point)
    {
        float x = (point.x - minPoint.position.x) / (maxPoint.position.x - minPoint.position.x);
        float z = (point.z - minPoint.position.z) / (maxPoint.position.z - minPoint.position.z);
        Color col = sampledTexture.GetPixelBilinear(x, z);
        return new Vector3(point.x, transform.position.y + col.r * heightFactor, point.z);
    }
}
