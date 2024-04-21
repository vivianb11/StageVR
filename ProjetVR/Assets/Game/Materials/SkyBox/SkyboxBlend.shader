Shader "Custom/SkyboxBlend"
{
    Properties
    {
        _Tint ("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)
        [Gamma] _Exposure ("Exposure", Range(0.0, 8.0)) = 1.0
        _Rotation ("Rotation", Range(0.0, 360.0)) = 0.0
        [NoScaleOffset] _Tex ("Cubemap (HDR)", CUBE) = "grey" { }
        [NoScaleOffset] _Tex2 ("Cubemap 2 (HDR)", CUBE) = "grey" { }
        _Blend ("Blend", Range(0.0, 1.0)) = 0.0
    }

    SubShader
    {
        Tags { "RenderType"="Background" "Queue"="Background" }
        Cull Off
        ZWrite Off
        ZTest LEqual
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
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldRefl : TEXCOORD0;
            };

            float4 _Tint;
            float _Exposure;
            float _Rotation;
            float _Blend;
            samplerCUBE _Tex;
            samplerCUBE _Tex2;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float3 worldNormal = UnityObjectToWorldNormal(-v.vertex);
                o.worldRefl = reflect(worldNormal, float3(0.0, 1.0, 0.0));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 refl = lerp(texCUBE(_Tex, i.worldRefl), texCUBE(_Tex2, i.worldRefl), _Blend);
                refl = pow(refl, _Exposure);
                return fixed4(refl * _Tint, 1);
            }
            ENDCG
        }
    }
}
