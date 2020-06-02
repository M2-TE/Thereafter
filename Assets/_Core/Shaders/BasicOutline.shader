Shader "Unlit/BasicOutline"
{
    Properties
    {
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _Outline("Outline width", Range(.002, 0.03)) = 0.005
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Cull Front
        ZWrite On 
        ColorMask RGB
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            Name "OUTLINE"

            //CGPROGRAM
            HLSLPROGRAM
            //#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            //#include "UnityCG.cginc"

            CBUFFER_START(UnityPerMaterial)
            float _Outline;
            float4 _OutlineColor;
            CBUFFER_END


            struct appdata
            {
                float4 vertexOS : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertexCS : SV_POSITION;
                half fogCoord : TEXCOORD0;
                half4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;

                v.vertexOS.xyz += v.normal.xyz * _Outline;

                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertexOS.xyz);

                o.vertexCS = vertexInput.positionCS;

                o.color = _OutlineColor;
                o.fogCoord = ComputeFogFactor(o.vertexCS.z);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                i.color.rgb = MixFog(i.color.rgb, i.fogCoord);
                return i.color;
            }
            //ENDCG
                ENDHLSL
        }
    }
}
