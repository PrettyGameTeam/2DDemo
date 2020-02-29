﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaneraMask : MonoBehaviour
{
    private float devWidth = 5.4f;
    private float devHeight = 9.6f;
    //[Range(0,3)]
    //public float Lerp = 0;

    // public Texture2D MaskTex;
    //
    public Shader ScreenShader;
    //
    // public Texture2D MaskTexture2D;
    //
    // public Material GetMaterial
    // {
    //     get
    //     {
    //         if (_material == null) _material = new Material(ScreenShader);
    //         return _material;
    //     }
    // }
    //
    // private Material _material = null;
    // private List<Vector2> currentShowPoint = new List<Vector2>();
    // private List<Vector2> shouldShowPoint = new List<Vector2>();
    //
    // private void OnRenderImage(RenderTexture src, RenderTexture dest)
    // {
    //     // Debug.Log("src=" + src + ",dest=" + dest);
    //     GetMaterial.SetTexture("_MainTex",src);
    //     GetMaterial.SetTexture("_MaskTex",MaskTex);
    //     //GetMaterial.SetFloat("_Lerp",Lerp);
    //     Graphics.Blit(src,dest,GetMaterial);
    // }


    // Start is called before the first frame update
    void Start()
    {
        // float screenHeight = Screen.height;
        // Debug.Log ("screenHeight = " + screenHeight);
        // float orthographicSize = this.GetComponent<Camera>().orthographicSize;
        // float aspectRatio = Screen.width * 1.0f / Screen.height;
        // float cameraWidth = orthographicSize * 2 * aspectRatio;
        // Debug.Log ("cameraWidth = " + cameraWidth);
        // if (cameraWidth < devWidth)
        // {
        //     orthographicSize = devWidth / (2 * aspectRatio);
        //     Debug.Log ("new orthographicSize = " + orthographicSize);
        //     this.GetComponent<Camera>().orthographicSize = orthographicSize;
        // }
#if UNITY_STANDALONE
        Screen.SetResolution(1080, 1920, true);
#endif
        // Screen.SetResolution(1080,1920,true);
    }

    // Update is called once per frame
    void Update()
    {
        // //鼠标点击时改变遮罩的值
        // if (Input.GetMouseButtonDown(0))
        // {
        //     var mousePositionOnScreen = Input.mousePosition;
        //     var mousePositionInWorld =  Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
        //     //找到西瓜 让西瓜所在区域的遮罩alpha值改变
        //     // var sprite = GameObject.Find("Sprite");
        //     
        //     // float x = sprite.transform.localScale.x ;
        //     Debug.Log("mousePositionOnScreen.x=" + mousePositionOnScreen.x + ",mousePositionOnScreen.y=" + mousePositionOnScreen.y);
        //     var minX = (int) mousePositionOnScreen.x - 50;
        //     var maxX = (int) mousePositionOnScreen.x + 50;
        //     var minY = (int) mousePositionOnScreen.y - 50;
        //     var maxY = (int) mousePositionOnScreen.y + 50;
        //     
        //     for (int x = minX; x < maxX; x++)
        //     {
        //         for (int y = minY; y < maxY; y++)
        //         {
        //             Vector3 p1 = new Vector3(x,y,0);
        //             Vector3 p2 = new Vector3(mousePositionOnScreen.x,mousePositionOnScreen.y,0);
        //             var r = (p1 - p2).magnitude;
        //             if (r <= 50)
        //             {
        //                 Color color = new Color(0f,0f,0f,0.8f);
        //                 color.a = (r / 50) / 2;
        //                 // Debug.Log("x=" + x + ",y=" + y + ",r=" + r + ",color=" + color);
        //                 MaskTex.SetPixel(x,y,color);
        //             }
        //         }
        //     }
        //     MaskTex.Apply();
        // }
    }
}
