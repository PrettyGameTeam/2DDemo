using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//光线传送器脚本
public class Diliver : MonoBehaviour
{
    //光线输出点
    public GameObject[] Outs;
    
    //光源照射累计计时 Update每次执行-1TimeDelta,光源照射时每次+2TimeDelta
    private float lastShiningTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lastShiningTime > 0f)
        {
            foreach (var Out in Outs)
            {
                var gun = Out.GetComponent<Gun>();
                if (!Out.activeSelf && gun != null && gun.GetDiliver() == null){
                    gun.SetDiliver(gameObject);
                    Out.SetActive(true);
                }
            }
        }
        else
        {
            Debug.Log("Diliver [" + gameObject.name + "] Update else lastShiningTime=" + lastShiningTime);
            foreach (var Out in Outs)
            {
                if (Out.activeSelf)
                {
                    var gun = Out.GetComponent<Gun>();
                    if (gun != null && gun.GetDiliver() == gameObject)
                    {
                        gun.SetDiliver(null);
                        gun.SetDirty();
                        gun.ResetLight();                
                        Out.SetActive(false);
                    }
                }
                
            }
            lastShiningTime = 0;
        }
        lastShiningTime -= Time.deltaTime;
    }
    
    //光源照射时输出源开始发射光线
    public void LightShining(LineRenderer line)
    {
        // Debug.Log("LightShining lastShiningTime=" + lastShiningTime);
        if (lastShiningTime <= 0f)
        {
            lastShiningTime += Time.deltaTime * 2f;
        }
        lastShiningTime += Time.deltaTime;
    }
}
