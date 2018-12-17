// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Surfaces/Shield"
{
	Properties
	{
		_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Particle Texture", 2D) = "white" {}
		_InvFade("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_Scale("Noise Scale", Vector) = (1,1,1)
		_Timescale("Timescale", Float) = 1
		_Type("Perlin | Turbulence | Normal", Int) = 0
		_Octaves("Noise Octaves", Float) = 2
		_Extrude("Face Extrude", Float) = 0
		_ExponentCurve("Shield Exponent Factor", Float) = 2.0
		_ShellStrength("Shield Shell Strength", Float) = 1
		_CoreStrength("Shield Core Strength", Float) = 0.1
		_TotalStrength("Shield Total Strength", Float) = 1
		_BreakStrength("Shield Fracture Strength", Float) = 100
		_ImpactCurve("Impact Exponent Factor", Float) = 100
		_Pos0("Impact 0", Vector) = (0,0,0,0)
		_Pos1("Impact 1", Vector) = (0,0,0,0)
		_Pos2("Impact 2", Vector) = (0,0,0,0)
		_Pos3("Impact 3", Vector) = (0,0,0,0)
		_WorldPosition("World Position", Vector) = (0, 0, 0, 0)
	}

		Category
		{
			Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
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
						float4 local: TEXCOORD6;
					};

					float4 _MainTex_ST;

					float _Extrude;
					float _Octaves;
					float _ImpactCurve;

					float4 _Pos0;
					float4 _Pos1;
					float4 _Pos2;
					float4 _Pos3;

					float4 _WorldPosition;

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
						v.vertex.xyz += v.normal * _Extrude;
						//o.normal = normalize(v.vertex);
						o.vertex = UnityObjectToClipPos(v.vertex);

						#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos(o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
						#endif
						o.color = float4(1, 1, 1, 1);//v.color;
						o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
						o.world = mul(unity_ObjectToWorld, v.vertex);
						o.local = o.world - _WorldPosition;
						o.viewDir = normalize(ObjSpaceViewDir(v.vertex));//normalize(_WorldSpaceCameraPos.xyz - o.world.xyz);
						return o;
					}

					sampler2D _CameraDepthTexture;
					int _Type;
					float3 _Scale;
					float _Timescale;
					float _InvFade;
					float _ExponentCurve;
					float _ShellStrength;
					float _CoreStrength;
					float _TotalStrength;
					float _BreakStrength;

					fixed4 frag(v2f i) : COLOR
					{
						_TotalStrength = saturate(_TotalStrength);

						#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
						float partZ = i.projPos.z;
						float fade = saturate(_InvFade * (sceneZ - partZ));
						i.color.a *= fade;
						#endif
						//float freznel = abs(dot(normalize(ObjSpaceViewDir(float4(i.viewDir,1))), i.normal));
						float freznel = abs(dot(float4(i.viewDir, 1), i.normal));
						float iFreznel = pow(1 - freznel, _ExponentCurve);
						freznel = pow(freznel, _ExponentCurve);

						float noise = snoise4(float4(i.local.x *_Scale.x, i.local.y *_Scale.y, i.local.z *_Scale.z, _Time.x * _Timescale), _Octaves);
						if (_Type == 1)
							noise = abs(noise);
						else if (_Type == 2)
							noise = (noise + 1) / 2;

						//noise = saturate(noise);
						noise = (1 - abs(noise) > pow(_TotalStrength, 1 / _ExponentCurve)) ? 0 : noise * _BreakStrength;
						noise = iFreznel * noise + freznel * noise + noise * _TotalStrength;

						if (_Pos0.w != 0)
							i.color.a += saturate(pow(i.impacts.x, _ExponentCurve));
						if (_Pos1.w != 0)
							i.color.a += saturate(pow(i.impacts.y, _ExponentCurve));
						if (_Pos2.w != 0)
							i.color.a += saturate(pow(i.impacts.z, _ExponentCurve));
						if (_Pos3.w != 0)
							i.color.a += saturate(pow(i.impacts.w, _ExponentCurve));

						i.color.a = lerp(noise, iFreznel * _ShellStrength + freznel * _CoreStrength, _TotalStrength) * _TotalStrength;

						//i.color.a *= iFreznel * _ShellStrength + freznel * _CoreStrength;

						//float preNoise = pow(noise, 1 / _ExponentCurve);
						//i.color.a = length(noise) > pow(1 - _TotalStrength, _ExponentCurve) ? pow(_TotalStrength, 1 / _ExponentCurve) : i.color.a * noise * _BreakStrength * pow(_TotalStrength, 1 / _ExponentCurve);//max(iFreznel, max(freznel, (preNoise + (preNoise > 0) ? pow(1 - noise, _ExponentCurve) : 0) * _BreakStrength * (1 - _TotalStrength))) * _TotalStrength;//saturate(pow(_TotalStrength, 1 / _ExponentCurve));;

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