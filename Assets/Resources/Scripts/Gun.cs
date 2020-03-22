﻿using System.Collections;
using System.Collections.Generic;
 using UnityEditor;
 using UnityEngine;

public class Gun : MonoBehaviour
{

    //光线prefab
    public GameObject lineObject;
    
    //初始射出角度
    public int initAngle = 0;
    
    //光线路径点
    private List<Vector3> linePoints = new List<Vector3>();
    
    //光线
    private LineRenderer lineRenderer;

    //默认红色光线 1  绿色光线 2 蓝色光线 3
    public int color = 1;

    //光源强度 贴图增强的数量
    public int LineStrenth = 1;

    private bool _dirty = false;

    private Vector2 _shotDir = Vector2.zero;

    private GameObject _diliver;

    private List<GameObject> _shiningPoints = new List<GameObject>();

    private List<GameObject> _guns = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Material m = null;
        // lineRenderer.material = Resources.Load<Material>(path);
        // lineRenderer.material = Instantiate(Resources.Load<Material>(path));
        _dirty = true;
        Debug.LogWarning("Gun [" + gameObject.name + "] start color = " + color);
    }

    void Awake() {
        lineRenderer = Instantiate(lineObject).GetComponent<LineRenderer>();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,transform.eulerAngles.z + initAngle);
        lineRenderer.transform.parent = gameObject.transform;
        SetLightInfo(color,LineStrenth);

        ObjectEventDispatcher.dispatcher.addEventListener (EventTypeName.LoadStage, OnNewStageLoad);
    }

    private void OnNewStageLoad(UEvent evt){
        //销毁自身存放的gun和point
        ClearDirty(0,0);

    }

    void OnDestroy(){
        ObjectEventDispatcher.dispatcher.removeEventListener(EventTypeName.LoadStage, OnNewStageLoad);
    }

    //改变光线颜色
    public void SetLightInfo(int cl,int strenth)
    {
        string path = "Materials/LightRed";
        if (cl == 1)    //红光
        {
            path = "Materials/LightRed";
        } 
        else if (cl == 2) //绿光
        {
            path = "Materials/LightGreen";
        }
        else if (cl == 3) //蓝光
        {
            path = "Materials/LightBlue";
        }

        if (strenth > 1){
            path = path + "2";
        }

        Debug.LogWarning("Gun [" + gameObject.name + "] start color = " + cl + ",strenth=" + strenth + ",path=" + path);

        var ma = Resources.Load<Material>(path); 
        Material[] ms = new Material[1];
        ms[0] = ma;
        // lineRenderer.materials = new Material[1];
        // for (int i = 0; i < strenth; i++){
        //     ms[i] = Resources.Load<Material>(path);
        // }
        lineRenderer.materials = ms;
    }

    public void SetDiliver(GameObject obj)
    {
        _diliver = obj;
    }

    public GameObject GetDiliver()
    {
        return _diliver;
    }

    public void SetShotDir(Vector2 dir)
    {
        _shotDir = dir;
    }

    public void SetDirty()
    {
        _dirty = true;
    }

    public void ClearDirty(int spIdx,int gunIdx){
        // Debug.LogWarning("[" + gameObject.name + "] ClearDirty spIdx=" + spIdx + ",_shiningPoints.Count=" + _shiningPoints.Count + ",gunIdx=" + gunIdx + ",_guns.Count=" + _guns.Count);
        if (spIdx < _shiningPoints.Count){
            for (int i = _shiningPoints.Count - 1; i >= spIdx; i--)
            {
                var it = _shiningPoints[i];
                it.transform.parent = null;
                ObjectPool.GetInstance().PutShiningPoint(it);
                _shiningPoints.RemoveAt(i);
            }
        }

        if (gunIdx < _guns.Count){
            for (int i = _guns.Count - 1; i >= gunIdx; i--)
            {
                var gu = _guns[i];
                gu.transform.parent = null;
                ObjectPool.GetInstance().PutGun(gu);
                _guns.RemoveAt(i);
            }
        }
    }

    void CastLight()
    {
        // Debug.LogWarning("CastLight 1");
        int spIdx = 0; 
        int gunIdx = 0; 
        linePoints.Clear();
        //光线起始点
        Vector2 startPoint = transform.position;

        //光线射出方向
        Vector2 direction = (Vector2)transform.up;
        if (_shotDir != Vector2.zero){
            direction = _shotDir;
        }
        
        linePoints.Add(startPoint);

        RaycastHit2D hit = new RaycastHit2D();
        // do
        // {
        // Debug.LogWarning("CastLight 2");
        hit = Physics2D.Raycast(startPoint, direction);
        if (hit.collider != null)
        {
            linePoints.Add(hit.point);
            //添加击中点闪光
            if (spIdx >= _shiningPoints.Count){
                var ogg = ObjectPool.GetInstance().GetShiningPoint();
                _shiningPoints.Add(ogg);
                
            }
            GameObject gObj = _shiningPoints[spIdx];
            // gObj.transform.parent = transform;
            gObj.transform.position = hit.point; 
            spIdx++;
            // Debug.LogWarning("CastLight 3
            
            var obj = hit.collider.gameObject;
            LightCondition lc = obj.GetComponent<LightCondition>();
            bool isMatch = true;
            if (lc != null)
            {
                lc.LightShining(color,LineStrenth);
                isMatch = lc.isMatch();
            }

            if (!isMatch){
                ClearDirty(spIdx,gunIdx);
                return;
            }
            // Debug.LogWarning("CastLight 4");

            Reflect re = obj.GetComponent<Reflect>();
            if (re != null)
            {
                //利用Vector2的反射函数来计算反射方向
                var inDirection = hit.point - (Vector2) startPoint;
                direction = re.GetOutDirection(inDirection, hit.normal);
                if (gunIdx >= _guns.Count){
                    var gg = ObjectPool.GetInstance().GetGun();
                    _guns.Add(gg);
                }
                GameObject gun = _guns[gunIdx];
                Gun g = gun.GetComponent<Gun>();
                g.SetShotDir(direction);
                // gun.transform.parent = transform;
                // gun.transform.position = hit.point + direction * 0.01f;
                gun.transform.position = hit.point + direction * 0.01f;
                // Debug.LogWarning("Reflect hit.point=" + hit.point + ",gun position=" + gun.transform.position);
                gunIdx++;
            }
            // Debug.LogWarning("CastLight 5");

            ThroughPass th = obj.GetComponent<ThroughPass>();
            if (th != null)
            {
                // Debug.Log("Exec ThroughPass LightShining");
                var inDirection = hit.point - (Vector2) startPoint;
                Vector2 hitP = th.GetOutPosition(hit.point,inDirection);
                if (gunIdx >= _guns.Count){
                    _guns.Add(ObjectPool.GetInstance().GetGun());
                }
                GameObject gun = _guns[gunIdx];
                Gun g = gun.GetComponent<Gun>();
                if (th.UseOriginLight){
                    g.color = color;
                    g.LineStrenth = LineStrenth;
                    // g.ResetLight();
                }
                else 
                {
                    var c = th.OutColor;
                    if (th.OutColor == 0){ 
                        c = color;
                    }

                    var l = th.OutLineStrength;
                    if (th.OutLineStrength == 0){
                        l = LineStrenth;
                    }
                    g.color = c;
                    g.LineStrenth = l;
                    // g.ResetLight();
                }
                g.SetShotDir(inDirection);
                g.SetLightInfo(g.color,g.LineStrenth);
                // gun.transform.parent = transform;
                // gun.transform.position = hit.point + direction * 0.01f;
                gun.transform.position = hitP;
                // Debug.LogWarning("Through[" + th.gameObject.name + "] hit.point=" + hit.point + ",gun position=" + gun.transform.position);
                gunIdx++;
                // th.LightShining(lineRenderer,hit.point,linePoints[linePoints.Count-1]-linePoints[linePoints.Count-2],color,LineStrenth);
            }
            // Debug.LogWarning("CastLight 6");

            Refraction refr = obj.GetComponent<Refraction>();
            if (refr != null){
                var inDirection = hit.point - (Vector2) startPoint;
                Vector2 outDir = refr.GetOutDir(inDirection);
                Vector2 hitP = refr.GetOutPosition(outDir);
                if (gunIdx >= _guns.Count){
                    _guns.Add(ObjectPool.GetInstance().GetGun());
                }
                GameObject gun = _guns[gunIdx];
                Gun g = gun.GetComponent<Gun>();
                if (refr.UseOriginLight){
                    g.color = color;
                    g.LineStrenth = LineStrenth;
                }
                else 
                {
                    var c = refr.OutColor;
                    if (refr.OutColor == 0){ 
                        c = color;
                    }

                    var l = refr.OutLineStrength;
                    if (refr.OutLineStrength == 0){
                        l = LineStrenth;
                    }
                    g.color = c;
                    g.LineStrenth = l;
                    // g.ResetLight();
                }
                g.SetShotDir(outDir);
                g.SetLightInfo(g.color,g.LineStrenth);
                // gun.transform.parent = transform;
                // gun.transform.position = hit.point + direction * 0.01f;
                gun.transform.position = hitP;
                // Debug.LogWarning("Refraction[" + refr.gameObject.name + "] hit.point=" + hit.point + ",gun position=" + gun.transform.position);
                gunIdx++;
            }

            
            ActiveObject ao = obj.GetComponent<ActiveObject>();
            if (ao != null)
            {
                // Debug.Log("Exec LightShining");
                ao.LightShining(lineRenderer);
            }
            
            Diliver di = obj.GetComponent<Diliver>();
            if (di != null)
            {
                // Debug.Log("Exec Diliver LightShining");
                di.LightShining(lineRenderer);
            }

            Collections co = obj.GetComponent<Collections>();
            if (co != null)
            {
                // Debug.Log("Exec Collections LightShining");
                co.LightShining();
            }

            FrameAni fa = obj.GetComponent<FrameAni>();
            if (fa != null)
            {
                // Debug.LogWarning("Exec FrameAni LightShining");
                fa.LightShining();
            }
        }
        else
        {
            var p = startPoint + (direction * 1920/100f);
            linePoints.Add(p);
        }
        ClearDirty(spIdx,gunIdx);
        // } while (needReflect);
    }

    // Update is called once per frame
    void Update()
    {
        if (_dirty){
            _dirty = false;
            SetLightInfo(color,LineStrenth);
        }

        lineRenderer.gameObject.SetActive(true);
        CastLight();
        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.ToArray());
    }

    public void ResetLight()
    {
        if (_dirty){
            _dirty = false;
            SetLightInfo(color,LineStrenth);
        }
        linePoints.Clear();
        lineRenderer.positionCount = 0;
    }
}
