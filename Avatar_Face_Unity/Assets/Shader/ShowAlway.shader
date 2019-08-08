// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "QQ/ShowAlway"
{
	Properties
	{
		_Color("Color", Color) = (1,0,0,1)
		_MainTex("MainTex",2D) = "white"
	}
		SubShader
	{
		Tags{
		"Queue" = "Overlay"
		"RenderType" = "Overlay"
		}
		LOD 100
		Pass
		{
			Ztest Always
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

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

			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			v2f vert(a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return tex2D(_MainTex,i.uv)*_Color;
			}
			ENDCG
		}
	}
}
