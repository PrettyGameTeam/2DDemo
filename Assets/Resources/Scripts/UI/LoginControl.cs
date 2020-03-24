using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        //加载策划配置 
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.PlayCurrentStage,OnPlayCurrentStage);
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.ClickSetting,OnClickSetting);
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.ClickShare,OnClickShare);
        // ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.ClickMemory,OnClickMemory);
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.QQLogin,OnQQLogin);
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.WechatLogin,OnWechatLogin);
    }

    private void OnDestroy() {
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.PlayCurrentStage,OnPlayCurrentStage);
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.ClickSetting,OnClickSetting);
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.ClickShare,OnClickShare);
        // ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.ClickMemory,OnClickMemory);
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.QQLogin,OnQQLogin);
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.WechatLogin,OnWechatLogin);
    }

    private void OnPlayCurrentStage(UEvent evt){
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.ClickMemory),null);
        // SceneManager.LoadScene("Stage", LoadSceneMode.Single);
    }

    private void OnClickSetting(UEvent evt){

    }

    private void OnClickShare(UEvent evt){
        
    }

    // private void OnClickMemory(UEvent evt){

    // }

    private void OnQQLogin(UEvent evt){

    }

    private void OnWechatLogin(UEvent evt){

    }
}
