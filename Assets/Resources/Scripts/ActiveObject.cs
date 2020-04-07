using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//激活组件
public class ActiveObject : MonoBehaviour
{
    //激活方式 1 点击激活 2 光照激活 3不再光照激活
    public int ActiveType = 0;

    //帧动画每帧的间隔
    public float DeltaPerFrame = 0.1f;
    
    //播放动画所用的组件
    public GameObject AnimationObj;

    //激活后显示的组件
    public GameObject ActivedObj;
    
    //帧动画
    public Sprite[] Frames;

    //0 不指定照射的颜色 1 红色  2 绿色  3 蓝色
    public int LineColor = 0;

    //光源强度 贴图增强的数量
    public int LineStrenth = 1;

    //是否正在播放动画GetComponentInChildren
    private bool IsPlaying = false;

    //当前播放到的帧数
    private int frameIndex = 0;

    //光源照射累计计时 Update每次执行-1TimeDelta,光源照射时每次+2TimeDelta
    private float lastShiningTime = 0;
    
    //动画帧持续时间 大于DeltaPerFrame则播放下一帧,并且归零
    private float framePlayTime = 0;
    
    //是否反向播放动画
    private bool IsOpposePlay = false;
    
    //本节点Sprite
    private SpriteRenderer sr;
    
    //本节点材质颜色
    private Color color;

    private Vector2 _originPos;
    
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        color = sr.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("ActiveType=" + ActiveType + ",lastShiningTime=" + lastShiningTime);
        if (IsPlaying)
        {
            if (AnimationObj == null)
            {
                ResetInfo();
                //隐藏激活前的组件
                gameObject.SetActive(false);
                ActivedObj.SetActive(true);
                ClickAndRotate car = ActivedObj.GetComponent<ClickAndRotate>();
                car.SetChecked(true);
            }
            else 
            {
                bool changeAniSpr = false;
                if (framePlayTime >= DeltaPerFrame)
                {
                    //正向播放且当前帧数等于最后一帧 播放完成
                    if (!IsOpposePlay)
                    {
                        if (frameIndex >= Frames.Length - 1)
                        {
                            ResetInfo();
                            //动画播放完成,隐藏播放动画的组件
                            AnimationObj.SetActive(false);
                            //显示激活后的组件
                            ActivedObj.SetActive(true);
                            //隐藏激活前的组件
                            gameObject.SetActive(false);
                            ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LightStatusChange),null);
                            return;
                        }
                        else
                        {
                            frameIndex++;
                            changeAniSpr = true;
                        }
                    }
                    //反向播放回到第一帧,播放结束
                    else
                    {
                        if (frameIndex <= 0)
                        {
                            ResetInfo();
                            //动画播放完成,隐藏播放动画的组件
                            AnimationObj.SetActive(false);
                            ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LightStatusChange),null);
                            return;
                        }
                        else
                        {
                            frameIndex--;
                            changeAniSpr = true;
                        }
                        
                    }
                    framePlayTime = 0;
                }

                if (changeAniSpr)
                {
                    SpriteRenderer aniSpr = AnimationObj.GetComponent<SpriteRenderer>();
                    Debug.Log("frameIndex=" + frameIndex + ",Frames.length=" + Frames.Length);
                    aniSpr.sprite = Frames[frameIndex];
                    ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LightStatusChange),null);
                }

                framePlayTime += Time.deltaTime;
                if (ActiveType == 2)
                {
                    lastShiningTime -= Time.deltaTime;
                    if (lastShiningTime < 0)
                    {
                        lastShiningTime = 0;
                        //正向播放改反向
                        if (!IsOpposePlay)
                        {
                            //持续照射时间归零,开始反向播放动画
                            IsOpposePlay = true;
                            //重置framePlayTime
                            framePlayTime = 0;
                            ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LightStatusChange),null);
                        }
                    }
                }
            }
        }
        
        if (ActiveType == 3)
        {
            lastShiningTime -= Time.deltaTime;
            if (lastShiningTime < 0)
            {
                lastShiningTime = 0f;
                //设置播放状态为播放中
                if (!IsPlaying)
                {
                    IsPlaying = true;
                    //向激活方向播放
                    IsOpposePlay = false;
                    sr.material.color = new Color(1,1,1,0);
                    //设置动画节点显示
                    AnimationObj.SetActive(true);
                    //重置framePlayTime
                    framePlayTime = 0;
                    ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LightStatusChange),null);
                }
            }
        }
    }

    //当鼠标点击下去
    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        if (ActiveType == 1 && !IsPlaying){
            var mousePositionOnScreen = Input.mousePosition;
            var mousePositionInWorld =  Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
            _originPos = mousePositionInWorld;
        }
    }

    //当鼠标抬起
    private void OnMouseUp()
    {
        Debug.Log("OnMouseUp");
        if (ActiveType == 1 && !IsPlaying){
            var mousePositionOnScreen = Input.mousePosition;
            var mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
            IsPlaying = true;
            //设置动画节点显示
            if (AnimationObj != null) {
                AnimationObj.SetActive(true);
            }
            //设置动画为正向播放
            IsOpposePlay = false;
            ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LightStatusChange),null);
        }
    }

    private void ResetInfo()
    {
        IsPlaying = false;
        if (ActiveType == 2)
        {
            lastShiningTime = 0;
        }
        framePlayTime = 0;
        IsOpposePlay = false;
        frameIndex = 0;
        sr.material.color = color;
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LightStatusChange),null);
    }

    //光源照射时调用
    public void LightShining(LineRenderer line)
    {
        if (ActiveType == 2)
        {
            var m = line.material;
            bool colorMatch = false;    //颜色满足要求
            bool strenthMatch = false;  //强度满足要求
            if (LineColor == 0)
            {
                colorMatch = true;
            } 
            else if (m.name == "LineRed" && LineColor == 1)
            {
                colorMatch = true;
            }
            else if (m.name == "LineGreen" && LineColor == 2)
            {
                colorMatch = true;
            }
            else if (m.name == "LineBlue" && LineColor == 3)
            {
                colorMatch = true;
            }


            if (line.materials.Length >= LineStrenth)
            {
                strenthMatch = true;
            }

            if (!colorMatch || !strenthMatch){
                return;
            }


            if (!IsPlaying)
            {
                lastShiningTime += Time.deltaTime * 1.01f;
                IsPlaying = true;
                //设置本节点透明度为0
                sr.material.color = new Color(1,1,1,0);
                //设置动画节点显示
                if (AnimationObj != null) {
                    AnimationObj.SetActive(true);
                }
                //设置动画为正向播放
                IsOpposePlay = false;
                ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LightStatusChange),null);
            }
        } 
        else if (ActiveType == 3)
        {
            //如果正在倒退动画,改为反向播放
            if (IsPlaying)
            {
                //正在激活下一个状态,停止,恢复到现在状态
                if (!IsOpposePlay)
                {
                    lastShiningTime += Time.deltaTime * 1.01f;
                    IsOpposePlay = true;
                    framePlayTime = 0;
                    ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LightStatusChange),null);
                }
            }
        }
        lastShiningTime += Time.deltaTime * 1.01f;
    }
}
