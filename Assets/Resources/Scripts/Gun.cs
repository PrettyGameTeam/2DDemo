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

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = Instantiate(lineObject).GetComponent<LineRenderer>();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,transform.eulerAngles.z + initAngle);
        lineRenderer.transform.parent = gameObject.transform;
        Material m = null;
        string path = "Materials/LightRed";
        Debug.LogWarning("Gun [" + gameObject.name + "] start color = " + color);
        if (color == 1)    //红光
        {
            path = "Materials/LightRed";
        } 
        else if (color == 2) //绿光
        {
            path = "Materials/LightGreen";
        }
        else if (color == 3) //蓝光
        {
            path = "Materials/LightBlue";
        }
        lineRenderer.material = Instantiate(Resources.Load<Material>(path));
        Debug.LogWarning("Gun [" + gameObject.name + "] start color = " + color);
    }

    //改变光线颜色
    public void ChangeColor(int changeColor)
    {
        if (changeColor == 1)
        {
            lineRenderer.materials[0] = Resources.Load<Material>("Materials/LightRed");
        }
        else if (changeColor == 2)
        {
            lineRenderer.materials[0] = Resources.Load<Material>("Materials/LightGreen");
        }
        else if (changeColor == 3)
        {
            lineRenderer.materials[0] = Resources.Load<Material>("Materials/LightBlue");
        }
    }

    void CastLight()
    {
        linePoints.Clear();
        //光线起始点
        var startPoint = transform.position;

        //光线射出方向
        var direction = transform.up;
        
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
                    startPoint = (Vector3)hit.point + direction * 0.01f;
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
        lineRenderer.gameObject.SetActive(true);
        CastLight();
        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.ToArray());
    }

    public void ResetLight()
    {
        linePoints.Clear();
        lineRenderer.positionCount = 0;
    }
}
