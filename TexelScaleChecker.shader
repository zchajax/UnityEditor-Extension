Shader "FTPCustom/VertexInput/TexelScaleChecker"
{
	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }

		Pass
		{
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0
			#include "UnityCG.cginc"

			sampler2D _Checker;
			float4 _MainTex_TexelSize;
			float4 _MainTex_ST;

			float _Scale;
			float _Resolution;

			struct appdata
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};			

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 posWS : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				o.posWS = mul(unity_ObjectToWorld, v.vertex);

				return o;
			};


			float len(float a, float b)
			{
				return sqrt(a * a + b * b);
			}

			half4 frag(v2f i) : SV_Target
			{
				float2 size = float2(_MainTex_TexelSize.z, _MainTex_TexelSize.w);

				float a = len(ddx(i.uv.x) * (size.x / _Resolution), ddx(i.uv.y) * (size.y / _Resolution)) / length(ddx(i.posWS.xyz));
				a += len(ddy(i.uv.x) * (size.x / _Resolution), ddy(i.uv.y) * (size.y / _Resolution)) / length(ddy(i.posWS.xyz));
				a /= 2;

				half3 c;
				if (a >= 1.0)
				{
					c = lerp(half3(0.0, 1.0, 0.0), half3(1.0, 0.0, 0.0), (a - 1.0));
				}
				else
				{	
					c = lerp(half3(0.0, 1.0, 0.0), half3(0, 0, 1), (1.0 - a));
				}

				return half4(c, 1);	
			}

			ENDCG
		}
	}	
}
