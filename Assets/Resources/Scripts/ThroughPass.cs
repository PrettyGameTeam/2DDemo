﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * 光线穿过效果
 *
 */
public class ThroughPass : MonoBehaviour
{
    public GameObject Out;//穿透过后的光线发射点(也是发光点)

    public bool UseOriginLight = true;  //是否沿用射入的光线的属性

    private float lastShiningTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lastShiningTime -= Time.deltaTime;
        if (lastShiningTime > 0)
        {
            Out.SetActive(true);
        }
        else
        {
            if (Out.activeSelf)
            {
                var gun = Out.GetComponent<Gun>();
                if (gun != null)
                {
                    gun.ResetLight();                
                }
            }
            Out.SetActive(false);
            lastShiningTime = 0;
        }
    }

    // public Vector2 GetOut

    public void LightShining(LineRenderer line,Vector2 hitPoint,Vector2 dir, int color, int strength)
    {
        if (lastShiningTime == 0f)
        {
            lastShiningTime += Time.deltaTime * 2f;
            //设置闪光点位置
            Out.transform.position = hitPoint;
            //显示闪光点
            Out.SetActive(true);
        }
        else
        {
            lastShiningTime += Time.deltaTime;
            Out.transform.position = hitPoint;
        }

        //从击中点向向量发射RayCast
        RaycastHit2D hit = new RaycastHit2D();
        Vector2 start = hitPoint;
        do
        {
            var d = dir.normalized * 0.01f;
            start = start + d;
            hit = Physics2D.Raycast(start, dir);
            //如果没有碰到碰撞体,用lastHitPoint进行反向碰撞找到穿出点
            if (hit.collider == null)
            {
                hit = Physics2D.Raycast(start, -dir);
                if (hit.collider != null)
                {
                    Out.transform.position = hit.point + 0.01f * dir.normalized;
                    var gun = Out.GetComponent<Gun>();
                    if (gun != null)
                    {
                        if (UseOriginLight){
                            gun.color = color;
                            gun.LineStrenth = strength;
                        }
                        // gun.SetLightInfo(color,strength);     
                        gun.SetDirty();   
                        gun.SetShotDir(dir);        
                    }
                    return;
                }
            }
            else 
            {
                if (hit.collider.gameObject != gameObject){
                    hit = Physics2D.Raycast(hit.point + 0.01f * -dir, -dir);
                    if (hit.collider != null)
                    {
                        Out.transform.position = hit.point + 0.01f * dir;
                        var gun = Out.GetComponent<Gun>();
                        if (gun != null)
                        {
                            if (UseOriginLight){
                                gun.color = color;
                                gun.LineStrenth = strength;
                            }
                            // gun.SetLightInfo(color,strength);     
                            gun.SetDirty();   
                            gun.SetShotDir(dir);        
                        }
                        return;
                    }
                }
            }
        } while (hit.collider != null);
    }
}
