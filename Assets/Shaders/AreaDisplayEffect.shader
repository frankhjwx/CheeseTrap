Shader "Custom/AreaDisplayEffect"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _TimeTex ("Texture", 2D) = "white" {}
        _Area ("Area", Range(0, 10000)) = 0
        _Alpha ("Alpha", Range(0, 1)) = 0
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
            int _Area;
            float _Alpha;

            // i = 0~9
            // uv = (0,0) - (1,1)
            // return uv pos on the texture
            float2 getNumberuv(int i, float2 uv){
                int x = (9 - i) % 8;
                int y = 1 - (9 - i)/8;
                return float2(uv.x*0.125 + x*0.125, uv.y*0.5 + y*0.5);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                int num1 = _Area / 1000;
                int num2 = (_Area / 100) % 10;
                int num3 = (_Area / 10) % 10;
                int num4 = _Area % 10;
                
                fixed4 col;

                float2 targetuv = i.uv;

                // 根据uv位置生成横着的贴图
                if (targetuv.x < 0.25) {
                    col = tex2D(_TimeTex, getNumberuv(num1, float2(targetuv.x / 0.25f, targetuv.y)));
                } else if (targetuv.x < 0.5){
                    col = tex2D(_TimeTex, getNumberuv(num2, float2((targetuv.x-0.25) / 0.25, targetuv.y)));
                } else if (targetuv.x < 0.75){
                    col = tex2D(_TimeTex, getNumberuv(num3, float2((targetuv.x-0.5) / 0.25, targetuv.y)));
                } else {
                    col = tex2D(_TimeTex, getNumberuv(num4, float2((targetuv.x-0.75) / 0.25, targetuv.y)));
                }
                if (col.a != 0)
                    col.a = _Alpha;

                return col;
            }
            ENDCG
        }
    }
}