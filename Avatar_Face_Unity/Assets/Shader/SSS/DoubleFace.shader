// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "QQ/Multiple/DoubleFace"
{
	Properties
	{
		_Color("Color",Color) = (0.5,0.5,0.5,1)
		_FaceColor("FaceColor",Color) = (0.5,0.5,0.5,1)
		_MainTex("Texture", 2D) = "black" {}
		_SecondTex("Texture",2D) = "black" {}
		_FaceSize("face size",Range(0,1)) = 1
		_FaceAlpha("face alpha",Range(0,1)) = 1
		_FaceBlur("face blur",Range(0,0.03)) = 0
		_FaceLuminance("face luminance",Range(-0.2,0.2)) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100
			CGINCLUDE
			#include "UnityCG.cginc"

			fixed4 _Color;
			fixed4 _FaceColor;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _SecondTex;
			float4 _SecondTex_ST;
			float _FaceSize;
			float _FaceAlpha;
			float _FaceBlur;
			float _FaceLuminance;
			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv[2] : TEXCOORD0;
				float3 normal:NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv[2] : TEXCOORD1;
				float3 normal:TEXCOORD3;
				float3 wPos:TEXCOORD4;
				float3 local_normal:TEXCOORD5;
				UNITY_FOG_COORDS(0)
			};
			ENDCG
				Pass
			{
				Tags{ "LightMode" = "Always" }
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				v2f vert(a2v v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv[0] = TRANSFORM_TEX(v.uv[0], _MainTex);
					o.normal = UnityObjectToWorldNormal(v.normal);
					o.local_normal = v.normal;
					o.uv[1] = TRANSFORM_TEX(v.uv[1], _SecondTex);
					o.wPos = mul(unity_ObjectToWorld, v.vertex);
					UNITY_TRANSFER_FOG(o,o.pos);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 tex0 = tex2D(_MainTex,i.uv[0])*_Color * 2;
					fixed4 tex_origin = tex2D(_SecondTex,i.uv[1]);
					fixed4 tex_blur = tex2D(_SecondTex, i.uv[1],float2(_FaceBlur,0),float2(0, _FaceBlur));

					float sc = (tex_origin.r + tex_origin.g + tex_origin.b) / 3;
					fixed4 tex1 = lerp(tex_origin,tex_blur,smoothstep(0.5,1.0, sc));

					tex1 += tex1 * (tex1.r + tex1.g + tex1.b) / 3 * _FaceLuminance;
					float l = length(smoothstep(1 - _FaceSize, 1,normalize(i.local_normal).z)*_FaceAlpha);
					fixed4 col = lerp(tex0, tex1, l*tex1.a);

					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG
			}
		}
			FallBack "Diffuse"
}
