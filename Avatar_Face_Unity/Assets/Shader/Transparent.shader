// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "QQ/Transparent"
{
	Properties
	{
		_Color("color",Color) = (0.5,0.5,0.5,1)
		_MainTex("texture", 2D) = "white" {}
		_Cut("cut",Range(0,1)) = 0.5
		[Enum(Off, 0, On, 1)] _ZWrite("ZWrite", float) = 1
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", float) = 2
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend Mode", float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend Mode", float) = 10
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
			Blend[_SrcBlend][_DstBlend]
			ZWrite[_ZWrite]
			Cull[_Cull]
			LOD 100
			Pass
			{
			Tags{
			"LightMode" = "ForwardBase"
			}
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fwdbase_fullshadows
				#pragma multi_compile_fog
				#include "UnityCG.cginc"
				#include "AutoLight.cginc"
			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal:TEXCOORD1;
				float4 wPos:TEXCOORD2;
				UNITY_FOG_COORDS(3)
				LIGHTING_COORDS(4, 5)
			};
			fixed4 _LightColor0;
			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Cut;
			v2f vert(a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.wPos = mul(unity_ObjectToWorld, v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				UNITY_TRANSFER_FOG(o,o.pos);
				TRANSFER_VERTEX_TO_FRAGMENT(o)
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float4 tex = tex2D(_MainTex, i.uv);
				tex.a = max(tex.a - _Cut, 0) / (1 - _Cut);
				float3 lightColor = LIGHT_ATTENUATION(i)*_LightColor0.rgb;
				float diff = max(0, dot(i.normal, normalize(_WorldSpaceLightPos0.xyz)));
				float3 diffColor = diff * lightColor * tex.rgb;
				float4 col = float4(diffColor,tex.a);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
		}
}
