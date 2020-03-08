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

}
