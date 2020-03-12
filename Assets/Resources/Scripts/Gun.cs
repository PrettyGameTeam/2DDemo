﻿using System.Collections;
using System.Collections.Generic;
 using UnityEditor;
 using UnityEngine;

public class Gun : MonoBehaviour
{

    //光线prefab
    public GameObject lineObject;
    
    //初始射出角度
    public int initAngle = 0;
    
    //光线路径点
    private List<Vector3> linePoints = new List<Vector3>();
    
    //光线
    private LineRenderer lineRenderer;

    //默认红色光线 1  绿色光线 2 蓝色光线 3
    public int color = 1;

    //光源强度 贴图增强的数量
    public int LineStrenth = 1;

    private bool _dirty = false;

    private Vector2 _shotDir = Vector2.zero;

    private GameObject _diliver;

    // Start is called before the first frame update
    void Start()
    {
        // Material m = null;
        // lineRenderer.material = Resources.Load<Material>(path);
        // lineRenderer.material = Instantiate(Resources.Load<Material>(path));
        _dirty = true;
        Debug.LogWarning("Gun [" + gameObject.name + "] start color = " + color);
    }

    void Awake() {
        lineRenderer = Instantiate(lineObject).GetComponent<LineRenderer>();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,transform.eulerAngles.z + initAngle);
        lineRenderer.transform.parent = gameObject.transform;
        SetLightInfo(color,LineStrenth);
    }

    //改变光线颜色
    public void SetLightInfo(int cl,int strenth)
    {
        string path = "Materials/LightRed";
        Debug.LogWarning("Gun [" + gameObject.name + "] start color = " + cl);
        if (cl == 1)    //红光
        {
            path = "Materials/LightRed";
        } 
        else if (cl == 2) //绿光
        {
            path = "Materials/LightGreen";
        }
        else if (cl == 3) //蓝光
        {
            path = "Materials/LightBlue";
        }
        // var ma = Resources.Load<Material>(path); 
        Material[] ms = new Material[strenth];
        for (int i = 0; i < strenth; i++){
            ms[i] = Resources.Load<Material>(path);
        }
        lineRenderer.materials = ms;
    }

    public void SetDiliver(GameObject obj)
    {
        _diliver = obj;
    }

    public GameObject GetDiliver()
    {
        return _diliver;
    }

    public void SetShotDir(Vector2 dir)
    {
        _shotDir = dir;
    }

    public void SetDirty()
    {
        _dirty = true;
    }

    void CastLight()
    {
        linePoints.Clear();
        //光线起始点
        Vector2 startPoint = transform.position;

        //光线射出方向
        Vector2 direction = (Vector2)transform.up;
        if (_shotDir != Vector2.zero){
            direction = _shotDir;
        }
        
        linePoints.Add(startPoint);

        RaycastHit2D hit = new RaycastHit2D();
        var needReflect = false;
        do
        {
            hit = Physics2D.Raycast(startPoint, direction);
            if (hit.collider != null)
            {
                linePoints.Add(hit.point);
                var obj = hit.collider.gameObject;
                Block bl = obj.GetComponent<Block>();
                //阻挡光线
                if (bl != null)
                {
                    needReflect = false;
                    bl.LightShining(hit.point);
                }
                
                Reflect re = obj.GetComponent<Reflect>();
                if (re != null)
                {
                    needReflect = true;
                    //利用Vector2的反射函数来计算反射方向
                    
                    var inDirection = hit.point - (Vector2) startPoint;
                    direction = re.GetOutDirection(inDirection, hit.normal);
                    re.LightShining(hit.point);
                    //击中点作为新的起点
                    startPoint = hit.point + direction * 0.01f;
                }
                
                ActiveObject ao = obj.GetComponent<ActiveObject>();
                if (ao != null)
                {
                    Debug.Log("Exec LightShining");
                    ao.LightShining(lineRenderer);
                }
                
                Diliver di = obj.GetComponent<Diliver>();
                if (di != null)
                {
                    Debug.Log("Exec Diliver LightShining");
                    di.LightShining(lineRenderer);
                }

                ThroughPass th = obj.GetComponent<ThroughPass>();
                if (th != null)
                {
                    Debug.Log("Exec ThroughPass LightShining");
                    th.LightShining(lineRenderer,hit.point,linePoints[linePoints.Count-1]-linePoints[linePoints.Count-2],color,LineStrenth);
                }

                Collections co = obj.GetComponent<Collections>();
                if (co != null)
                {
                    Debug.Log("Exec Collections LightShining");
                    co.LightShining();
                }

                LightCondition lc = obj.GetComponent<LightCondition>();
                if (lc != null)
                {
                    Debug.Log("Exec LightCondition LightShining");
                    lc.LightShining(color,LineStrenth);
                }
            }
            else
            {
                needReflect = false;
                var p = startPoint + (direction * 1920/100f);
                linePoints.Add(p);
            }
        } while (needReflect);
    }

    // Update is called once per frame
    void Update()
    {
        if (_dirty){
            _dirty = false;
            SetLightInfo(color,LineStrenth);
        }

        lineRenderer.gameObject.SetActive(true);
        CastLight();
        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.ToArray());
    }

    public void ResetLight()
    {
        if (_dirty){
            _dirty = false;
            SetLightInfo(color,LineStrenth);
        }
        linePoints.Clear();
        lineRenderer.positionCount = 0;
    }
}
