﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour
{
    //当前被选中的组件名称
    private GameObject currentCheckObj = null;
    private GameObject choose;
    private GameObject stage;
    private GameObject mask;

    //改变选中的组件
    public void ChangeCheckedObj(GameObject obj)
    {
        if (currentCheckObj != null)
        {
            ClickAndRotate car = currentCheckObj.gameObject.GetComponent<ClickAndRotate>();
            car.SetChecked(false);
        }
        currentCheckObj = obj;
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
        //加载关卡
        loadStage(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ResetSceneControl()
    {
        if (stage != null)
        {
            mask.transform.parent = null;
            DestroyImmediate(mask);
            choose.transform.parent = null;
            choose.SetActive(false);
            currentCheckObj = null;
        }
    }
    

    //加载关卡prefab
    public void loadStage(string stageName)
    {
        
        if (stageName == null)
        {
            stageName = "Stage";   
        }

        if (stage != null)
        {
            ResetSceneControl();
            DestroyImmediate(stage.gameObject);
            // Destroy(stage.gameObject);
        }

        //加载prefab
        stage = (GameObject)Instantiate(Resources.Load("Prefabs/" + stageName));
        mask = (GameObject)Instantiate(Resources.Load("Prefabs/Mask"));
    }

    //恭喜胜利
    public void Victory()
    {
        
    }

}
