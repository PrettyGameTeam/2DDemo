using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log("Lighting OnMouseDown originPos=" + originPos); 
    }
    
    

    //当鼠标抬起
    private void OnMouseUp()
    {
        var mousePositionOnScreen = Input.mousePosition;
        var mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
        if (originPos.x == mousePositionInWorld.x && originPos.y == mousePositionInWorld.y)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null && sr.material != null && sr.material.color.a >= 1f)
            {
                Debug.Log("Lighting OnMouseUp currentPos=" + mousePositionInWorld + ",Victory!!");
                //TODO 弹出胜利框(发送胜利事件) 
            }
        }   
    }
}
