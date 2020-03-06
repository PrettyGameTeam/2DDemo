using System;

/*
 * 事件类
 */
public class UEvent
{
    //事件类型
    public string eventType;

    //参数
    public Object eventParams;

    //事件抛出者
    public Object target;

    public UEvent(string eventType, object eventParams = null)
    {
        this.eventType = eventType;
        this.eventParams = eventParams;
    }
}