using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Target : MonoBehaviour
{
    private Vector3 originPos;

    public Sprite[] Frames;

    public float DeltaPerFrame = 0.1f;

    private int _status = 0;

    private float _playTime = 0f;

    private int _frameIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_status == 1){
            if (_frameIndex >= Frames.Length - 1)
            {
                _status = 2;
                ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.Victory),null);
                gameObject.SetActive(false);
                return;
                
            }
            else 
            {
                if (_playTime >= DeltaPerFrame){
                    _frameIndex++;
                    _playTime = 0;
                    SpriteRenderer aniSpr = GetComponent<SpriteRenderer>();
                    aniSpr.sprite = Frames[_frameIndex];
                }
            }
            _playTime += Time.deltaTime;
        }
    }

    private void PlayAni(){
        if (_status == 0 && Frames != null && Frames.Length > 0){
            _status = 1;
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
                PlayAni();
            }
        }   
    }
}
