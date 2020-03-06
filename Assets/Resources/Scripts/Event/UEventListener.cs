﻿using System;

/*
 * 事件监听类 
 */
public class UEventListener
{
    public UEventListener()
    {
        
    }

    public delegate void EventListenerDelegate(UEvent evt);

    public event EventListenerDelegate OnEvent;

    public void Excute(UEvent evt)
    {
        if (OnEvent != null)
        {
            this.OnEvent(evt);
        }
    }

}