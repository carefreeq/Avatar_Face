// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "QQ/Multiple/MappingDiffuse"
{
	Properties
	{
		_MainTex("Texture", 2D) = "black" {}
		_SecondTex("Texture",2D) = "black" {}
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
					float2 uv : TEXCOORD0;
					float3 normal:NORMAL;
				};

				struct v2f
				{
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD1;
					float2 uvDir : TEXCOORD2;
					float3 normal:TEXCOORD3;
					UNITY_FOG_COORDS(0)
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _SecondTex;
				float4 _SecondTex_ST;
				uniform float _Dir;
				uniform float _Power;
				uniform float4 _Size;
				v2f vert(a2v v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.normal = v.normal;
					o.uvDir = v.vertex.xy - _Size.zw;
					UNITY_TRANSFER_FOG(o,o.pos);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 tex0 = tex2D(_MainTex,i.uv);
					fixed4 tex1 = tex2D(_SecondTex, i.uvDir.xy/ _Size.xy+0.5)*_Power;
					float dir = saturate(i.normal.z-0.3)*1.7;
					fixed4 col = lerp(tex0, tex1, dir*tex1.a);
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG
			}
		}
}
