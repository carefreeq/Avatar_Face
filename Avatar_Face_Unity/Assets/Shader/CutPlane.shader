// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "QQ/CutPlane"
{
	Properties
	{
		_Color("Color",Color) = (.5,.5,.5,1)
		_MainTex("Texture", 2D) = "white" {}
		_Height("Height",float) = 0

	}
		SubShader
		{
			Tags { "RenderType" = "Transparent"
			"Queue" = "Transparent"
			}
			LOD 100

			Pass
			{
				Zwrite Off
				Blend SrcAlpha OneMinusSrcAlpha
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
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
					float4 wPos:TEXCOORD1;
				};

				fixed4 _Color;
				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _Height;
				v2f vert(a2v v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.wPos = mul(unity_ObjectToWorld, v.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv)*_Color*2;
					if (i.wPos.y > _Height)
						col.a = 0;
					return col;
				}
				ENDCG
			}
		}
}
