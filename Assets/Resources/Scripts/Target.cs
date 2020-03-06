using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Target : MonoBehaviour
{
    private Vector3 originPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //当鼠标点击下去
    private void OnMouseDown()
    {
        var mousePositionOnScreen = Input.mousePosition;
        var mousePositionInWorld =  Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
        originPos = mousePositionInWorld;
        Debug.Log("target OnMouseDown originPos=" + originPos); 
    }
    
    

    //当鼠标抬起
    private void OnMouseUp()
    {
        Debug.Log("target OnMouseUp originPos=" + originPos); 
        var mousePositionOnScreen = Input.mousePosition;
        var mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
        if (originPos.x == mousePositionInWorld.x && originPos.y == mousePositionInWorld.y)
        {
            Debug.Log("xxxxxxxx");
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null && sr.material != null && sr.material.color.a >= 1f)
            {
                Debug.Log("wwwwwwwwww");
                ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.Victory),null);
            }
        }   
    }
}
