// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "QQ/ShaderTest"
{
	Properties
	{
		_Color0("Color0",Color) = (1,1,1,1)
		_Color1("Color1",Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
	}
		CGINCLUDE
	#include "UnityCG.cginc"	
	uniform float4 _Color0;
	uniform float4 _Color1;
	uniform sampler2D _MainTex;
	uniform float4 _MainTex_ST;
	uniform float4 _Depth;
	struct a2v
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	v2f vert(a2v v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		return o;
	}
	ENDCG
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100
		Pass
		{
			Tags{ "LightMode" = "Always" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
