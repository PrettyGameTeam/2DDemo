using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 星星被照射后 发送星星收集事件,光线脱离后,发送星星丢失事件
 */
public class Collections : MonoBehaviour
{
    private float lastShiningTime = 0f;
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




}
