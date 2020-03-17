
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
            PutShiningPoint((GameObject)Object.Instantiate(Resources.Load("Prefabs/Objects/ShiningPoint")));
            // _shiningPoints.Push();
        }

        for (int i = 0; i < gunCount; i++)
        {
            PutGun((GameObject)Object.Instantiate(Resources.Load("Prefabs/Objects/Gun")));
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
                PutGun((GameObject)Object.Instantiate(Resources.Load("Prefabs/Objects/Gun")));
                // _guns.Push((GameObject)Object.Instantiate(Resources.Load("Prefabs/Objects/Gun")));
            }
        }
        GameObject gun = _guns.Pop();
        gun.SetActive(true);
        return gun;
    }

    public void PutGun(GameObject gun){
        gun.SetActive(false);
        _guns.Push(gun);
    }

    public GameObject GetShiningPoint(){
        if (_shiningPoints.Count == 0){
            for (int i = 0; i < 10; i++)
            {
                PutShiningPoint((GameObject)Object.Instantiate(Resources.Load("Prefabs/Objects/ShiningPoint")));
                // _shiningPoints.Push((GameObject)Object.Instantiate(Resources.Load("Prefabs/Objects/ShiningPoint")));
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