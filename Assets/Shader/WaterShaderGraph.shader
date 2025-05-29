Shader "Custom/ProceduralWater2D"
{
    Properties
    {
        _ColorTop ("Top Color", Color) = (0.3, 0.7, 1.0, 1)
        _ColorBottom ("Bottom Color", Color) = (0.0, 0.2, 0.5, 1)
        _Speed ("Wave Speed", Float) = 1
        _Amplitude ("Wave Amplitude", Float) = 0.02
        _Frequency ("Wave Frequency", Float) = 20
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _ColorTop;
            float4 _ColorBottom;
            float _Speed;
            float _Amplitude;
            float _Frequency;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float time = _Time.y * _Speed;
                float wave = sin((i.uv.x + time) * _Frequency) * _Amplitude;
                float finalY = i.uv.y + wave;
                fixed4 col = lerp(_ColorBottom, _ColorTop, saturate(finalY));
                return col;
            }
            ENDCG
        }
    }
}
