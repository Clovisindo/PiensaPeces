Shader "Custom/FishTankWaterDistortion"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _DistortionTex("Distortion Texture", 2D) = "bump" {}
        _DistortionStrength("Distortion Strength", Range(0,0.1)) = 0.02
        _DistortionSpeed("Distortion Speed", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Name "FishTankDistortionPass"
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _DistortionTex;
            float4 _DistortionTex_ST;
            float _DistortionStrength;
            float _DistortionSpeed;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

half4 frag(Varyings IN) : SV_Target
{
    float2 uv = IN.uv;

    // Usar la variable global _Time.y para animación
    float time = _Time.y;

    float2 scrollUV = uv + float2(time * _DistortionSpeed, time * _DistortionSpeed * 0.5);
    float2 distortion = tex2D(_DistortionTex, scrollUV).rg * 2.0 - 1.0;

    uv += distortion * _DistortionStrength;

    half4 col = tex2D(_MainTex, uv);
    return col;
}
            ENDHLSL
        }
    }
}
