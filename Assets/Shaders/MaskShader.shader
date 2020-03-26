// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/MaskShader"
{
    Properties
    {
        _MainTex ("Texture",2D) = "white" {}
        _Alpha ("Alpha",Float) = 1.0
    }
    SubShader
    {
        Tags {"RenderType"="Opaque" "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0; 
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            //point1和point2为线的两个端点
            float pointToLine(vector point1, vector point2, vector position)
            {
                float space = 0;
                float a, b, c;
                a = distance(point1,point2);// 线段的长度      
                b = distance(point1, position);// position到点point1的距离      
                c = distance(point2, position);// position到point2点的距离 
                if (c <= 0.000001 || b <= 0.000001)
                {
                    space = 0;
                    return space;
                }
                if (a <= 0.000001)
                {
                    space = b;
                    return space;
                }
                if (c * c >= a * a + b * b)
                {
                    space = b;
                    return space;
                }
                if (b * b >= a * a + c * c)
                {
                    space = c;
                    return space;
                }
                float p = (a + b + c) / 2;// 半周长      
                float s = sqrt(p * (p - a) * (p - b) * (p - c));// 海伦公式求面积      
                space = 2 * s / a;// 返回点到线的距离（利用三角形面积公式求高）      
                return space;
            }

            sampler2D _MainTex;
            float _Alpha;
            uniform float4 _LightingArr[30];
            uniform float _LightingArrLen;
            uniform float4 _LineLightingArr[90];
            uniform float _LineLightingArrLen;
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex,i.uv);
                col.a = _Alpha;
                //先计算可以让像素完全透明的
                //点光源
                vector p;
                p.x = i.vertex.x;
                p.y = i.vertex.y;
                p.z = 0;
                p.w = 0;
                for(int j = 0; j < _LightingArrLen; j++)
                {
                    float4 p4 = _LightingArr[j];
                    vector startP;
                    startP.x = p4.x;
                    startP.y = p4.y;
                    startP.z = 0;
                    startP.w = 0;
                    //float4 s = UnityObjectToClipPos(startP);
                    vector endP;
                    endP.x = p.x;
                    endP.y = p.y;
                    endP.z = 0;
                    endP.w = 0;
                    float dis = distance(startP,endP);
                    if (dis <= p4.z)
                    {
                        if (dis/p4.z < col.a){
                            col.a = dis/p4.z;
                        }
                    } 
                    //else if (dis <= p4.w + p4.z){
                        //fixed temC = 0.5 * (dis - p4.z) / p4.w;
                        //if (temC < col.a)
                        //{
                            //col.a = temC;
                        //}
                        //col.a = 0.5 * (dis - p4.z) / p4.w;
                    //}   
                }
                
                //如果此时该点不是完全透明的,则继续判断是否在光线范围内
                if (col.a > 0 && _LineLightingArrLen > 0)
                {
                    //每3个数组为光线的一个参数
                    for(int j = 0; j < _LineLightingArrLen; j = j + 3)
                    {
                        //第一个参数为起始点左右2边的点
                        vector param1 = _LineLightingArr[j];
                        vector p1;
                        p1.x = param1.x;
                        p1.y = param1.y;
                        p1.z = 0;
                        p1.w = 0;
                        vector p2;
                        p2.x = param1.z;
                        p2.y = param1.w;
                        p2.z = 0;
                        p2.w = 0;
                        
                        //第二个参数为结束点左右2边的点
                        vector param2 = _LineLightingArr[j + 1];
                        vector p3;
                        p3.x = param2.x;
                        p3.y = param2.y;
                        p3.z = 0;
                        p3.w = 0;                        
                        vector p4;
                        p4.x = param2.z;
                        p4.y = param2.w;
                        p4.z = 0;
                        p4.w = 0;
                        //第三个点为起始点和范围参数
                        vector param3 = _LineLightingArr[j + 2];
                        //取得点到4条边的长度
                        float dis1 = pointToLine(p1,p2,p);
                        float dis2 = pointToLine(p3,p4,p);
                        float dis3 = pointToLine(p1,p3,p);
                        float dis4 = pointToLine(p2,p4,p);
                        if (dis1 <= param3.x && dis2 <= param3.x)
                        {
                            if (dis3 <= param3.y * 2 + 1 && dis4 <= param3.y * 2 + 1)
                            {
                                col.a = 0;
                                break;
                            }
                            else if (dis3 <= param3.y * 2 + param3.z && dis3 > param3.y * 2 && dis4 < param3.z && dis4 > 0 && dis3 > dis4)
                            {
                                fixed temC = (dis3 - param3.y * 2) / param3.z;
                                if (temC < col.a)
                                {
                                    col.a = temC;
                                }
                            } 
                            else if (dis4 <= param3.y * 2 + param3.z && dis4 > param3.y * 2 && dis3 < param3.z && dis3 > 0 && dis4 > dis3)
                            {
                                fixed temC = (dis4 - param3.y * 2) / param3.z;
                                if (temC < col.a)
                                {
                                    col.a = temC;
                                }
                            }     
                        }
                    }          
                }
                return col;
            }
            
            
            
            ENDCG
        }
    }
}
