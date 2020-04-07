using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageChooseControl : MonoBehaviour
{
    private GameObject _loginCanvas;
    private GameObject _stageChooseCanvas;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("StageChooseControl Start");
        _loginCanvas = (GameObject)Instantiate(Resources.Load("Prefabs/UI/LoginPanel"));
        _loginCanvas.transform.SetParent(transform,false);
        // _loginCanvas.transform.parent = transform;
        _stageChooseCanvas = (GameObject)Instantiate(Resources.Load("Prefabs/UI/StageChoose"));
        _stageChooseCanvas.transform.SetParent(transform,false);
        // _stageChooseCanvas.transform.parent = transform;
        var pre = VariableManager.GetInstance().GetIntVariable("preLoginPrefab");
        if (pre == 0){
            _loginCanvas.SetActive(true);
            _stageChooseCanvas.SetActive(false);
        } 
        else
        {
            _loginCanvas.SetActive(false);
            _stageChooseCanvas.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        //加载策划配置
        ConfigManager.GetInstance().LoadConfig();
        UserDataManager.GetInstance().LoadUserData(); 
        AudioManager.GetInstance().PlayNewAudio(ConfigManager.GetInstance().GetSystemParamByKey("StageChooseSound"));
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.ClickMemory,OnClickMemory);
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.BackToMain,OnBackToMain);
        // ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.PlayStage,OnPlayStage);
        // ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.DebugOneKeyClick,OnOneKey);
        // ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.NextChapterClick,OnNextChapterClick);
        // ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.PreChapterClick,OnPreChapterClick);
        // AudioManager.GetInstance().PlayNewAudio(ConfigManager.GetInstance().GetSystemParamByKey("StageChooseSound"));
    }

    private void OnDestroy() {
        Debug.Log("StageChooseControl OnDestroy");
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.ClickMemory,OnClickMemory);
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.BackToMain,OnBackToMain);
    }

    private void OnClickMemory(UEvent evt){
        _loginCanvas.SetActive(false);
        _stageChooseCanvas.SetActive(true);
        VariableManager.GetInstance().SetIntVariable("preLoginPrefab",1);
    }

    
    private void OnBackToMain(UEvent evt)
    {
        _loginCanvas.SetActive(true);
        _stageChooseCanvas.SetActive(false);
        VariableManager.GetInstance().SetIntVariable("preLoginPrefab",0);
    }

}
