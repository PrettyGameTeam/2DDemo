using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 光线条件检测器
 */
public class LightCondition : MonoBehaviour
{
    public int Color = 1;//光源颜色条件

    public int LightStrength = 1;//光源强度条件

    private float lastShiningTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lastShiningTime -= Time.deltaTime;
        if (lastShiningTime <= 0)
        {
            lastShiningTime = 0;
        }
    }

    public void LightShining(int color, int lightStrength)
    {
        if ((Color == 0 || Color == color) && lightStrength >= LightStrength){
            if (lastShiningTime == 0f)
            {
                lastShiningTime += Time.deltaTime * 2f;
            }
            else
            {
                lastShiningTime += Time.deltaTime;
            }
        }
    }

    //是否满足检测条件
    public bool isMatch(){
        if (lastShiningTime > 0)
        {
            return true;
        }
        return false;
    }
}
