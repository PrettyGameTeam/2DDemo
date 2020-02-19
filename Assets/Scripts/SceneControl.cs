﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour
{
    //当前被选中的组件名称
    private string currentCheckedObjectName = null;
    private GameObject choose;

    //改变选中的组件
    public void ChangeCheckedObj(GameObject obj)
    {
        if (currentCheckedObjectName != null)
        {
            var gameObject = GameObject.Find(currentCheckedObjectName);
            if (gameObject != null)
            {
                BaseObject bo = gameObject.GetComponent<BaseObject>();
                bo.SetChecked(false);
            }
            
        }
        currentCheckedObjectName = obj.name;
        choose.SetActive(true);
        Debug.Log("Choose set true");
        choose.transform.parent = obj.transform;
        choose.transform.position = obj.transform.position;
        Debug.Log("set Choose parent obj");
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        choose = GameObject.Find("Choose");
        choose.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
