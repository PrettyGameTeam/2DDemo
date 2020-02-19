﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public Shader MaskShader;
    
    private SpriteRenderer spriteRenderer;
    
    public Material GetMaterial
    {
        get
        {
            if (_material == null) _material = new Material(MaskShader);
            return _material;
        }
    }
    
    private Material _material = null;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start1");
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log("Start2");
        spriteRenderer.material.SetFloat("_LineLightingArrLen",0);
        List<Vector4> lineLights = new List<Vector4>(60);
        spriteRenderer.material.SetVectorArray("_LineLightingArr",lineLights);
        
        // spriteRenderer.material.SetColor("_Color",new Color(0f,0f,0f,0.5f));
        
        // var spTex = new Texture2D(MaskTex.width,MaskTex.height,TextureFormat.RGBA32,false);
        // RenderTexture currentRT = RenderTexture.active;
        // RenderTexture renderTexture = RenderTexture.GetTemporary(MaskTex.width, MaskTex.height, 32);
        // Graphics.Blit(MaskTex,renderTexture);
        //
        // RenderTexture.active = renderTexture;
        // spTex.ReadPixels(new Rect(0,0,renderTexture.width,renderTexture.height),0,0 );
        // spTex.Apply();
        //
        // RenderTexture.active = currentRT;
        // RenderTexture.ReleaseTemporary(renderTexture);
        //
        // _maskWidth = spTex.width;
        // _maskHeight = spTex.height;
        // var sp = Sprite.Create(spTex,new Rect(0.0f, 0.0f, spTex.width, spTex.height), new Vector2(0.5f,0.5f));
        // spriteRenderer.sprite = sp;
    }

    // Update is called once per frame
    void Update()
    {
        var LightingObjects = GameObject.FindGameObjectsWithTag("Lighting");
        List<Vector4> pointLights = new List<Vector4>();
        
        int pointCount = 0;
        List<Vector4> lineLights = new List<Vector4>();
        
        int lineCount = 0;
        Debug.Log("Screen.width=" + Screen.width + ",Screen.height=" + Screen.height);
        var asW = Screen.width / 1080f;
        var asH = Screen.height / 1920f;
        foreach (var obj in LightingObjects)
        {
            BaseObject bo = obj.gameObject.GetComponent<BaseObject>();
            if (bo.LightShape == 1 && bo.P1 > 0)
            {
                
                pointLights.Add(bo.GetPointLightInfo());
                pointCount++;
            } 
            else if (bo.LightShape == 2 && bo.P1 > 0)
            {
                
                foreach (var e in bo.GetLineLightList())
                {
                    lineLights.Add(e);
                    lineCount++;
                }
                
            }

            // if (obj.name == "Sprite")
            // {
            //     Debug.Log("obj.tr=" + obj.transform.rotation);
            // }
        }
        for (int i = pointCount; i < 20; i++)
        {
            pointLights.Add(new Vector4());
        }
        
        for (int i = lineCount; i < 60; i++)
        {
            lineLights.Add(new Vector4());
        }
        spriteRenderer.material.SetFloat("_Alpha",0.5f);
        
        spriteRenderer.material.SetVectorArray("_LightingArr",pointLights);
        spriteRenderer.material.SetFloat("_LightingArrLen",pointCount);
        spriteRenderer.material.SetFloat("_LineLightingArrLen",lineCount);
        if (lineLights.Count > 0)
        {
            spriteRenderer.material.SetVectorArray("_LineLightingArr",lineLights);
            
        }
        //鼠标点击时改变遮罩的值
        // if (Input.GetMouseButtonDown(0))
        // {
        //     var LightingObjects = GameObject.FindGameObjectsWithTag("Lighting");
        //     List<Vector2> points = new List<Vector2>();
        //     foreach (var obj in LightingObjects)
        //     {
        //         BaseObject bo = obj.gameObject.GetComponent<BaseObject>();
        //         bo.CalLightingPoints(_maskWidth,_maskHeight);
        //         points.AddRange(bo.GetLightPoints);
        //     }
        //     Debug.Log("points=" + points);
        //
        //     foreach (var p in points)
        //     {
        //         Color color = new Color(0f,0f,0f,0f);
        //         // Debug.Log("x=" + x + ",y=" + y + ",r=" + r + ",color=" + color);
        //         spriteRenderer.sprite.texture.SetPixel((int)p.x,(int)p.y,color);
        //     }
        //     spriteRenderer.sprite.texture.Apply();
            // var LightingObjects = GameObject.FindGameObjectsWithTag("Lighting");
            // foreach (var obj in LightingObjects)
            // {
            //     Debug.Log("obj.position=" + obj.transform.position);
            //     Debug.Log("obj.localPosition=" + obj.transform.localPosition);
            //     Debug.Log("obj.localScale=" + obj.transform.localScale);
            //     Debug.Log("obj.size=" + obj.GetComponent<Renderer>().bounds.size);
            //     var xOnMask = (int) (obj.transform.position.x * 100) + spriteRenderer.sprite.texture.width / 2;
            //     var yOnMask = (int) (obj.transform.position.y * 100) + spriteRenderer.sprite.texture.height / 2;
            //     var minX = xOnMask - 50;
            //     var maxX = xOnMask + 50;
            //     var minY = yOnMask - 50;
            //     var maxY = yOnMask + 50;
            //     // Debug.Log("leftAdd=" + leftAdd + ",minX=" + minX + ",maxX=" + maxX + ",minY=" + minY + ",maxY=" + maxY);
            //
            //     for (int x = minX; x < maxX; x++)
            //     {
            //         for (int y = minY; y < maxY; y++)
            //         {
            //             Vector3 p1 = new Vector3(x,y,0);
            //             Vector3 p2 = new Vector3(xOnMask,yOnMask,0);
            //             var r = (p1 - p2).magnitude;
            //             if (r <= 50)
            //             {
            //                 Color color = new Color(0f,0f,0f,0.8f);
            //                 color.a = (r / 50) / 2;
            //                 // Debug.Log("x=" + x + ",y=" + y + ",r=" + r + ",color=" + color);
            //                 spriteRenderer.sprite.texture.SetPixel(x,y,color);
            //                 // MaskTex.SetPixel(x,y,color);
            //             }
            //         }
            //     }
            // }
            // spriteRenderer.sprite.texture.Apply();
        // }
    }
}
