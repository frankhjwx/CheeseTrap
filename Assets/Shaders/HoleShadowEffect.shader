﻿Shader "Custom/HoleShadowEffect"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Mask ("Base (RGB)", 2D) = "white" {}
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
                float4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _Mask;
            fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                half4 mask = tex2D(_Mask, i.uv);
                float hStep = 1.0 / 540;
                col.a = 0;
            
                for (int k = 2; k <= 45; k++){
                    half4 maskk = tex2D(_Mask, i.uv + float2(0, hStep*k));
                    if ((i.uv + float2(0, hStep*k)).y <= 1 && maskk.r == 0) {
                        col.a = 1;
                        col.rgb = tex2D(_MainTex,i.uv + float2(0, hStep*k)).rgb;
                        break;
                    }
                }
                
                return col;
            }
            ENDCG
        }
    }
}
