Shader "Custom/FogPlane"
{
    Properties
    {
        _MainTex ("Noise Texture", 2D) = "white" {}  // Noise texture for fog effect
        _FogColor ("Fog Color", Color) = (1,1,1,1)   // Fog color
        _Opacity ("Opacity", Range(0,1)) = 0.8       // Controls overall transparency
        _FadeDistance ("Fade Distance", Range(0,5)) = 2.0 // Distance for fog to fade
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha  // Transparent blending
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _FogColor;
            float _Opacity;
            float _FadeDistance;
            float3 _PlayerPos;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float distanceToPlayer = length(i.worldPos - _PlayerPos);
                float alpha = smoothstep(_FadeDistance, 0, distanceToPlayer); // Fade effect

                fixed4 texColor = tex2D(_MainTex, i.uv) * _FogColor;
                texColor.a *= _Opacity * alpha; // Adjust transparency
                return texColor;
            }
            ENDCG
        }
    }
}
