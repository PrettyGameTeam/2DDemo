
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private static ObjectPool _objectPool = new ObjectPool();
        
    private Stack<GameObject> _shiningPoints = new Stack<GameObject>();

    private Stack<GameObject> _guns = new Stack<GameObject>();

    public static ObjectPool GetInstance()
    {
        return _objectPool;
    }

    //由场景控制器在进入场景时进行初始化
    public void InitPool(int shiningCount, int gunCount)
    {
        for (int i = 0; i < shiningCount; i++)
        {
            var ob = (GameObject)Object.Instantiate(Resources.Load("Prefabs/Objects/ShiningPoint"));
            Object.DontDestroyOnLoad(ob);
            PutShiningPoint(ob);
            // _shiningPoints.Push();
        }

        for (int i = 0; i < gunCount; i++)
        {
            var ob = (GameObject)Object.Instantiate(Resources.Load("Prefabs/Objects/Gun"));
            Object.DontDestroyOnLoad(ob);
            PutGun(ob);
            // _guns.Push();
        }
    }

    //由场景控制器在退出场景时进行销毁
    public void ResetPool()
    {
        _shiningPoints.Clear();
        _guns.Clear();
    }

    public GameObject GetGun(){
        if (_guns.Count == 0){
            for (int i = 0; i < 5; i++)
            {
                var ob = (GameObject)Object.Instantiate(Resources.Load("Prefabs/Objects/Gun"));
                Object.DontDestroyOnLoad(ob);
                PutGun(ob);
            }
        }
        GameObject gun = _guns.Pop();
        gun.SetActive(true);
        return gun;
    }

    public void PutGun(GameObject gun){
        Gun g = gun.GetComponent<Gun>();
        g.ClearDirty(0,0);
        gun.SetActive(false);
        _guns.Push(gun);
    }

    public GameObject GetShiningPoint(){
        if (_shiningPoints.Count == 0){
            for (int i = 0; i < 10; i++)
            {
                var ob = (GameObject)Object.Instantiate(Resources.Load("Prefabs/Objects/ShiningPoint"));
                Object.DontDestroyOnLoad(ob);
                PutShiningPoint(ob);
            }
        }

        GameObject sp = _shiningPoints.Pop();
        sp.SetActive(true);
        return sp;
    }

    public void PutShiningPoint(GameObject po){
        po.SetActive(false);
        _shiningPoints.Push(po);
    }


}