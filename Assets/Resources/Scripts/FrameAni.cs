using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * 序列帧动画
 */
public class FrameAni : MonoBehaviour
{
    //帧动画
    public float DeltaPerFrame = 0.033f;
    public Sprite[] Frames;

    //激活后是否自动激活
    public bool AutoPlay = false;

    public float CallbackTime = 0f;

    //循环播放
    public bool Repeat = false;

    private int _status = 0; //动画状态 0 初始状态(隐藏) 1 播放中 2 播放完成

    private int _direction = 1; //1 正向 2 反向

    private float _playTime = 0f;

    private int _index = 0;

    private SpriteRenderer _aniSpr;

    private GameObject _driveObj;//驱动此动画脚本的组件

    private float _totalPlayTime = 0f;  //总计播放时间

    private bool _lightControl = false; //被激活后是否采用光照控制

    private float _lastShiningTime = 0f;  //持续光照时间

    // Start is called before the first frame update
    void Start()
    {
        _aniSpr = gameObject.GetComponent<SpriteRenderer>();
        if (AutoPlay){
            _status = 1;//设置为播放
            _direction = 1; //正向播放
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Update _lastShiningTime = " + _lastShiningTime + ",_totalPlayTime=" + _totalPlayTime + ",_direction=" + _direction);
        if (_lightControl){
            _lastShiningTime -= Time.deltaTime;
            if (_lastShiningTime <= 0)
            {
                _lastShiningTime = 0;
                if (_direction == 1 && _status == 1){
                    PlayFallback(_driveObj);
                }
            }
        }
        
        
        //正在播放中
        if (_status == 1){
            // Debug.Log("Update status=" + _status + ",direction=" + _direction + ",totalPlayTime=" + _totalPlayTime);
            //正向播放到底
            if (_index == Frames.Length - 1 && _direction == 1){
                //非循环播放
                if(!Repeat){
                    _status = 2;    //设置播放状态为播放结束
                    _playTime = 0;    
                    PlayOver();
                } 
                else 
                {
                    if (_playTime >= DeltaPerFrame){
                        _index = 0;
                        _aniSpr.sprite = Frames[_index];
                        _playTime = 0;
                    }
                    _playTime += Time.deltaTime;
                    if (CallbackTime > 0f){
                        _totalPlayTime += Time.deltaTime;
                        if (_totalPlayTime >= CallbackTime){
                            PlayOver(); 
                        }
                    }
                }
            } 
            //反向播放到底
            else if (_index == 0 && _direction == 2){
                if(!Repeat){
                    _status = 0;    //设置播放状态为播放结束
                    _playTime = 0;
                    PlayOver();
                }
                else 
                {   
                    if (_playTime >= DeltaPerFrame){
                        _index = Frames.Length - 1;
                        _aniSpr.sprite = Frames[_index];
                        _playTime = 0;
                    }
                    _playTime += Time.deltaTime;
                    if (CallbackTime > 0f){
                        _totalPlayTime -= Time.deltaTime;
                        if (_totalPlayTime <= 0){
                            PlayOver(); 
                        }
                    }
                }
            }
            else {
                if (_playTime >= DeltaPerFrame){
                    _index = _direction == 1 ? _index + 1 : _index - 1;
                    _aniSpr.sprite = Frames[_index];
                    _playTime = 0;
                }
                _playTime += Time.deltaTime;
                if (CallbackTime > 0f){
                    _totalPlayTime += _direction == 1 ? Time.deltaTime : -Time.deltaTime;
                    if (_totalPlayTime >= CallbackTime){
                        PlayOver(); 
                    }
                    else if (_totalPlayTime <= 0 && _direction == 2)
                    {
                        PlayOver(); 
                    }
                }
            }
        }
    }

    public void PlayOver()
    {
        if (_driveObj == null){
            gameObject.SetActive(false);
        }
        else
        {
            MultiActive ma = _driveObj.GetComponent<MultiActive>();
            if (_direction == 1){
                _status = 2;
                ma.ForwardOver();
            }
            else if (_direction == 2){
                _status = 0;
                ma.FallbackOver();
            }
        }
        _totalPlayTime = 0;
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LightStatusChange),null);
    }

    public void PlayForward(GameObject gameObject)
    {
        //当前没有物件驱动此动画
        if (_driveObj == null){
            _driveObj = gameObject;
        }
        else if (_driveObj != gameObject){  // 不是该物体驱动的此动画
            return;
        }
        _direction = 1;
        _index = _index == 0 ? 1 :_index;
        _status = 1;
        _totalPlayTime = 0;
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LightStatusChange),null);
    }

    public void PlayFallback(GameObject gameObject)
    {
        //当前没有物件驱动此动画
        if (_driveObj == null){
            _driveObj = gameObject;
        }
        else if (_driveObj != gameObject){  // 不是该物体驱动的此动画
            return;
        }
        _direction = 2;
        _index = _index == Frames.Length - 1 ? Frames.Length - 2 :_index;
        _status = 1;
        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.LightStatusChange),null);
        // _totalPlayTime = 0;
    }

    //是否播放结束
    public bool IsEnd(){
        return _status == 2;
    }

    public bool IsRollBack(){
        return _status == 0;
    }


    public void LightShining()
    {
        if (_lightControl){
            if (_lastShiningTime == 0f)
            {
                _lastShiningTime += Time.deltaTime * 2f;
            }
            else
            {
                _lastShiningTime += Time.deltaTime;
            }

            if (_lastShiningTime >= 0 && _status == 1 && _direction == 2){
                _direction = 1;
                _index = _index == 0 ? 1 :_index;
            }
        }
    }

    //开启光照控制
    public void SwitchLightControl(bool isControl){
        _lightControl = isControl;
    }
}
