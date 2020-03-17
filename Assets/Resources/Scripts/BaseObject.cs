﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    //取得输入条件
    public LightCondition[] GetConditions(){
        return null;
    }

    //取得输出光线发射器
    public GameObject[] GetOuts(){
        return null;
    }

}
