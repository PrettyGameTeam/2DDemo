﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    // 亮光范围 1 圆形  2 矩形 
    public int LightShape = 1;
    // 亮光范围1  LightShape 为1时为半径 为2时为宽
    public int P1 = 0;
    // 亮光范围2  渐变像素
    public int P2 = 0;
    //是否遮挡物
    public bool IsBlock = false;
    //是否反光物
    public bool IsReflect = false;
    //是否可旋转 可旋转的物件可以操作
    public bool CanRotate = false;
    //是否光源组件
    public bool IsLightControl = false;
    //光源组件点击半径
    public int Radius = 0;

    //取得直线光源信息
    private List<Vector4> _lineLight = new List<Vector4>();
    
    //取得附着在直线上的点光源的照亮范围
    private List<Vector4> _pointLights = new List<Vector4>();

    //点击起始位置
    private Vector2 originPos;
    
    //点击结束位置
    private Vector2 currentPos;

    //是否被选中
    private bool isChecked = false;

    //是否处于拖动状态
    private bool isDraging = false;

    //前一个拖动点
    private Vector2 prePos;

    //子物件
    public GameObject rotateObj;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //如果是光源组件
        if (CanRotate)
        {
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
                else if (IsLightControl && Vector2.Distance(transform.position,mousePositionInWorld) <= Radius/100)
                {
                    originPos = mousePositionInWorld;
                    Debug.Log("Lighting GetMouseButtonDown originPos=" + originPos);
                }
            }
            
            //响应鼠标抬起事件
            if (Input.GetMouseButtonUp(0))
            {
                isDraging = false;
                var mousePositionOnScreen = Input.mousePosition;
                var mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
                
                if (!isChecked)
                {
                    if (originPos.x == mousePositionInWorld.x && originPos.y == mousePositionInWorld.y)
                    {
                        if (IsLightControl && Vector2.Distance(transform.position,mousePositionInWorld) <= Radius/100)
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
                if (rotateObj != null)
                {
                    t = rotateObj.transform;
                }
                else
                {
                    t = transform;
                }

                Vector2 from = prePos - (Vector2)t.position;
                prePos = mousePositionInWorld;
                Vector2 to = prePos - (Vector2)t.position;
                t.rotation *= Quaternion.FromToRotation(from,to);
            }
        }
    }

    //当鼠标点击下去
    private void OnMouseDown()
    {
        if (CanRotate)
        {
            var mousePositionOnScreen = Input.mousePosition;
            var mousePositionInWorld =  Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
            originPos = mousePositionInWorld;
            Debug.Log("Lighting OnMouseDown originPos=" + originPos); 
        }
    }
    
    

    //当鼠标抬起
    private void OnMouseUp()
    {
        if (CanRotate)
        {
            var mousePositionOnScreen = Input.mousePosition;
            var mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
            if (!isChecked)
            {
                if (originPos.x == mousePositionInWorld.x && originPos.y == mousePositionInWorld.y)
                {
                    SetChecked(true);
                    Debug.Log("Lighting OnMouseUp currentPos=" + currentPos);
                }   
            }
        }
    }

    //设置组件选中状态
    public void SetChecked(bool isCheck)
    {
        if (CanRotate)
        {
            isChecked = isCheck;

            //被选中了,发送给场景控制器选中消息,将其他组件设置成非选中状态
            if (isCheck)
            {
                GameObject obj = GameObject.Find("SceneControl");
                SceneControl sc = obj.GetComponent<SceneControl>();
                sc.ChangeCheckedObj(gameObject);
            }
        }
    }

    //取得点光源
    public Vector4 GetPointLightInfo()
    {
        var asW = Screen.width / 1080f;
        var asH = Screen.height / 1920f;
        Vector4 v = new Vector4(transform.position.x * asW * 100 + Screen.width / 2 , -transform.position.y * asH * 100 + Screen.height / 2, P1 * asW, P2 * asW);
        return v;
    }

    //取得直线光源
    public List<Vector4> GetLineLightList()
    {
        _lineLight.Clear();
        CalLinePoints();
        return _lineLight;
    }
    
    //TODO 取得直线顶端挂载的点光源
    public List<Vector4> GetPointLightList()
    {
        return _pointLights;
    }

    public void CalLinePoints()
    {
        // Debug.Log("CalLinePoints 1");
        //矩形光照范围,只有光线才使用此类型
        if (LightShape == 2)
        {
            var lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                 Vector3[] linePoints = new Vector3[lineRenderer.positionCount];
                 lineRenderer.GetPositions(linePoints);
                  
                 //取得所有光线的线段
                 for (int i = 0; i < linePoints.Length - 1; i++)
                 {
                     Vector2 startP = linePoints[i];
                     Vector2 endP = linePoints[i + 1];
                     var maskWidth = 1080f;
                     var maskHeight = 1920f;
                     var asW = Screen.width / maskWidth;
                     var asH = Screen.height / maskHeight;
                     startP.x = startP.x * asW * 100 + Screen.width / 2;
                     startP.y = -startP.y * asH * 100 + Screen.height / 2;
                     endP.x = endP.x * asW * 100 + Screen.width / 2;
                     endP.y = -endP.y * asH * 100 + Screen.height / 2;
                      
                     //求与这条线垂直的向量
                     var dir = GetVerticalDir(startP - endP);
                      
                     // 1-2 3-4 是同方向直线
                     // 1-3 2-4 是同方向直线
                     var pos1 = dir * P1 * asW + startP;
                     var pos2 = -dir * P1 * asW + startP;
                     var pos3 = dir * P1 * asW + endP;
                     var pos4 = -dir * P1 * asW + endP;
                     var len = Vector2.Distance(startP,endP);
                     Vector4 v1 = new Vector4(pos1.x,pos1.y,pos2.x,pos2.y);
                     Vector4 v2 = new Vector4(pos3.x,pos3.y,pos4.x,pos4.y);
                     Vector4 v3 = new Vector4(len,P1 * asW,P2 * asW,0);
                     _lineLight.Add(v1);
                     _lineLight.Add(v2);
                     _lineLight.Add(v3);
                     // Debug.Log("CalLinePoints 2 dir=" + dir);                  
                 }
            }
        }
    }

    // public string GetTimeStamp()
    // {
    //     TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
    //     return Convert.ToInt64(ts.TotalSeconds).ToString();
    // } 

    //获取垂直向量
    Vector2 GetVerticalDir(Vector2 _dir)
    {
        var len = _dir.magnitude;
        _dir = _dir/len;
        // Debug.Log("_dir=" + _dir);
        if (_dir.y == 0)
        {
            return new Vector2(0,1);
        }

        if (_dir.x == 0)
        {
            return new Vector2(1,0);
        }
        
        return new Vector2(_dir.y,-_dir.x);
    }

    public bool isPointOnReflectLine(Vector2 hitPoint)
    {
        PolygonCollider2D polygonCollider2D = GetComponent<PolygonCollider2D>();
        polygonCollider2D.GetPath(0);
        return false;
    }

    float pointToLine(Vector2 point1, Vector2 point2, Vector2 position)//point1和point2为线的两个端点
    {
        float space = 0;
        float a, b, c;
        a = Vector2.Distance(point1,point2);// 线段的长度      
        b = Vector2.Distance(point1, position);// position到点point1的距离      
        c = Vector2.Distance(point2, position);// position到point2点的距离 
        if (c <= 0.000001f || b <= 0.000001f)
        {
            space = 0;
            return space;
        }
        if (a <= 0.000001f)
        {
            space = b;
            return space;
        }
        if (c * c >= a * a + b * b)
        {
            space = b;
            return space;
        }
        if (b * b >= a * a + c * c)
        {
            space = c;
            return space;
        }
        float p = (a + b + c) / 2;// 半周长      
        float s = Mathf.Sqrt(p * (p - a) * (p - b) * (p - c));// 海伦公式求面积      
        space = 2 * s / a;// 返回点到线的距离（利用三角形面积公式求高）      
        return space;
    }
}
