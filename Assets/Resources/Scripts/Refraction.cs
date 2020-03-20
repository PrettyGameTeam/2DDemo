using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refraction : MonoBehaviour
{

    public bool UseOriginLight = true;  //是否沿用射入的光线的属性

    public int OutColor = 0;    // 0 射入光源颜色 1 红色 2 绿色 3 蓝色

    public int OutLineStrength = 0;    //0 射出光源强度 1 细光线 >1 粗光线

    public int Angle = 120;    // 偏折角度
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetOutDir(Vector2 inDir){
        return Quaternion.AngleAxis(Angle,new Vector3(0,0,1)) * -inDir.normalized;
    }

    public Vector2 GetOutPosition(Vector2 dir)
    {
        RaycastHit2D hit = new RaycastHit2D();
        Vector2 start = gameObject.transform.position;

        do
        {
            var d = dir.normalized * 0.01f;
            start = start + d;
            hit = Physics2D.Raycast(start, dir);
            //如果没有碰到碰撞体,用lastHitPoint进行反向碰撞找到穿出点
            if (hit.collider == null)
            {
                hit = Physics2D.Raycast(start, -dir);
                if (hit.collider != null)
                {
                    return hit.point + 0.01f * dir.normalized;
                }
                // Debug.LogWarning("[" + gameObject.name + "] GetOutPosition if 1");
            }
            else 
            {
                // Debug.LogWarning("[" + gameObject.name + "] [" + hit.collider.gameObject.name + "] GetOutPosition if 2");
                if (hit.collider.gameObject != gameObject){
                    hit = Physics2D.Raycast(hit.point + 0.01f * -dir.normalized, -dir);
                    if (hit.collider != null)
                    {
                        return hit.point + 0.01f * dir.normalized;
                    }
                }
                // Debug.LogWarning("[" + gameObject.name + "] GetOutPosition else if");
            }
        } while (hit.collider != null);
        // Debug.LogWarning("[" + gameObject.name + "] GetOutPosition end");
        return Vector2.zero;
    }



}
