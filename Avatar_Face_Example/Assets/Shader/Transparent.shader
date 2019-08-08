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
			LOD 100

			Pass
			{ 
				ZWrite On
				ColorMask 0
			}
			Pass
			{
			Tags{
				"LightMode" = "ForwardBase"
				}
				Blend[_SrcBlend][_DstBlend]
				ZWrite[_ZWrite]
				Cull[_Cull]
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
			Pass{
				Name "ShadowCaster"
				Tags{
				"LightMode" = "ShadowCaster"
			}
				Offset 1, 1

				CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define UNITY_PASS_SHADOWCASTER
#include "UnityCG.cginc"
#pragma target 3.0
				uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
			uniform fixed4 _Color;
			struct VertexInput {
				float4 vertex : POSITION;
				float2 texcoord0 : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoord2 : TEXCOORD2;
			};
			struct VertexOutput {
				V2F_SHADOW_CASTER;
				float2 uv0 : TEXCOORD1;
				float2 uv1 : TEXCOORD2;
				float2 uv2 : TEXCOORD3;
				float4 posWorld : TEXCOORD4;
			};
			VertexOutput vert(VertexInput v) {
				VertexOutput o = (VertexOutput)0;
				o.uv0 = v.texcoord0;
				o.uv1 = v.texcoord1;
				o.uv2 = v.texcoord2;
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex);
				TRANSFER_SHADOW_CASTER(o)
					return o;
			}
			float4 frag(VertexOutput i) : COLOR{
				/////// Vectors:
				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
				clip(_Color.a - 0.5);
				SHADOW_CASTER_FRAGMENT(i)
			}
				ENDCG
				}
	}
}
