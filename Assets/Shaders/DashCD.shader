Shader "Custom/DashCD"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color1 ("Base Color", Color) = (0.5, 0.5, 0.5, 1)
        _Color2 ("Mix Color", Color) = (0.5, 0.5, 0.5, 1)
        _Progress ("Progress", Range(0, 1)) = 0
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
            fixed4 _Color1;
            fixed4 _Color2;
            float _Progress;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float theta = atan(-(i.uv.y - 0.5)/(i.uv.x - 0.5)) + 3.14159 * (i.uv.x-0.5>0?0:1) + 3.14159 / 2;
                theta /= (3.14159 * 2);
                if (theta < _Progress) return _Color2 * col.a;
                return _Color1 * col.a;
            }
            ENDCG
        }
    }
}
