﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.SceneManagement;
 using UnityEngine.UI;

 public class SceneControl : MonoBehaviour
{
    //当前被选中的组件名称
    private GameObject currentCheckObj = null;
    private GameObject choose;
    private GameObject stage;
    private GameObject mask;
    private GameObject _victoryPanel;//菜单面板
    private GameObject canvas;//UI面板
    private Stage _stage;
    private int _star = 0;

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
        Debug.LogWarning("SceneControl Start");
        choose = GameObject.Find("Choose");
        choose.SetActive(false);
        canvas = GameObject.Find("Canvas");
        _stage = ConfigManager.GetInstance().GetStage(UserDataManager.GetInstance().GetUserData().CurrentStage);
        //加载关卡
        loadStage(_stage.PrefabName);
        //加载菜单面板
        _victoryPanel = (GameObject)Instantiate(Resources.Load("Prefabs/UI/VictoryPanel"));
        _victoryPanel.transform.SetParent(canvas.transform);
        _victoryPanel.transform.position = canvas.transform.position;
        _victoryPanel.SetActive(false);
    }

    void OnDestroy(){
        Debug.LogWarning("SceneControl Destroy");
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.Victory, OnVictory);
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.BackClick, OnBackClick);
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.RetryClick, OnRetryClick);
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.NextClick, OnNextClick);
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.StarShining, OnStarShining);
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.StarOutShining, OnStarOutShining);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        Debug.LogWarning("SceneControl Awake");
        //监听胜利事件
        ObjectEventDispatcher.dispatcher.addEventListener (EventTypeName.Victory, OnVictory);
        //监听菜单点击事件
        ObjectEventDispatcher.dispatcher.addEventListener (EventTypeName.BackClick, OnBackClick);
        //监听重玩事件
        ObjectEventDispatcher.dispatcher.addEventListener (EventTypeName.RetryClick, OnRetryClick);
        //监听下一关事件
        ObjectEventDispatcher.dispatcher.addEventListener (EventTypeName.NextClick, OnNextClick);
        //监听星星被照射
        ObjectEventDispatcher.dispatcher.addEventListener (EventTypeName.StarShining, OnStarShining);
        //监听星星脱离照射
        ObjectEventDispatcher.dispatcher.addEventListener (EventTypeName.StarOutShining, OnStarOutShining);
    }

    private void OnStarShining(UEvent evt)
    {
        _star++;
        Debug.Log("OnStarShining _star=" + _star);
        
    }

    private void OnStarOutShining(UEvent evt)
    {
        _star--;
        Debug.Log("OnStarOutShining _star=" + _star);
    }

    private void OnVictory(UEvent evt)
    {
        Debug.Log("OnVictory");
        //更改玩家数据
        UserData userData = UserDataManager.GetInstance().GetUserData();
        UserStage userStage = userData.GetUserStage(_stage.StageId);
        bool needSave = false;
        if (userStage.Star < _star)
        {
            needSave = true;
            userStage.Star = _star;
        }

        if (!userStage.Completed)
        {
            userStage.Completed = true;
            needSave = !userData.OpenStage(_stage.NextStage);
        }

        if (needSave){
            DBManager.WriteUserData();
        }
        //展示胜利面板
        _victoryPanel.SetActive(true);
        //展示星星数
        for (int i = 0; i <= 2; i++)
        {
            var star = _victoryPanel.transform.Find("StarNode" + i + "/Star");
            var empty = _victoryPanel.transform.Find("StarNode" + i + "/Empty");
            if (i + 1 <= _star){
                star.gameObject.SetActive(true);
                empty.gameObject.SetActive(false);
            } 
            else 
            {
                star.gameObject.SetActive(false);
                empty.gameObject.SetActive(true);
            }
        }
    }
    
    private void OnBackClick(UEvent evt)
    {
        Debug.Log("OnBackClick");
        SceneManager.LoadScene("StageChoose");
    }
    
    private void OnRetryClick(UEvent evt)
    {
        Debug.Log("OnRetryClick");
        loadStage(_stage.PrefabName);
        _victoryPanel.SetActive(false);
    }
    
    private void OnNextClick(UEvent evt)
    {
        Debug.Log("OnNextClick");
        if (_stage.NextStage == null){
            //TODO 提示玩家已经是最后一关
            return;
        }
        _stage = _stage.NextStage;
        loadStage(_stage.PrefabName);
        UserData userData = UserDataManager.GetInstance().GetUserData();
        userData.CurrentStage = _stage.StageId;
        _victoryPanel.SetActive(false);
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
            ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LoadStage),null);
            ResetSceneControl();
            DestroyImmediate(stage.gameObject);
            // Destroy(stage.gameObject);
        }

        //加载prefab
        // stage = (GameObject)Instantiate(Resources.Load("Prefabs/Stage/Stage"));
        stage = (GameObject)Instantiate(Resources.Load("Prefabs/Stage/" + stageName));
        mask = (GameObject)Instantiate(Resources.Load("Prefabs/Objects/Mask"));
    }

}
