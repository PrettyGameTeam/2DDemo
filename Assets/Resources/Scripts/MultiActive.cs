using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 复合条件激活脚本
 */
public class MultiActive : MonoBehaviour
{
    //绑定了条件检测脚本的物体
    public GameObject[] ConditionObjs;

    //条件激活后透明的组件
    public GameObject[] HideObjs;  

    //是否设置为不显示作为隐藏手段
    public bool HideWithUnactive = false;

    //播放动画所用的组件
    public GameObject[] AniObjs;

    //激活后显示的组件
    public GameObject[] TargetObjs;

    //状态0 未播放  1 正向播放  2 反向播放 3 播放完成
    private int _status = 0;

    //隐藏的物件的颜色
    private Color[] _hideColors;

    // Start is called before the first frame update
    void Start()
    {
        if (!HideWithUnactive){
            _hideColors = new Color[HideObjs.Length];
            for (int i = 0; i < _hideColors.Length; i++)
            {
                SpriteRenderer sr = HideObjs[i].GetComponent<SpriteRenderer>();
                _hideColors[i] = sr.material.color;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_status == 3){
            return;
        }

        bool isFit = true;
        foreach (var con in ConditionObjs)
        {
            LightCondition lc = con.GetComponent<LightCondition>();
            if (!lc.isMatch()){
                isFit = false;
                break;
            }
        }

        //满足条件
        if (isFit){
            //没有开始播放,设置为开始播放
            if (_status == 0){
                ChangeForwardDir();
                SetHideObjsTransparent();
            }
            //正在反向播放, 改为正向播放
            else if (_status == 2) {
                ChangeForwardDir();
            }
        } else {    //不满足条件
            //正向播放中,设置为反向播放
            if (_status == 1){
                ChangeBackwardDir();
            }
        }
    }

    //设置源组件透明-
    public void SetHideObjsTransparent(){
        for (int i = 0; i < HideObjs.Length; i++)
        {
            if (HideWithUnactive){
                HideObjs[i].SetActive(false);
            }
            else 
            {
                SpriteRenderer sr = HideObjs[i].GetComponent<SpriteRenderer>();
                sr.material.color = new Color(1,1,1,0);
            }
        }

        foreach (var obj in AniObjs)
        {
            if (HideWithUnactive){
                //开启光源控制
                obj.GetComponent<FrameAni>().SwitchLightControl(true);
            }
            obj.SetActive(true);
        }
    }

    //设置正向播放方向
    public void ChangeForwardDir(){
        _status = 1;
        foreach (var obj in AniObjs)
        {
            FrameAni fa = obj.GetComponent<FrameAni>();
            fa.PlayForward(gameObject);
        }
    }

    //设置方向播放方向
    public void ChangeBackwardDir(){
        _status = 2;
        foreach (var obj in AniObjs)
        {
            FrameAni fa = obj.GetComponent<FrameAni>();
            fa.PlayFallback(gameObject);
        }
    }

    public void ForwardOver(){
        //检查所有动画是否播放完
        bool isEnd = true;
        foreach (var obj in AniObjs)
        {
            FrameAni fa = obj.GetComponent<FrameAni>();
            if (!fa.IsEnd()){
                isEnd = false;
                break;
            }
        }

        if (isEnd){
            _status = 3;
            //设置隐藏的物体真正隐藏
            foreach (var obj in HideObjs)
            {
                obj.SetActive(false);
            }

            //设置动画组件隐藏
            foreach (var obj in AniObjs)
            {
                obj.SetActive(false);
            }

            //设置目标组件展示
            foreach (var obj in TargetObjs)
            {
                obj.SetActive(true);
            }

        }
    }

    public void FallbackOver(){
        //检查所有动画是否回滚完
        bool isEnd = true;
        foreach (var obj in AniObjs)
        {
            FrameAni fa = obj.GetComponent<FrameAni>();
            if (!fa.IsRollBack()){
                isEnd = false;
                break;
            }
        }

        if (isEnd){
            _status = 0;
            //设置隐藏的物体显示
            for (int i = 0; i < HideObjs.Length; i++)
            {
                if (HideWithUnactive){
                    HideObjs[i].SetActive(true);
                } 
                else 
                {
                    SpriteRenderer sr = HideObjs[i].GetComponent<SpriteRenderer>();
                    sr.material.color = _hideColors[i];
                }
                
            }

            //设置动画组件隐藏
            foreach (var obj in AniObjs)
            {
                if (HideWithUnactive){
                    obj.GetComponent<FrameAni>().SwitchLightControl(false);
                }
                obj.SetActive(false);
            }

            //设置目标组件隐藏
            foreach (var obj in TargetObjs)
            {
                obj.SetActive(false);
            }
        }

    }
}
