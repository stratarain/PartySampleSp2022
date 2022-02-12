Shader "Custom/VerticalBillboard" {
    Properties {
        _MainTex ("Texture", 2D) = "white" { }
        _Tint ("Tint", Color) = (1, 1, 1, 1)
        _Additive ("Additive", Color) = (0, 0, 0, 0)
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.1
        _ScaleXY ("Scale XY", Vector) = (1, 1, 0, 0)
    }
    SubShader {
        
        Tags { 
            "Queue" = "AlphaTest"
            "RenderType" = "TransparentCutout"
            "IgnoreProjector" = "True"
            "DisableBatching" = "True" 
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #pragma multi_compile _BILLBOARD_OFF _BILLBOARD_ALL_AXIS _BILLBOARD_Y_AXIS

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float3 _Tint;
            float3 _Additive;
            float _Cutoff;
            float2 _ScaleXY;

            v2f vert (appdata v) {
                v2f o;

                float4x4 matrix_M_noRot = unity_ObjectToWorld;
                matrix_M_noRot[0][0] = 1;
                matrix_M_noRot[0][1] = 0;
                matrix_M_noRot[0][2] = 0;
 
                matrix_M_noRot[1][0] = 0;
                matrix_M_noRot[1][1] = 1;
                matrix_M_noRot[1][2] = 0;
 
                matrix_M_noRot[2][0] = 0;
                matrix_M_noRot[2][1] = 0;
                matrix_M_noRot[2][2] = 1;
                
                float3 scaleRotatePos = mul((float3x3) matrix_M_noRot, v.vertex.xyz * float3(_ScaleXY.x, _ScaleXY.y, 0));                
                float3x3 ViewRotateY = float3x3(
                    1, UNITY_MATRIX_V._m01, 0,
                    0, UNITY_MATRIX_V._m11, 0,
                    0, UNITY_MATRIX_V._m21, -1
                );
                
                float3 viewPos = UnityObjectToViewPos(float3(0, 0, 0));
                viewPos += mul(ViewRotateY, scaleRotatePos);
                o.vertex = mul(UNITY_MATRIX_P, float4(viewPos, 1));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - _Cutoff);
                col.rgb *= _Tint;
                col.rgb += _Additive;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            
            ENDCG
        }
    }
}