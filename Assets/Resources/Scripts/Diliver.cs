using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//光线传送器脚本
public class Diliver : MonoBehaviour
{
    //光线输出点
    public GameObject outObject;
    
    //光源照射累计计时 Update每次执行-1TimeDelta,光源照射时每次+2TimeDelta
    private float lastShiningTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update lastShiningTime=" + lastShiningTime);
        lastShiningTime -= Time.deltaTime;
        if (lastShiningTime > 0)
        {
            outObject.SetActive(true);
        }
        else
        {
            if (outObject.activeSelf)
            {
                var gun = outObject.GetComponent<Gun>();
                if (gun != null)
                {
                    gun.ResetLight();                
                }
            }
            outObject.SetActive(false);
            lastShiningTime = 0;
        }
    }
    
    //光源照射时输出源开始发射光线
    public void LightShining(LineRenderer line)
    {
        Debug.Log("LightShining lastShiningTime=" + lastShiningTime);
        if (lastShiningTime <= 0)
        {
            lastShiningTime += Time.deltaTime * 2f;
        }
        lastShiningTime += Time.deltaTime;
    }
}
