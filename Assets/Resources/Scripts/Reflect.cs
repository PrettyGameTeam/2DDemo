using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //返回出射向量
    public Vector2 GetOutDirection(Vector2 inDirection,Vector2 normal)
    {
        return Vector2.Reflect(inDirection, normal);
    }
}
