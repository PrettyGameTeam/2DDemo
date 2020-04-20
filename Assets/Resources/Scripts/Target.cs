using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Target : MonoBehaviour
{
    private Vector3 originPos;

    public GameObject[] AniObjs;

    private int _status = 0;

    private bool _gunDisappear = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

        Debug.Log("ForwardOver isEnd=" + isEnd + ",_gunDisappear=" + _gunDisappear);
        if (isEnd && _gunDisappear){
            _status = 2;
            //设置动画组件隐藏
            foreach (var obj in AniObjs)
            {
                obj.SetActive(false);
            }
            ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.Victory),null);
        }
    }

    private void PlayAni(){
        
        if (_status == 0){
            _status = 1;
            //自身设置为隐藏
            gameObject.SetActive(false);
            foreach (var obj in AniObjs)
            {
                FrameAni fa = obj.GetComponent<FrameAni>();
                fa.PlayForward(gameObject);
                obj.SetActive(true);
            }
        }
    }

    //当鼠标点击下去
    private void OnMouseDown()
    {
        var mousePositionOnScreen = Input.mousePosition;
        var mousePositionInWorld =  Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
        originPos = mousePositionInWorld;
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
                // PlayAni();
                ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.TargetClick),gameObject);
            }
        }   
    }

    public void SetGunDisappear(){
        Debug.Log("SetGunDisappear");
        _gunDisappear = true;
        ForwardOver();
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.VictoryAniPlay,OnVictoryAniPlay);
    }

    private void OnDestroy() {
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.VictoryAniPlay,OnVictoryAniPlay);
    }

    private void OnVictoryAniPlay(UEvent evt){
        PlayAni();
    }


}
