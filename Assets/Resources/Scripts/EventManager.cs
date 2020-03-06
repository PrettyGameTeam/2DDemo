using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    
    public delegate void Callback();
    public delegate void CallbackPrm(params object[] prm);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static EventManager _Event;
    // 最终结束回调字典
    private Dictionary<string, CallbackPrm> _myEventCallback = new Dictionary<string, CallbackPrm>();
 
    // 最终结束一次回调字典
    private Dictionary<string, CallbackPrm> _myEventOnceCallback = new Dictionary<string, CallbackPrm>();
    
    void Awake()
    {
        _Event = this;
    }
 
    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="name"> 事件名称 </param>
    /// <param name="callback"> 带参事件 </param>
    public void AddEvent(string name, CallbackPrm callback)
    {
        if (_myEventCallback.ContainsKey(name))
        {
            Debug.Log("Events already exist (CallbackPrm)");
        }
        else
        {
            _myEventCallback.Add(name, callback);
        }
    }
 
    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="name"> 动画名称 </param>
    /// <param name="callback"> 不带参事件 </param>
    public void AddEvent(string name, Callback callback)
    {
        if (_myEventCallback.ContainsKey(name))
        {
            Debug.Log("Events already exist (callback)");
        }
        else
        {
            _myEventCallback.Add(name,(object[] o)=> { callback(); });
        }
    }
 
    /// <summary>
    /// 添加执行一次事件
    /// </summary>
    /// <param name="name"> 事件名称 </param>
    /// <param name="callback"> 带参事件 </param>
    public void AddOnceEvent(string name, CallbackPrm callback)
    {
        if (_myEventOnceCallback.ContainsKey(name))
        {
            Debug.Log("Events already exist (CallbackPrm)");
        }
        else
        {
            _myEventOnceCallback.Add(name, callback);
        }
    }
 
    /// <summary>
    /// 添加执行一次事件
    /// </summary>
    /// <param name="name"> 动画名称 </param>
    /// <param name="callback"> 不带参事件 </param>
    public void AddOnceEvent(string name, Callback callback)
    {
        if (_myEventOnceCallback.ContainsKey(name))
        {
            Debug.Log("Events already exist (callback)");
        }
        else
        {
            _myEventOnceCallback.Add(name, (object[] o) => { callback(); });
        }
    }
 
    /// <summary>
    /// 派发事件
    /// </summary>
    /// <param name="name"> 事件名称 </param>
    public void DispatchEvent(string name,params object[] o)
    {
        if (_myEventCallback.ContainsKey(name))
        {
            _myEventCallback[name](o);
        }
        else if (_myEventOnceCallback.ContainsKey(name))
        {
            _myEventOnceCallback[name](o);
            _myEventOnceCallback.Remove(name);
            //Debug.Log("err : 未注册 "+ name);
        }
        else {
            Debug.Log("err : 未注册 " + name);
        }
    }
 
    /// <summary>
    /// 清理事件
    /// </summary>
    /// <param name="name"> 事件名称 </param>
    public void ClearEvent(string name)
    {
        if (_myEventCallback.ContainsKey(name))
        {
            _myEventCallback.Remove(name);
        }
        if (_myEventOnceCallback.ContainsKey(name))
        {
            _myEventOnceCallback.Remove(name);
        }
    }
}
