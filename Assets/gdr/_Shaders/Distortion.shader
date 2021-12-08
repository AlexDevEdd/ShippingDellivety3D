Shader "Custom/ScreenSpaceUVDistortion"
{
    Properties
    {
        _Distortion("Texture", 2D) = "white" {}
        _DistortionStrength("Strength", Range(0,1)) = 0.5
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 100

            GrabPass {}

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
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float4 grabPos : TEXCOORD1;
                };

                sampler2D _GrabTexture;

                sampler2D _Distortion;
                float _DistortionStrength;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.grabPos = ComputeGrabScreenPos(o.pos);
                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    float4 distortionPos = float4(tex2D(_Distortion, i.uv).xy - 0.5, 0.0, 1.0);
                    float4 clipSpaceDistortion = UnityObjectToClipPos(distortionPos);
                    float4 grabDistortion = ComputeGrabScreenPos(clipSpaceDistortion);
                    grabDistortion.xy /= grabDistortion.w;

                    float2 grabUV = i.grabPos.xy / i.grabPos.w;
                    grabUV = lerp(grabUV, grabDistortion, _DistortionStrength);

                    half4 col = tex2D(_GrabTexture, grabUV);
                    return col;
                }
                ENDCG
            }
        }
}