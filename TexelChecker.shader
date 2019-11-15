Shader "DrawMode/TexelChecker"
{
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		
		Pass
		{
			ZWrite On
			CGPROGRAM
			#pragma vertex vect
			#pragma fragment frag
			#pragma target 5.0
			#pragma "UnityCG.cginc"

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
			};

			v2f vert ( appdata v )
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			half4 frag( v2f i ) : SV_Target
			{
				float2 uvScale = _MainTex_TexelSize.zw * _Scale;
				float4 color = tex2D(_Checker, float2(i.uv.x * uvScale.x, i.uv.y * uvScale.y));
				return half4(color.xyz, 1);
			}
			ENDCG
		}
	}
}
