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

    private int _status = 0; //动画状态 0 初始状态(隐藏) 1 播放中 2 播放完成

    private int _direction = 1; //1 正向 2 反向

    private float _playTime = 0f;

    private int _index = 0;

    private SpriteRenderer _aniSpr;

    private GameObject _driveObj;//驱动此动画脚本的组件

    private bool _isActive;//是否已被激活

    // Start is called before the first frame update
    void Start()
    {
        _aniSpr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //正在播放中
        if (_status == 1){
            //正向播放到底
            if (_index == Frames.Length - 1 && _direction == 1){
                _status = 2;    //设置播放状态为播放结束
                _playTime = 0;
                MultiActive ma = _driveObj.GetComponent<MultiActive>();
                ma.ForwardOver();
            } 
            //反向播放到底
            else if (_index == 0 && _direction == 2){
                _status = 0;    //设置播放状态为播放结束
                _playTime = 0;
                MultiActive ma = _driveObj.GetComponent<MultiActive>();
                ma.FallbackOver();
            }
            else {
                if (_playTime >= DeltaPerFrame){
                    _index = _direction == 1 ? _index + 1 : _index - 1;
                    _aniSpr.sprite = Frames[_index];
                    _playTime = 0;
                }
                _playTime += Time.deltaTime;
            }
        }
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
    }

    //是否播放结束
    public bool IsEnd(){
        return _status == 2;
        //正向播放且已到最后一帧
        // if (_index == Frames.Length - 1 && _direction == 1){
        //     MultiActive ma = _driveObj.GetComponent<MultiActive>();
        //     ma.ForwardOver();
        // }
        // return false;
    }

    public bool IsRollBack(){
        return _status == 0;
        //反向播放且已到第一帧
        // if (_index == 0 && _direction == 2){
        //     MultiActive ma = _driveObj.GetComponent<MultiActive>();
        //     ma.FallbackOver();
        // }
        // return false;
    }
}
