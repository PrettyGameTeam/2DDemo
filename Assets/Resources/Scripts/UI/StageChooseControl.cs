﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageChooseControl : MonoBehaviour
{
    private StageNode[] _stageNodes;
    private Chapter _chapter;

    // public AudioClip BgAudio;

    // private AudioSource audio = null;
    
    // Start is called before the first frame update
    void Start()
    {
        //查找Bg
        _stageNodes = new StageNode[25];
        GameObject stageList = GameObject.Find("Canvas/StageList");
        for (int i = 0; i < 25; i++)
        {
            _stageNodes[i] = stageList.transform.Find("Stage" + i).gameObject.GetComponent<StageNode>();
        }
        var userData = UserDataManager.GetInstance().GetUserData();
        LoadChapter(userData.Chapters[userData.Chapters.Count - 1]);
        // audio = GetComponent<AudioSource>();
        // audio.clip = BgAudio;
        // audio.Play();
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
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.PlayStage,OnPlayStage);
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.DebugOneKeyClick,OnOneKey);
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.NextChapterClick,OnNextChapterClick);
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.PreChapterClick,OnPreChapterClick);
        AudioManager.GetInstance().PlayNewAudio(ConfigManager.GetInstance().GetSystemParamByKey("StageChooseSound"));
    }

    public void LoadChapter(UserChapter userChapter)
    {
        //加载标题
        ChapterNode cn = GameObject.Find("Canvas/Bg").GetComponent<ChapterNode>();
        ConfigManager cm = ConfigManager.GetInstance();
        _chapter = cm.GetChapter(userChapter.ChapterId);
        cn.LoadData(_chapter,userChapter);

        Chapter chapter = ConfigManager.GetInstance().GetChapter(userChapter.ChapterId);
        //加载关卡按钮
        for (int i = 0; i < _stageNodes.Length; i++)
        {
            if (i >= chapter.Stages.Count)
            {
                _stageNodes[i].LoadData(null,null);
            }
            else if (i >= userChapter.Stages.Count)
            {
                _stageNodes[i].LoadData(ConfigManager.GetInstance().GetStage(chapter.Stages[i].StageId),null);
            }
            else
            {
                Debug.Log("chapter.Stages[i].StageId=" + chapter.Stages[i].StageId);
                Debug.Log("userChapter.Stages[i]=" + userChapter.Stages[i]);
                _stageNodes[i].LoadData(ConfigManager.GetInstance().GetStage(chapter.Stages[i].StageId),userChapter.Stages[i]);
            }
        }
    }

    private void OnPlayStage(UEvent evt)
    {
        Debug.Log("OnPlayStage");
        //查找到点击的关卡
        UserDataManager.GetInstance().GetUserData().CurrentStage = (int) evt.eventParams;
        
        // s.PrefabName  
        SceneManager.LoadScene("Stage", LoadSceneMode.Single);
    }

    private void OnOneKey(UEvent evt)
    {
        Debug.Log("OnOneKey");
        //查找到点击的关卡
        UserDataManager.GetInstance().OneKeyOpen();
        UserChapter uc = UserDataManager.GetInstance().GetUserData().GetUserChapter(_chapter.ChapterId);
        LoadChapter(uc);
    }

    private void OnNextChapterClick(UEvent evt)
    {
        Debug.Log("OnNextChapterClick");
        //查找到点击的关卡
        if (_chapter.NextChapter == null){
            return;
        }
        UserChapter uc = UserDataManager.GetInstance().GetUserData().GetUserChapter(_chapter.NextChapter.ChapterId);
        if (uc == null){
            return;
        }
        _chapter = _chapter.NextChapter;
        LoadChapter(uc);
    }

    private void OnPreChapterClick(UEvent evt)
    {
        Debug.Log("OnNextChapterClick");
        //查找到点击的关卡
        if (_chapter.PreChapter == null){
            return;
        }
        UserChapter uc = UserDataManager.GetInstance().GetUserData().GetUserChapter(_chapter.PreChapter.ChapterId);
        if (uc == null){
            return;
        }
        _chapter = _chapter.PreChapter;
        LoadChapter(uc);
    }
}
