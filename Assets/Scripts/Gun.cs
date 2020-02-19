﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //光线
    public LineRenderer lineRenderer;
    
    //光线路径点
    public List<Vector3> linePoints = new List<Vector3>();

    // private float durationTime = 0f; 
    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    void CastLight()
    {
        linePoints.Clear();
        //光线起始点
        // var startPoint = transform.position + new Vector3(0,1,0);
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
                BaseObject bo = obj.GetComponent<BaseObject>();
                if (bo != null)
                {
                    //阻挡光线物体
                    if (bo.IsBlock)
                    {
                        needReflect = false;
                    }
                    //可反射物
                    else if (bo.IsReflect)
                    {
                        needReflect = true;
                        //利用Vector2的反射函数来计算反射方向
                        var inDirection = hit.point - (Vector2) startPoint;
                        direction = Vector2.Reflect(inDirection, hit.normal);
                        // Debug.Log("i=" + i + ",hit=" + hit.point + " startPoint=" + startPoint + ",inDirection=" + inDirection + ",outDirection=" + direction);
                        //击中点作为新的起点
                        startPoint = (Vector3)hit.point + direction * 0.01f;
                    }
                    //既不反射也不阻挡物体(光源)
                    else
                    {
                        needReflect = false;
                    }
                }
                else
                {
                    needReflect = false;
                }
                
                ActiveObject ao = obj.GetComponent<ActiveObject>();
                if (ao != null)
                {
                    Debug.Log("Exec LightShining");
                    ao.LightShining(lineRenderer);
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
}
