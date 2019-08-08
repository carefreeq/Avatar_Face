// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "QQ/Sky/Latitude"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", float) = 2
		[Enum(Off, 0, On, 1)] _Inverse("inverse", float) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				Cull[_Cull]
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct a2v
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float4 pos : SV_POSITION;
					float3 normal : TEXCOORD0;
					float3 wPos:TEXCOORD1;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _Cull;
				float _Inverse;
				v2f vert(a2v v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.normal = v.normal;
					o.wPos = mul(unity_ObjectToWorld, v.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float3 nor = normalize(i.normal);
					float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.wPos);
					float3 refl = (_Inverse > 0 ? 1 : -1) * reflect(viewDir,nor);
					float u = atan(refl.x / refl.z) / UNITY_PI;
					u = (u + 0.5)*0.5;
					if (refl.z >= 0)
						u += 0.5;
					float v = acos(refl.y) / UNITY_PI;
					float2 uv = float2(u, v)*_MainTex_ST.xy + _MainTex_ST.zw;
					fixed4 col = tex2D(_MainTex,uv,float2(0,0), float2(0,0));
					return col;
				}
				ENDCG
			}
		}
}
