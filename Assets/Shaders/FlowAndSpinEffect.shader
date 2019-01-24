Shader "Custom/FlowAndSpinEffect"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _FlowTex ("Base (RGB)", 2D) = "white" {}
        _ScrollXSpeed("XSpeed", Range(-10, 10)) = -3
        _RotateSpeed("RotateSpeed", Range(-10, 10)) = 0.5
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
            sampler2D _FlowTex;
            fixed _ScrollXSpeed;
            fixed _RotateSpeed;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed xScrollValue = _ScrollXSpeed * _Time.y;
                fixed theta = _RotateSpeed * _Time.y;

                fixed2 pos = i.uv-fixed2(0.5, 0.5);
                fixed4 col = tex2D(_MainTex, fixed2(pos.x*cos(theta) - pos.y*sin(theta), pos.x*sin(theta) + pos.y*cos(theta)) + fixed2(0.5, 0.5));
                fixed4 flowcol = tex2D(_FlowTex, i.uv + fixed2(xScrollValue, 0));
                
                if ((i.uv.x-0.5) * (i.uv.x-0.5) + (i.uv.y-0.5) * (i.uv.y-0.5) > 0.25)
                    col.a = 0;
                col.rgb = min(col.rgb + flowcol.rgb / 4, 1);
                return col;
            }
            ENDCG
        }
    }
}
