using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ClickAndRotate : MonoBehaviour
{
    
    //光源组件点击半径
    public int Radius;
    //子物件
    public GameObject RotateObj;
    //相对初始位置的可转动角度
    public int Angle = 90;
    
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
    
    //组件开始的旋转角度
    private float initedAngle;

    // Start is called before the first frame update
    void Start()
    {
        initedAngle = transform.eulerAngles.z;
        Debug.Log("ClickAndRotate initedAngle=" + initedAngle);
    }

    // Update is called once per frame
    void Update()
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

            Vector2 from = prePos - (Vector2)t.position;
            prePos = mousePositionInWorld;
            Vector2 to = prePos - (Vector2)t.position;
            t.rotation *= Quaternion.FromToRotation(from,to);
            Debug.Log("t.eulerAngles=" + t.eulerAngles);
        }
    }
    
    //设置组件选中状态
    public void SetChecked(bool isCheck)
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
