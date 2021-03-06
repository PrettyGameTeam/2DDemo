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
    // private GameObject choose2;
    private GameObject stage;
    private GameObject mask;
    private GameObject _victoryPanel;//菜单面板
    private GameObject canvas;//UI面板
    private Stage _stage;
    private int _star = 0;

    private int _starAniCount = 0;

    private int _victoryStatus = 0;  //胜利状态 0 未开始 1 播放中 2 播放结束

    private SpriteRenderer _spr;

    private GameObject _targetObject;

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
        else {
            //第一个选中的是女孩
            _spr = parentObj.GetComponent<SpriteRenderer>();
        }
        currentCheckObj = obj;

        ClickAndRotate ca = parentObj.GetComponent<ClickAndRotate>();
        if (ca.RotateObj != null){
            Debug.Log("parentObj car.RotateObj != null");
            parentObj = ca.RotateObj;
        }
        //旋转
        if (ca.OpType == 1){
            choose.SetActive(true);
            // choose2.SetActive(false);
            choose.transform.SetParent(parentObj.transform);
            choose.transform.position = parentObj.transform.position;
            // choose2.transform.parent = null;
        }
        //滑动 
        else if (ca.OpType == 2)
        {
            choose.SetActive(false);
            // choose2.SetActive(true);
            // choose2.transform.SetParent(parentObj.transform);
            // choose2.transform.position = parentObj.transform.position;
            choose.transform.parent = null;
        }
        // Debug.Log("Choose set true parentObj.name=" + parentObj.name);
        // choose.transform.parent = parentObj.transform;
        // choose.transform.position = parentObj.transform.position;
        // Debug.Log("set Choose parent obj choose.transform.position=" + choose.transform.position);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("SceneControl Start");
        choose = GameObject.Find("Choose");
        choose.SetActive(false);
        // choose2 = GameObject.Find("Choose2");
        // choose2.SetActive(false);
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
        ObjectEventDispatcher.dispatcher.removeEventListener (EventTypeName.TargetClick, OnTargetClick);
        ObjectEventDispatcher.dispatcher.removeEventListener (EventTypeName.StarFlyEnd, OnStarFlyEnd);
    }

    // Update is called once per frame
    void Update()
    {
        //开始播放胜利界面
        if (_victoryStatus == 1){
            if (_spr.material.color.a <= 0){
                // 通知target已经完成
                _spr.gameObject.SetActive(false);
                Gun mainGun = _spr.gameObject.GetComponentInChildren<Gun>();
                mainGun.ClearDirty(0,0);
                _victoryStatus = 2;
                Target t = _targetObject.GetComponent<Target>();
                t.SetGunDisappear();
                return;
            }
            float a = _spr.material.color.a - 1f / 14f;
            Color c = new Color(1f,1f,1f,a < 0 ? 0 : a);
            _spr.material.color = c;
        }
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
        //监听胜利目标被点击
        ObjectEventDispatcher.dispatcher.addEventListener (EventTypeName.TargetClick, OnTargetClick);

        ObjectEventDispatcher.dispatcher.addEventListener (EventTypeName.StarFlyEnd, OnStarFlyEnd);
    }

    private void OnStarFlyEnd(UEvent evt){
        Debug.Log("OnStarFlyEnd _starAniCount=" + _starAniCount + ",_victoryStatus=" + _victoryStatus);
        _starAniCount++;
        if (_starAniCount >= _star && _victoryStatus == 0){
            //开始播放
            _victoryStatus = 1;
            ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.VictoryAniPlay),null);
            Debug.Log("OnStarFlyEnd _victoryStatus=" + _victoryStatus);
        }
    }
    
    private void OnTargetClick(UEvent evt)
    {
        _targetObject = (GameObject)evt.target;
        // if (_starAniCount == 0 && _victoryStatus == 0){
        //     //开始播放
        //     _victoryStatus = 1;
        //     Debug.Log("OnTargetClick _victoryStatus=" + _victoryStatus);
        // }
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
        Debug.Log("OnVictory userStage.Star=" + userStage.Star + ",_star=" + _star);
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
            // choose2.transform.parent = null;
            // choose2.SetActive(false);
            currentCheckObj = null;
            _targetObject = null;
        }
        _victoryStatus = 0;
        _star = 0;
        _starAniCount = 0;
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
        // GameObject sr = GameObject.Find(stage.name + "/Sprite");
        // _spr = sr.GetComponent<SpriteRenderer>();
        // SceneControl sc = obj.GetComponent<SceneControl>();
        // ClickAndRotate car = spr.GetComponent<ClickAndRotate>();
        // car.SetChecked(true);
    }

}
