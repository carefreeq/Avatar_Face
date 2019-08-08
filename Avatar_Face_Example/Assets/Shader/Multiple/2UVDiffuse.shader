// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "QQ/Multiple/2UVDiffuse"
{
	Properties
	{
		_MainTex("Texture", 2D) = "black" {}
		_SecondTex("Texture",2D) = "black" {}
		_NormalDir("show dir",Vector) = (1,1,1,0)
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				#include "UnityCG.cginc"

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
					UNITY_FOG_COORDS(0)
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _SecondTex;
				float4 _SecondTex_ST;
				float4 _NormalDir;
				v2f vert(a2v v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv[0] = TRANSFORM_TEX(v.uv[0], _MainTex);
					o.normal = v.normal;
					o.uv[1] = TRANSFORM_TEX(v.uv[1], _SecondTex);
					UNITY_TRANSFER_FOG(o,o.pos);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 tex0 = tex2D(_MainTex,i.uv[0]);
					fixed4 tex1 = tex2D(_SecondTex,i.uv[1]);
					float l = length(normalize(i.normal)*_NormalDir.xyz);
					fixed4 col = lerp(tex0, tex1, l*tex1.a);
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG
			}
		}
}
