using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Victory : MonoBehaviour
{
    private GameObject _excellet;

    private GameObject _bg;

    private GameObject _star0;

    private GameObject _star1;

    private GameObject _star2;

    private int _aniIndex = 0;

    private bool _isPlaying = false;

    private int _star = 3;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Victory Start");
        _bg = transform.Find("Panel").gameObject;
        _excellet = transform.Find("Panel/Victory").gameObject;
        _star0 = transform.Find("Panel/Star0/Star").gameObject;
        _star1 = transform.Find("Panel/Star1/Star").gameObject;
        _star2 = transform.Find("Panel/Star2/Star").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (_aniIndex == 0){
            Debug.Log("Victory ResetVictory");
            ResetVictory();
            _aniIndex = 1;
        }

        if (!_isPlaying && _aniIndex == 1){
            Debug.Log("Victory _bg");
            _isPlaying = true;
            CommonUIAni ani = _bg.GetComponent<CommonUIAni>();
            ani.PlayScale(3f,EventTypeName.VictoryActionDone);
            _bg.SetActive(true);
        }

        if (!_isPlaying && _aniIndex == 2){
            Debug.Log("Victory _excellet");
            _isPlaying = true;
            CommonUIAni ani = _excellet.GetComponent<CommonUIAni>();
            ani.PlayScale(3f,EventTypeName.VictoryActionDone);
            _excellet.SetActive(true);
        }

        if (!_isPlaying && _aniIndex == 3 && _star >= 1){
            _isPlaying = true;
            CommonUIAni ani = _star0.GetComponent<CommonUIAni>();
            ani.PlayScale(3f,EventTypeName.VictoryActionDone);
            _star0.SetActive(true);
        }

        if (!_isPlaying && _aniIndex == 4 && _star >= 2){
            _isPlaying = true;
            CommonUIAni ani = _star1.GetComponent<CommonUIAni>();
            ani.PlayScale(3f,EventTypeName.VictoryActionDone);
            _star1.SetActive(true);
        }

        if (!_isPlaying && _aniIndex == 5 && _star >= 3){
            _isPlaying = true;
            CommonUIAni ani = _star2.GetComponent<CommonUIAni>();
            ani.PlayScale(3f,EventTypeName.VictoryActionDone);
            _star2.SetActive(true);
        }
        
    }

    private void Awake() {
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.VictoryActionDone,OnVictoryActionDone);
    }

    private void OnVictoryActionDone(UEvent evt){
        _aniIndex++;
        _isPlaying = false;
    }

    private void ResetVictory(){
        _excellet.SetActive(false);
        _bg.SetActive(false);
        _star0.SetActive(false);
        _star1.SetActive(false);
        _star2.SetActive(false);
    }

    private void OnDestroy() {
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.VictoryActionDone,OnVictoryActionDone);
    }


}
