﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour
{
    //当前被选中的组件名称
    private string currentCheckedObjectName = null;
    private GameObject choose;
    private GameObject stage;
    private string stageName = null;

    //改变选中的组件
    public void ChangeCheckedObj(GameObject obj)
    {
        if (currentCheckedObjectName != null)
        {
            var gameObject = GameObject.Find(currentCheckedObjectName);
            if (gameObject != null)
            {
                ClickAndRotate car = gameObject.GetComponent<ClickAndRotate>();
                car.SetChecked(false);
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
        loadStage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //加载关卡prefab
    public void loadStage()
    {
        if (stageName == null)
        {
            stageName = "Stage";   
        }

        if (stage != null)
        {
            Destroy(stage.gameObject);
        }

        //加载prefab
        stage = (GameObject)Instantiate(Resources.Load("Prefabs/Stage"));
    }

    //恭喜胜利
    public void Victory()
    {
        
    }

}
