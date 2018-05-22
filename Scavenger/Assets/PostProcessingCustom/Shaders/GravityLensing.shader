// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// Thanks https://unionassets.com/blog/the-effect-of-the-gravitational-lens-195

Shader "Hidden/Gravity Lensing"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Distance ("Distance", Float) = 1
		_Radius ("Radius", Float) = 1
		_Origin ("Origin", Vector) = (0,0,0)
		_Singularity ("Event Horizon", Range(0,1)) = 0
		_Sharpness ("Horizon Sharpness", Float) = 1
		[HDR]_Colour ("Singularity Colour", Color) = (0,0,0,1)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			float _Distance;
			float _Radius;
			float _Singularity;
			float _Sharpness;
			half4 _Colour;

			float3 _Origin;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 offset = i.uv - _Origin; // We shift our pixel to the desired position
				float2 ratio = { _ScreenParams.y / _ScreenParams.x, 1 }; // determines the aspect ratio
				float rad = length(offset / ratio); // the distance from the conventional "center" of the screen.
				float deformation = 1 / pow(rad*pow(_Distance,0.5),2)*_Radius * 2;

				offset = offset*(1 - deformation);

				offset += _Origin;

				// Modified https://unionassets.com/blog/the-effect-of-the-gravitational-lens-195 to include event horizon
				fixed4 col = lerp(tex2D(_MainTex, offset), _Colour, pow(saturate(deformation), _Sharpness) * _Singularity);
				//fixed4 col = lerp(tex2D(_MainTex, offset), _Colour, clamp(0, 1, deformation - _Singularity));
				return col;
			}
			ENDCG
		}
	}
}
