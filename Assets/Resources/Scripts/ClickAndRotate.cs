﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ClickAndRotate : MonoBehaviour
{
    
    //光源组件点击半径
    public int Radius;
    
    //操作方式 1 旋转 2 滑动
    public int OpType = 1;
    //子物件
    public GameObject RotateObj;
    //相对初始位置的可转动角度
    public int Angle = 90;
    //滑动组件
    public GameObject Slider;

    //点击起始位置
    private Vector2 originPos;
    
    //点击结束位置
    private Vector2 currentPos;

    //显示选中
    private GameObject _choose;

    //是否被选中
    private bool isChecked = false;

    //是否处于拖动状态
    private bool isDraging = false;

    //前一个拖动点
    private Vector2 prePos;

    //记录物件的旋转角度
    private float currentAngle = 0;

    private float minAngle;
    
    private float maxAngle;

    private Vector2 centerPosition;

    private Vector3 start;
    
    private Vector3 end;

    private bool _lockRotate = false;

    // Start is called before the first frame update
    void Start()
    {
        if (OpType == 1)
        {
            minAngle = -Angle / 2f;
            maxAngle = Angle / 2f;
            centerPosition = transform.position;
            Debug.Log("minAngle=" + minAngle + ",maxAngle=" + maxAngle);
        } 
        //取得物件在向量投影上的初始位置向量
        else if (OpType == 2)
        {
            Transform[] childs = Slider.gameObject.GetComponentsInChildren<Transform>();
            foreach (var ch in childs)
            {
                if (ch.name == "Start")
                {
                    start = ch.position;
                }

                if (ch.name == "End")
                {
                    end = ch.position;
                }
            }
            
            //计算初始位置在滑杆上的投影向量
            centerPosition = (Vector2)start + ShadowVector(transform.position - start, end - start);
            _choose = transform.Find("Choose").gameObject;
            _choose.SetActive(false);
            Debug.Log("Start centerPosition=" + centerPosition + ",start=" + start + ",end=" + end );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_lockRotate){
            return;
        }
        //响应鼠标点击事件
        if (Input.GetMouseButtonDown(0))
        {
            var mousePositionOnScreen = Input.mousePosition;
            var mousePositionInWorld =  Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
            if (isChecked)
            {
                prePos = mousePositionInWorld;
                isDraging = true;
            }
            else if (Vector2.Distance(transform.position,mousePositionInWorld) <= Radius/100)
            {
                originPos = mousePositionInWorld;
                Debug.Log("Lighting GetMouseButtonDown originPos=" + originPos);
            }
        }
        
        //响应鼠标抬起事件
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Lighting GetMouseButtonUp originPos=" + originPos);
            isDraging = false;
            var mousePositionOnScreen = Input.mousePosition;
            var mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
            
            if (!isChecked)
            {
                if (originPos.x == mousePositionInWorld.x && originPos.y == mousePositionInWorld.y)
                {
                    if (Vector2.Distance(transform.position,mousePositionInWorld) <= Radius/100)
                    {
                        SetChecked(true);
                        Debug.Log("Lighting GetMouseButtonUp currentPos=" + currentPos);
                    }
                }   
            }
        }

        //响应鼠标拖动事件
        if (isChecked && isDraging)
        {
            var mousePositionOnScreen = Input.mousePosition;
            var mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
            Transform t;
            if (RotateObj != null)
            {
                t = RotateObj.transform;
            }
            else
            {
                t = transform;
            }

            //转动
            if (OpType == 1) 
            {
                Vector2 from = prePos - centerPosition;
                prePos = mousePositionInWorld;
                Vector2 to = prePos - centerPosition;
                // Debug.Log("from=" + from + ",to=" + to);
                var angle = SignedAngleBetween(from,to,new Vector3(0,0,1));
                // Debug.Log("angle=" + angle + ",t.rotation=" + t.rotation);
                if (minAngle != 0f && maxAngle != 0f)
                {
                    if (currentAngle + angle > maxAngle || currentAngle + angle < minAngle)
                    {
                        angle = 0;
                    }
                    else
                    {
                        currentAngle += angle;
                    }
                }
                t.eulerAngles = new Vector3(t.eulerAngles.x,t.eulerAngles.y,t.eulerAngles.z + angle);
                ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LightStatusChange),null);
            } 
            //滑动
            else if (OpType == 2)
            {
                Vector2 a = (Vector2)mousePositionInWorld - prePos;
                prePos = mousePositionInWorld;
                Vector2 b = end - start;
                Vector2 c = ShadowVector(a,b);
                Vector2 tmp = centerPosition + c;
                //
                if ((tmp - (Vector2) start).magnitude > (end - start).magnitude)
                {
                    c = (Vector2)end - centerPosition;
                }
                else if ((tmp - (Vector2) end).magnitude > (start - end).magnitude)
                {
                    c = (Vector2)start - centerPosition;
                }
                Debug.Log("Update centerPosition=" + centerPosition + ",start=" + start + ",end=" + end + ",c=" + c );
                t.position += (Vector3)c;
                centerPosition += c;
                ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LightStatusChange),null);
            }


            // Debug.Log("t.eulerAngles=" + t.eulerAngles);
            
            // var rad = Mathf.Asin(Vector3.Distance(Vector3.zero, Vector3.Cross(from.normalized, to.normalized)));
            // var angle = rad * Mathf.Rad2Deg;
            //
            // //计算与右向量的夹角
            // //在X轴上方
            // var yFromAng = Vector2.Angle(Vector2.right, from);
            // var yToAng = Vector2.Angle(Vector2.right, to);
            // if (from.y > 0f)
            // {
            //     //如果to的y大于等于0
            //     if (to.y >= 0f && yToAng > yFromAng)
            //     {
            //         angle = -angle;
            //     }
            //     else if (to.y < 0f && from.x * )
            //     {
            //         
            //     }
            //
            // }
            //
            //
            // transform.RotateAround(centerPosition,axiw,angle);
            // t.rotation *= Quaternion.FromToRotation(from,to);
            // if (t.eulerAngles.z > angle1)
            // {
            //     
            // }
            
            // t.rotation = new Quaternion(t.rotation.x, t.rotation.y, t.rotation.z - angle/360, t.rotation.w);
            
        }
    }
    
    //设置组件选中状态
    public void SetChecked(bool isCheck)
    {
        isChecked = isCheck;
        //被选中了,发送给场景控制器选中消息,将其他组件设置成非选中状态
        if (isCheck)
        {
            if (OpType == 2 && _choose != null){
                _choose.SetActive(true);
            }
            GameObject obj = GameObject.Find("SceneControl");
            SceneControl sc = obj.GetComponent<SceneControl>();
            sc.ChangeCheckedObj(gameObject);
        }
        else 
        {
            if (OpType == 2 && _choose != null){
                _choose.SetActive(false);
            }
        }
    }
    
    //计算旋转角
    public float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
        float angle = Vector3.Angle(a,b);
        float sign = Mathf.Sign(Vector3.Dot(n,Vector3.Cross(a,b)));
        return angle * sign;
        // float signed_angle = angle * sign;
        // return (signed_angle <= 0) ? 360 + signed_angle : signed_angle;
    }

    //取得向量A在向量B上的投影
    public Vector2 ShadowVector(Vector2 a,Vector2 b)
    {
        return Vector3.Project(a,b);
    }

    private void OnTargetClick(UEvent evt){
        _lockRotate = true;
    }

    private void Awake() {
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.TargetClick, OnTargetClick);
    }

    private void OnDestroy() {
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.TargetClick, OnTargetClick);
    }
}
