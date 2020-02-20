using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shining : MonoBehaviour
{
    // 亮光范围 1 圆形  2 矩形 
    public int LightShape = 1;
    // 固定透明度亮光范围1  LightShape 为1时为半径 为2时为宽
    public int P1 = 0;
    //固定透明度亮光范围的Alpha值
    public float FixedAlpha = 1f;
    // 亮光范围2  渐变像素距离
    public int P2 = 0;
    //衰减开始Alpha值
    public float AttenuationStartAlpha = 1f;
    //衰减结束Alpha值
    public float AttenuationEndAlpha = 0f;
    
    private List<Vector4> _lineLight = new List<Vector4>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Vector4 GetPointLightInfo()
    {
        var asW = Screen.width / 1080f;
        var asH = Screen.height / 1920f;
        Vector4 v = new Vector4(transform.position.x * asW * 100 + Screen.width / 2 , -transform.position.y * asH * 100 + Screen.height / 2, P1 * asW, P2 * asW);
        return v;
    }
    
    //取得直线光源
    public List<Vector4> GetLineLightList()
    {
        _lineLight.Clear();
        CalLinePoints();
        return _lineLight;
    }
    
    public void CalLinePoints()
    {
        // Debug.Log("CalLinePoints 1");
        //矩形光照范围,只有光线才使用此类型
        if (LightShape == 2)
        {
            var lineRenderer = GetComponent<LineRenderer>();
            Vector3[] linePoints = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(linePoints);
                  
            //取得所有光线的线段
            for (int i = 0; i < linePoints.Length - 1; i++)
            {
                Vector2 startP = linePoints[i];
                Vector2 endP = linePoints[i + 1];
                var maskWidth = 1080f;
                var maskHeight = 1920f;
                var asW = Screen.width / maskWidth;
                var asH = Screen.height / maskHeight;
                startP.x = startP.x * asW * 100 + Screen.width / 2;
                startP.y = -startP.y * asH * 100 + Screen.height / 2;
                endP.x = endP.x * asW * 100 + Screen.width / 2;
                endP.y = -endP.y * asH * 100 + Screen.height / 2;
                      
                //求与这条线垂直的向量
                var dir = GetVerticalDir(startP - endP);
                      
                // 1-2 3-4 是同方向直线
                // 1-3 2-4 是同方向直线
                var pos1 = dir * P1 * asW + startP;
                var pos2 = -dir * P1 * asW + startP;
                var pos3 = dir * P1 * asW + endP;
                var pos4 = -dir * P1 * asW + endP;
                var len = Vector2.Distance(startP,endP);
                Vector4 v1 = new Vector4(pos1.x,pos1.y,pos2.x,pos2.y);
                Vector4 v2 = new Vector4(pos3.x,pos3.y,pos4.x,pos4.y);
                Vector4 v3 = new Vector4(len,P1 * asW,P2 * asW,0);
                _lineLight.Add(v1);
                _lineLight.Add(v2);
                _lineLight.Add(v3);
                // Debug.Log("CalLinePoints 2 dir=" + dir);                  
            }
        }
    }

    // public string GetTimeStamp()
    // {
    //     TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
    //     return Convert.ToInt64(ts.TotalSeconds).ToString();
    // } 

    //获取垂直向量
    Vector2 GetVerticalDir(Vector2 _dir)
    {
        var len = _dir.magnitude;
        _dir = _dir/len;
        // Debug.Log("_dir=" + _dir);
        if (_dir.y == 0)
        {
            return new Vector2(0,1);
        }

        if (_dir.x == 0)
        {
            return new Vector2(1,0);
        }
        
        return new Vector2(_dir.y,-_dir.x);
    }
}
