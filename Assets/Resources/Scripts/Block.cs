﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //发光点组件
    public GameObject ShiningPoint;
    
    //持续照射时间
    private float lastShiningTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lastShiningTime -= Time.deltaTime;
        if (lastShiningTime <= 0)
        {
            lastShiningTime = 0;
            if (ShiningPoint.activeSelf)
            {
                ShiningPoint.SetActive(false);
            }
        }
    }
    
    public void LightShining(Vector3 hitPoint)
    {
        if (lastShiningTime == 0f)
        {
            lastShiningTime += Time.deltaTime * 2f;
            //设置闪光点位置
            ShiningPoint.transform.position = hitPoint;
            //显示闪光点
            ShiningPoint.SetActive(true);
        }
        else
        {
            lastShiningTime += Time.deltaTime;
            ShiningPoint.transform.position = hitPoint;
        }
    }
}
