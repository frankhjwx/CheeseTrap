Shader "Custom/TimeDisplayEffect"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _TimeTex ("Texture", 2D) = "white" {}
        _GameTime ("GameTime", Range(0, 100)) = 2
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            sampler2D _TimeTex;
            int _GameTime;

            // i = 0~9
            // uv = (0,0) - (1,1)
            // return uv pos on the texture
            float2 getNumberuv(int i, float2 uv){
                int x = (9 - i) % 4;
                int y = 3 - (9 - i)/4;
                return float2(uv.x*0.25 + x*0.25, uv.y*0.25 + y*0.25);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                int num1 = _GameTime / 10;
                int num2 = _GameTime % 10;
                fixed2 targetuv;
                fixed4 col;

                fixed l1,l2,r1,r2,t1,t2,b1,b2;
                l1 = 0.2;
                l2 = 0.6;
                r1 = 0.1;
                r2 = 1;

                t1 = 0.1;
                t2 = 0.8;
                b1 = 0.1;
                b2 = 1;

                // 斜切算法，l1 l2为左侧y范围，r1 r2为右侧r范围
                targetuv = i.uv;

                if ((i.uv.x < b1 + i.uv.y * (t1 - b1)) || (i.uv.x > b2 + i.uv.y * (t2 - b2))) {
                    col.a = 0;
                    return col;
                }
                targetuv.x = (i.uv.x - (b1 + i.uv.y * (t1 - b1)))/(b2 - b1 + i.uv.y * (t2 - b2 - b1 + t1));

                if ((i.uv.y < l1 + i.uv.x * (r1 - l1)) || (i.uv.y > l2 + i.uv.x * (r2 - l2))) {
                    col.a = 0;
                    return col;
                }
                targetuv.y = (i.uv.y - (l1 + i.uv.x * (r1 - l1)))/(l2 - l1 + i.uv.x * (r2 - l2 - r1 + l1));
                
                float c = 0.45;

                // 根据uv位置生成横着的贴图
                if (targetuv.x < c) {
                    col = tex2D(_TimeTex, getNumberuv(num1, float2(targetuv.x / c, targetuv.y) + float2(-0.1, 0)));
                } else {
                    col = tex2D(_TimeTex, getNumberuv(num2, float2((targetuv.x-c) / (1 - c), targetuv.y) + float2(0.1, 0)));
                }

                if (num1 == 0 && (col.r*255 - 80) <= 0.01) {
                    col.rgb = fixed3(210.0/255, 40.0/255, 40.0/255);
                }
                return col;
            }
            ENDCG
        }
    }
}