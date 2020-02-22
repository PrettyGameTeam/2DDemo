using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuClick : MonoBehaviour
{
    public int ClickEvent = 1;
    private Vector3 originPos;
    private bool isPanelShow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        var mousePositionOnScreen = Input.mousePosition;
        var mousePositionInWorld =  Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
        originPos = mousePositionInWorld;
        Debug.Log("Lighting OnMouseDown originPos=" + originPos); 
    }

    private void OnMouseUp()
    {
        var mousePositionOnScreen = Input.mousePosition;
        var mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
        if (originPos.x == mousePositionInWorld.x && originPos.y == mousePositionInWorld.y)
        {

            switch (ClickEvent)
            {
                case 1://显示or隐藏菜单
                    break;
                case 2://回到关卡选择
                    break;
                case 3://进入下一关
                    break;
                case 4://重玩此关
                    break;
                default:
                    break;
            }
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            Debug.Log("Lighting OnMouseUp currentPos=" + mousePositionInWorld + ",Victory!!");
            //TODO 弹出胜利框(发送胜利事件)
        }   
    }
    
    
    
}
