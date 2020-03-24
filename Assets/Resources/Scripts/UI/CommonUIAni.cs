using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonUIAni : MonoBehaviour
{
    private RawImage _rawImage;

    private float _fadeSpeed = 0f;  //渐变速度

    private float _scaleSpeed = 0f;  //缩放速度

    private int _fadeStatus = 0;    // 0 未开始 1 进行中 2 结束

    private int _scaleStatus = 0;   // 0 未开始 1 进行中 2 结束

    private float _showTime = 0f;   //展示时间,如果展示时间为0 则渐入后不消退

    private float _lastShowTime = 0f;   //已持续显示时间

    private bool _fallback = false;

    private string _evtName = null;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _rawImage = GetComponent<RawImage>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (_fadeStatus == 1){
            if (_fadeSpeed > 0){
                Fade();
                //已经全显示清楚
                if (_rawImage.color.a >= 1f){
                    _fadeStatus = 2;
                }
            }
            else if (_fadeSpeed < 0)
            {
                Fade();
                //已经基本淡出
                if (_rawImage.color.a <= 0f){
                    _fadeStatus = 2;
                }
            }
        }


        if (_scaleStatus == 1){
            if (_scaleSpeed > 0){
                Scale();
                //已经全显示清楚
                if (gameObject.transform.localScale.x >= 1f){
                    _scaleStatus = 2;
                    Debug.Log("Scale Over _evtName=" + _evtName);
                    if (_evtName != null){
                        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(_evtName,null),null);
                    }
                }
            }
            else if (_scaleSpeed < 0)
            {
                Scale();
                //已经缩到最小
                if (gameObject.transform.localScale.x <= 0f){
                    _scaleStatus = 2;
                    if (_evtName != null){
                        ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(_evtName,null),null);
                    }
                }
            }
        }

        if (_showTime > 0){
            bool fadeOver = false;
            bool scaleOver = false;
            if (_fadeSpeed == 0 || (_fadeSpeed != 0 && _fadeStatus == 2)){
                fadeOver = true;
            }

            if (_scaleSpeed == 0 || (_scaleSpeed != 0 && _scaleStatus == 2)){
                scaleOver = true;
            }

            if (_fallback && fadeOver && scaleOver){
                gameObject.transform.parent = null;
                gameObject.SetActive(false);
            }

            if (fadeOver && scaleOver && !_fallback){
                if (_lastShowTime >= _showTime){
                    if (_fadeSpeed > 0){
                        _fadeSpeed = -_fadeSpeed;
                        _scaleSpeed = -_scaleSpeed;
                        _fadeStatus = 1;
                        _scaleStatus = 1;
                        _fallback = true;
                    }
                } else {
                    _lastShowTime += Time.deltaTime;
                }
            }
        }
    }

    private void Fade(){
        Debug.Log("fade");
        // _rawImage.color = Color.Lerp(_rawImage.color, Color.black, _fadeSpeed * Time.deltaTime);
        _rawImage.color = new Color(_rawImage.color.r,_rawImage.color.g,_rawImage.color.b,_rawImage.color.a + _fadeSpeed * Time.deltaTime);
    }

    // private void FadeOut(){
    //     Debug.Log("fadeout");
    //     _rawImage.color = Color.Lerp(_rawImage.color, Color.clear, _fadeSpeed * Time.deltaTime);
    // }

    private void Scale(){
        Debug.Log("Scale");
        var sc = gameObject.transform.localScale;
        sc.x = sc.x + _scaleSpeed * Time.deltaTime;
        sc.y = sc.y + _scaleSpeed * Time.deltaTime;
        sc.z = sc.z + _scaleSpeed * Time.deltaTime;
        gameObject.transform.localScale = sc;
    }

    public void AutoPlay(float fadeSpeed, float scaleSpeed, float showTime){
        _fadeSpeed = fadeSpeed;
        _scaleSpeed = scaleSpeed;
        _showTime = showTime;
        _rawImage.color = new Color(_rawImage.color.r,_rawImage.color.g,_rawImage.color.b,0f);
        gameObject.transform.localScale = new Vector3(0f,0f,0f);
        _fadeStatus = 1;
        _scaleStatus = 1;
    }

    public void PlayScale(float scaleSpeed, string evtName = null){
        _scaleSpeed = scaleSpeed;
        gameObject.transform.localScale = new Vector3(0f,0f,0f);
        _scaleStatus = 1;
        _evtName = evtName;
    }
}
