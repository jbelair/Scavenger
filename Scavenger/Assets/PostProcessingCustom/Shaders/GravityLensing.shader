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
				float4 interpolatedRay : TEXCOORD1;
			};
			
			sampler2D _MainTex;
			uniform sampler2D_float _CameraDepthTexture;

			// for fast world space reconstruction
			uniform float4x4 _FrustumCornersWS;
			uniform float4 _CameraWS;

			float _Distance;
			float _Radius;
			float _Singularity;
			float _Sharpness;
			half4 _Colour;

			float3 _Origin;

			float ComputeDistance(float3 camDir, float zdepth)
			{
				float dist = zdepth * _ProjectionParams.z;
				dist -= _ProjectionParams.y;
				return dist;
			}

			v2f vert(appdata v)
			{
				v2f o;
				half index = v.vertex.z;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.interpolatedRay = _FrustumCornersWS[(int)index];
				o.interpolatedRay.w = index;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				/*float rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
				float dpth = Linear01Depth(rawDepth);
				float4 wsDir = dpth * i.interpolatedRay;
				float4 wsPos = _CameraWS + wsDir;

				if (ComputeDistance(wsDir, dpth) >= _Distance)
				{*/
					float2 offset = i.uv - _Origin; // We shift our pixel to the desired position
					float2 ratio = { _ScreenParams.y / _ScreenParams.x, 1 }; // determines the aspect ratio
					float rad = length(offset / ratio); // the distance from the conventional "center" of the screen.
					float deformation = 1 / pow(rad*pow(_Distance,0.5),2)*_Radius * 2;

					offset = offset * (1 - deformation);

					offset += _Origin;

					//// Modified https://unionassets.com/blog/the-effect-of-the-gravitational-lens-195 to include event horizon
					//float rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, offset);
					//float dpth = Linear01Depth(rawDepth);
					//float4 wsDir = dpth * i.interpolatedRay;
					//float4 wsPos = _CameraWS + wsDir;

					//if (ComputeDistance(wsDir, dpth) >= _Distance)
					//{
						fixed4 col = tex2D(_MainTex, offset);
						if (rad * _Distance < _Singularity)
							col = fixed4(0, 0, 0, 1);
						
						//fixed4 col = lerp(tex2D(_MainTex, offset), _Colour, pow(saturate(deformation), _Sharpness) * _Singularity);
						//fixed4 col = lerp(tex2D(_MainTex, offset), _Colour, clamp(0, 1, deformation - _Singularity));
						return col;
					//}
					//else
					//{
					//	// sample the default reflection cubemap, using the reflection vector
					//	float3 reflectionDir = ((wsPos - _Origin) * (1 - deformation) + _Origin);
					//	half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, reflectionDir);//i.interpolatedRay);
					//	// decode cubemap data into actual color
					//	half3 skyColour = DecodeHDR(skyData, unity_SpecCube0_HDR);
					//	return half4(skyColour, 1);
					//}
				/*}
				else
					return tex2D(_MainTex, i.uv);*/
			}
			ENDCG
		}
	}
}
