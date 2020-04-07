﻿﻿using System;
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
        Debug.Log("ChangeCheckedObj obj.name=" + obj.name);
        var parentObj = obj;
        if (currentCheckObj != null)
        {
            ClickAndRotate car = currentCheckObj.gameObject.GetComponent<ClickAndRotate>();
            car.SetChecked(false);
        }
        currentCheckObj = obj;
        choose.SetActive(true);

        ClickAndRotate ca = parentObj.GetComponent<ClickAndRotate>();
        if (ca.RotateObj != null){
            Debug.Log("parentObj car.RotateObj != null");
            parentObj = ca.RotateObj;
        }
        Debug.Log("Choose set true parentObj.name=" + parentObj.name);
        choose.transform.parent = parentObj.transform;
        choose.transform.position = parentObj.transform.position;
        Debug.Log("set Choose parent obj choose.transform.position=" + choose.transform.position);
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
        loadStage();
        //加载菜单面板
        _victoryPanel = (GameObject)Instantiate(Resources.Load("Prefabs/UI/VictoryPanel"));
        _victoryPanel.transform.SetParent(canvas.transform,false);
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
        _victoryPanel.GetComponent<Victory>().ResetAni();
        _victoryPanel.SetActive(true);
        //展示星星数
        // for (int i = 0; i <= 2; i++)
        // {
        //     var star = _victoryPanel.transform.Find("StarNode" + i + "/Star");
        //     var empty = _victoryPanel.transform.Find("StarNode" + i + "/Empty");
        //     if (i + 1 <= _star){
        //         star.gameObject.SetActive(true);
        //         empty.gameObject.SetActive(false);
        //     } 
        //     else 
        //     {
        //         star.gameObject.SetActive(false);
        //         empty.gameObject.SetActive(true);
        //     }
        // }
    }
    
    private void OnBackClick(UEvent evt)
    {
        Debug.Log("OnBackClick");
        if (stage != null)
        {
            ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LoadStage),null);
            ResetSceneControl();
            DestroyImmediate(stage.gameObject);
            // Destroy(stage.gameObject);
        }
        VariableManager.GetInstance().SetIntVariable("preLoginPrefab",1);
        SceneManager.LoadScene("StageChoose");
    }
    
    private void OnRetryClick(UEvent evt)
    {
        Debug.Log("OnRetryClick");
        loadStage();
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
        loadStage();
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
        _star = 0;
    }
    

    //加载关卡prefab
    public void loadStage()
    {
        var stageName = _stage.PrefabName;
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
        AudioManager.GetInstance().PlayNewAudio("Audio/Stage/" + _stage.Chapter.ChapterAudio);
        UserData userData = UserDataManager.GetInstance().GetUserData();
        UserStage userStage = userData.GetUserStage(_stage.StageId);
        // _star = userStage.Star;
        GameObject obj = GameObject.Find("SceneControl");
        GameObject spr = GameObject.Find(stage.name + "/Sprite");
        SceneControl sc = obj.GetComponent<SceneControl>();
        ClickAndRotate car = spr.GetComponent<ClickAndRotate>();
        // car.SetChecked(true);
    }

}
