using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 星星被照射后 发送星星收集事件,光线脱离后,发送星星丢失事件
 */
public class Collections : MonoBehaviour
{
    public GameObject AniObj;
    private float lastShiningTime = 0f;

    private int _aniStatus = 0; //动画状态 0 未开始 1 播放中 2 播放结束

    private Vector2 _targetPos;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lastShiningTime > 0){
            lastShiningTime -= Time.deltaTime;
            if (lastShiningTime <= 0)
            {
                lastShiningTime = 0;
                ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.StarOutShining),null);
            }
        }

        if (_aniStatus == 1 && _targetPos != null){
            Debug.Log("star [" + gameObject.name + "] Update 1 _aniStatus = 1,_targetPos=" + _targetPos);
            if ((AniObj.transform.position.x <= _targetPos.x + 0.01f && AniObj.transform.position.x >= _targetPos.x - 0.01f)  
                && (AniObj.transform.position.y <= _targetPos.y + 0.01f && AniObj.transform.position.y >= _targetPos.y - 0.01f)){
                Debug.Log("star [" + gameObject.name + "] Update 2 _aniStatus = 1,_targetPos=" + _targetPos);
                //向最终目标发送星星已收集
                _aniStatus = 2;
                ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.StarFlyEnd),null);
                gameObject.SetActive(false);
            }
            else {
                Debug.Log("star [" + gameObject.name + "] Update 3 _aniStatus = 1,_targetPos=" + _targetPos);
                AniObj.transform.Rotate(new Vector3(0,0,1) * 1080 * Time.deltaTime);
                AniObj.transform.position = Vector2.MoveTowards(AniObj.transform.position,_targetPos,Time.deltaTime * 20f);
                Debug.Log("star [" + gameObject.name + "] Update 3 _aniStatus = 1,_targetPos=" + _targetPos + ",AniObj.transform.position=" + AniObj.transform.position);
            }
        }
    }
    
    public void LightShining()
    {
        Debug.Log("Collections LightShining");
        if (lastShiningTime <= 0f)
        {
            lastShiningTime = 0f;
            lastShiningTime += Time.deltaTime * 2f;
            //设置闪光点位置
            ObjectEventDispatcher.dispatcher.dispatchEvent(new UEvent(EventTypeName.StarShining),null);
        }
        else
        {
            lastShiningTime += Time.deltaTime;
        }
    }

    private void OnTargetClick(UEvent evt){
        Debug.Log("Collections OnTargetClick 1");
        if (lastShiningTime > 0f && _aniStatus == 0){
            Debug.Log("Collections OnTargetClick 2");
            //开始播放
            _aniStatus = 1;
            var tar = (GameObject)evt.target;
            _targetPos = tar.transform.position;
            AniObj.SetActive(true);
        }
    }

    private void Awake() {
        ObjectEventDispatcher.dispatcher.addEventListener(EventTypeName.TargetClick,OnTargetClick);
    }

    private void OnDestroy() {
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.TargetClick,OnTargetClick);
    }
}
 