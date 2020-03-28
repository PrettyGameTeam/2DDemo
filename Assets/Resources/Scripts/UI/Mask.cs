﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public Shader MaskShader;

    // public Texture2D MaskTex;
    
    private SpriteRenderer spriteRenderer;

    private int _status = 0;

    private int _showAfterFrames = 0; //延迟1帧显示光线

    private bool _dirty = false;
    
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
        ReloadMaterial();
        OnLightStatusChange(null);

        // spriteRenderer.material.SetColor("_Color",new Color(0f,0f,0f,0.5f));

        // var spTex = new Texture2D(MaskTex.width,MaskTex.height,TextureFormat.RGBA32,false);
        // RenderTexture currentRT = RenderTexture.active;
        // RenderTexture renderTexture = RenderTexture.GetTemporary(MaskTex.width, MaskTex.height, 32);
        // Graphics.Blit(MaskTex,renderTexture);
        
        // RenderTexture.active = renderTexture;
        // spTex.ReadPixels(new Rect(0,0,renderTexture.width,renderTexture.height),0,0 );
        // spTex.Apply();
        
        // RenderTexture.active = currentRT;
        // RenderTexture.ReleaseTemporary(renderTexture);
        // var sp = Sprite.Create(spTex,new Rect(0.0f, 0.0f, spTex.width, spTex.height), new Vector2(0.5f,0.5f));
        // spriteRenderer.sprite = sp;
    }

    private void OnVictory(UEvent evt){
        // Debug.Log(" Mask OnVictory ");
        if (_status == 0){
            _status = 1;
        }
    }

    private void Awake() {
        // Debug.Log(" Mask awake ");
        ObjectEventDispatcher.dispatcher.addEventListener (EventTypeName.Victory, OnVictory);
        ObjectEventDispatcher.dispatcher.addEventListener (EventTypeName.LightStatusChange, OnLightStatusChange);
    }

    private void OnLightStatusChange(UEvent evt){
        Debug.Log("OnLightStatusChange");
        _dirty = true;
        _showAfterFrames = 2;
    }

    private void reloadLight(){
        var LightingObjects = GameObject.FindGameObjectsWithTag("Lighting");
        // Debug.Log("Mask Update LightingObjects.Count=" + LightingObjects.Length);
        List<Vector4> pointLights = new List<Vector4>();
        
        int pointCount = 0;
        List<Vector4> lineLights = new List<Vector4>();
        
        int lineCount = 0;
        // Debug.Log("Screen.width=" + Screen.width + ",Screen.height=" + Screen.height);
        var asW = Screen.width / 1080f;
        var asH = Screen.height / 1920f;
        foreach (var obj in LightingObjects)
        {
            
            Shining sh = obj.gameObject.GetComponent<Shining>();
            if (sh.LightShape == 1 && sh.P1 > 0)
            {
                // Debug.Log("Lighting if Object Name=" + obj.name);
                pointLights.Add(sh.GetPointLightInfo());
                pointCount++;
            } 
            else if (sh.LightShape == 2 && sh.P1 > 0)
            {
                // Debug.Log("Lighting else if Object Name=" + obj.name);
                foreach (var e in sh.GetLineLightList())
                {
                    // Debug.Log("Lighting Object Name=" + obj.name + ",e=" + e);
                    lineLights.Add(e);
                    lineCount++;
                }   
            }
            else 
            {
                // Debug.Log("Lighting else Object Name=" + obj.name);
            }
        }
        for (int i = pointCount; i < 30; i++)
        {
            pointLights.Add(new Vector4());
        }
        
        for (int i = lineCount; i < 90; i++)
        {
            lineLights.Add(new Vector4());
        }
        spriteRenderer.material.SetFloat("_Alpha",spriteRenderer.color.a);
        
        spriteRenderer.material.SetVectorArray("_LightingArr",pointLights);
        spriteRenderer.material.SetFloat("_LightingArrLen",pointCount);
        spriteRenderer.material.SetFloat("_LineLightingArrLen",lineCount);
        if (lineLights.Count > 0)
        {
            // Debug.Log("Lighting lineLights.Count=" + lineLights.Count + ",lineCount=" + lineCount);
            spriteRenderer.material.SetVectorArray("_LineLightingArr",lineLights);
            
        }
    }

    public void ReloadMaterial()
    {
        // Debug.Log("Start1");
        spriteRenderer = GetComponent<SpriteRenderer>();
        // var t = new Texture2D(1080,1920);
        // Color a = new Color(0f,0f,0f,1f);
        // for (int i = 0; i < 1080; i++)
        // {
        //     for (int j = 0; j < 1920; j++)
        //     {
        //         t.SetPixel(i,j, a);
        //     }
        // }
        // spriteRenderer.sprite = Sprite.Create(t,new Rect(0,0,t.width,t.height), new Vector2(0.5f,0.5f) );
        // Debug.Log("Start2");
        spriteRenderer.material.SetFloat("_LineLightingArrLen",0);
        List<Vector4> lineLights = new List<Vector4>(90);
        spriteRenderer.material.SetVectorArray("_LineLightingArr",lineLights);
    }

    private void OnDestroy() {
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.Victory, OnVictory);
        ObjectEventDispatcher.dispatcher.removeEventListener (EventTypeName.LightStatusChange, OnLightStatusChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (_status == 1){
            
            if (spriteRenderer.color.a <= 0){
                // Debug.Log("spriteRenderer.color=" + spriteRenderer.color + " if");
                _status = 2;
            }
            else {
                var c = spriteRenderer.color;
                c.a = c.a - 0.5f * Time.deltaTime;
                c.a = c.a < 0 ? 0 : c.a;
                spriteRenderer.color = c;
                // Debug.Log("spriteRenderer.color=" + spriteRenderer.color + " else");
            }
            reloadLight();
        }

        if (_dirty){
            if (_showAfterFrames <= 0){
                _dirty = false;
                _showAfterFrames = 0;
                // Debug.Log("OnLightStatusChange _dirty = false _showAfterFrames=" + 0);
                reloadLight();
            } else {
                _showAfterFrames--;
                // Debug.Log("OnLightStatusChange _showAfterFrames-- _showAfterFrames=" + _showAfterFrames);
                reloadLight();
            }
        }
    }
}
