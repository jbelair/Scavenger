// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Surfaces/Shield"
{
	Properties
	{
		_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Particle Texture", 2D) = "white" {}
		_InvFade("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_ExponentCurve("Shield Exponent Factor", Float) = 2.0
		_ShellStrength("Shield Shell Strength", Float) = 1
		_CoreStrength("Shield Core Strength", Float) = 0.1
		_TotalStrength("Shield Total Strength", Float) = 1
		_ImpactCurve("Impact Exponent Factor", Float) = 100
		_Pos0("Impact 0", Vector) = (0,0,0,0)
		_Pos1("Impact 1", Vector) = (0,0,0,0)
		_Pos2("Impact 2", Vector) = (0,0,0,0)
		_Pos3("Impact 3", Vector) = (0,0,0,0)
	}

		Category
		{
			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Blend SrcAlpha One
			AlphaTest Greater .01
			ColorMask RGB
			Cull Off Lighting Off ZWrite Off
			BindChannels
			{
				Bind "Color", color
				Bind "Vertex", vertex
				Bind "TexCoord", texcoord
			}

			// ---- Fragment program cards
			SubShader
			{
				Pass
				{
					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma fragmentoption ARB_precision_hint_fastest
					#pragma multi_compile_particles

					#include "UnityCG.cginc"
					#include "Assets/Surfaces/Shaders/noise.cginc"

					sampler2D _MainTex;
					fixed4 _TintColor;

					struct appdata_t
					{
						float4 vertex : POSITION;
						fixed4 color : COLOR;
						float2 texcoord : TEXCOORD0;
						float3 normal : NORMAL;
					};

					struct v2f
					{
						float4 vertex : POSITION;
						fixed4 color : COLOR;
						float2 texcoord : TEXCOORD0;
						#ifdef SOFTPARTICLES_ON
						float4 projPos : TEXCOORD1;
						#endif
						float3 normal : NORMAL;
						float3 viewDir : TEXCOORD3;
						float4 impacts : TEXCOORD4;
						float4 world : TEXCOORD5;
					};

					float4 _MainTex_ST;

					float _ImpactCurve;

					float4 _Pos0;
					float4 _Pos1;
					float4 _Pos2;
					float4 _Pos3;

					v2f vert(appdata_t v)
					{
						v2f o;

						o.normal = v.normal;
						if (_Pos0.w != 0)
						{
							o.impacts.x = (dot(normalize(_Pos0.xyz), o.normal.xyz) + 1) / 2;
							v.vertex.xyz -= v.normal * clamp(pow(o.impacts.x, _ImpactCurve) * _Pos0.w, -1, 1);
						}
						if (_Pos1.w != 0)
						{
							o.impacts.y = (dot(normalize(_Pos1.xyz), o.normal.xyz) + 1) / 2;
							v.vertex.xyz -= v.normal * clamp(pow(o.impacts.y, _ImpactCurve) * _Pos1.w, -1, 1);
						}
						if (_Pos2.w != 0)
						{
							o.impacts.z = (dot(normalize(_Pos2.xyz), o.normal.xyz) + 1) / 2;
							v.vertex.xyz -= v.normal * clamp(pow(o.impacts.z, _ImpactCurve) * _Pos2.w, -1, 1);
						}
						if (_Pos3.w != 0)
						{
							o.impacts.w = (dot(normalize(_Pos3.xyz), o.normal.xyz) + 1) / 2;
							v.vertex.xyz -= v.normal * clamp(pow(o.impacts.w, _ImpactCurve) * _Pos3.w, -1, 1);
						}
						//o.normal = normalize(v.vertex);
						o.vertex = UnityObjectToClipPos(v.vertex);
						
						#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos(o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
						#endif
						o.color = v.color;
						o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
						o.world = mul(unity_ObjectToWorld, v.vertex);
						o.viewDir = normalize(_WorldSpaceCameraPos.xyz - o.world.xyz);
						return o;
					}

					sampler2D _CameraDepthTexture;
					float _InvFade;
					float _ExponentCurve;
					float _ShellStrength;
					float _CoreStrength;
					float _TotalStrength;

					fixed4 frag(v2f i) : COLOR
					{
						_TotalStrength = saturate(_TotalStrength);

						#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
						float partZ = i.projPos.z;
						float fade = saturate(_InvFade * (sceneZ - partZ));
						i.color.a *= fade;
						#endif
						float freznel = abs(dot(normalize(ObjSpaceViewDir(float4(i.viewDir,1))), i.normal));
						float iFreznel = pow(1 - freznel, _ExponentCurve);
						freznel = pow(freznel, _ExponentCurve);

						float noise = saturate(snoise4(float4(i.world.xyz * 0.5f, _Time.x), 2) + 1 * _TotalStrength);

						i.color.a *= iFreznel * _ShellStrength + freznel * _CoreStrength;

						if (_Pos0.w != 0)
							i.color.a += saturate(pow(i.impacts.x, _ExponentCurve));
						if (_Pos1.w != 0)
							i.color.a += saturate(pow(i.impacts.y, _ExponentCurve));
						if (_Pos2.w != 0)
							i.color.a += saturate(pow(i.impacts.z, _ExponentCurve));
						if (_Pos3.w != 0)
							i.color.a += saturate(pow(i.impacts.w, _ExponentCurve));

						float preNoise = pow(noise, 1 / _ExponentCurve);
						i.color.a *= max(iFreznel, max(freznel, preNoise + (preNoise > 0) ? pow(1 - noise, _ExponentCurve) : 0)) * _TotalStrength;//saturate(pow(_TotalStrength, 1 / _ExponentCurve));;

						//i.color.a = saturate(i.color.a - saturate((pow(noise, _ImpactCurve) + 1) / 2) * (1 - _TotalStrength));
						//i.color.a *= saturate(pow(1 - noise, _ImpactCurve) - pow(1 - noise, _ExponentCurve));

						return 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
					}
					ENDCG
				}
			}

			// ---- Dual texture cards
			SubShader
			{
				Pass
				{
					SetTexture[_MainTex]
					{
						constantColor[_TintColor]
						combine constant * primary
					}
					SetTexture[_MainTex]
					{
						combine texture * previous DOUBLE
					}
				}
			}

						// ---- Single texture cards (does not do color tint)
						SubShader
						{
							Pass
							{
								SetTexture[_MainTex]
								{
									combine texture * primary
								}
							}
						}
		}
}