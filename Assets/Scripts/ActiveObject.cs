using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//激活组件
public class ActiveObject : MonoBehaviour
{
    //激活方式 1 点击激活 2 光照激活
    public int ActiveType = 0;

    //帧动画每帧的间隔
    public float DeltaPerFrame = 0.1f;
    
    //播放动画所用的组件
    public GameObject AnimationObj;

    //激活后显示的组件
    public GameObject ActivedObj;
    
    //帧动画
    public Sprite[] Frames;

    //是否正在播放动画
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
    
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        color = sr.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlaying)
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
            }

            framePlayTime += Time.deltaTime;
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
                }
            }
        }
    }

    private void ResetInfo()
    {
        IsPlaying = false;
        lastShiningTime = 0;
        framePlayTime = 0;
        IsOpposePlay = false;
        frameIndex = 0;
        sr.material.color = color;
    }

    //光源照射时调用
    public void LightShining(LineRenderer line)
    {
        lastShiningTime += Time.deltaTime * 1.01f;
        if (!IsPlaying)
        {
            lastShiningTime += Time.deltaTime * 1.01f;
            IsPlaying = true;
            //设置本节点透明度为0
            sr.material.color = new Color(1,1,1,0);
            
            //设置动画节点显示
            AnimationObj.SetActive(true);
            
            //设置动画为正向播放
            IsOpposePlay = false;
        }
    }
}
