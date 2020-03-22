using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuClickFunction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //点击菜单
    public void BackClick()
    {
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.BackClick), null);
    }
    
    //点击重试
    public void RetryClick()
    {
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.RetryClick), null);
    }
    
    //点击下一关
    public void NextStageClick()
    {
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.NextClick), null);
    }

    //点击下一章
    public void NextChapterClick()
    {
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.NextChapterClick), null);
    }

    //点击上一章
    public void PreChapterClick()
    {
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.PreChapterClick), null);
    }

    //点击一键通关
    public void DebugOneKeyClick()
    {
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.DebugOneKeyClick), null);
    }

    public void PlayCurrentStage()
    {
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.PlayCurrentStage), null);
    }

    public void ClickSetting()
    {
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.ClickSetting), null);
    }

    public void ClickShare()
    {
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.ClickShare), null);
    }

    public void ClickMemory()
    {
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.ClickMemory), null);
    }

    public void QQLogin()
    {
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.QQLogin), null);
    }

    public void WechatLogin()
    {
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.WechatLogin), null);
    }

    public void BackToMain()
    {
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.BackToMain), null);
    }
}
