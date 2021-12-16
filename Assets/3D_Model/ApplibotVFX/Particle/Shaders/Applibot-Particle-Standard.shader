Shader "Applibot/Particles/Standard"
{
	Properties
	{
		// Main Color
		_MainTex ("Base (RGBA)", 2D)															= "white" {}
		[HDR]_Color ("Tint Color (RGBA)", Color)												= (1,1,1,1)

		// Distortion
		[Toggle]_UseDist ("Use Distortion", int)												= 0
		_DistTex ("Distortion (R)", 2D)															= "white" {}
		_DistPow ("Distortion Power", float)													= 0

		// Mask
		[Toggle]_UseMask ("Use Mask", int)														= 0
		_MaskTex ("Mask (R)", 2D)																= "white" {}
		_MaskPow ("Mask Power", float) = 0

		// Reflection
		[Toggle]_UseReflection("Use Reflection", int) = 0
		_ReflectionPower("Reflection Power", Range(0.0, 1.0)) = 1.0
		_ReflectionRoughness("Reflection Roughness", Range(0.0, 1.0)) = 1

		// Blend Rate
		[Toggle]_UseBlendRateFromRGB("Blend Rate From RGB", int)								= 0
		[Toggle]_UseBlendRateFromRim("Blend Rate From Rim", int)								= 0
		_BlendRateRimSharpness("Blend Rate Rim Sharpness", float)								= 1
		[Toggle]_BlendRateRimInverse("Blend Rate Rim Inverse", int)								= 0
		[Toggle]_UseBlendRateFromHeight ("Blend Rate From Height", int)							= 0
		_BlendRateHeightStartOffset ("Blend Rate Height Start", float)							= 0
		_BlendRateHeightDistance ("Blend Rate Height Distance", float)							= 1
		[Toggle]_UseBlendRateFromZ ("Blend Rate From Z", int)									= 0
		_BlendRateFromZDistance ("Blend Rate From Z Distance", float)							= 1
		
		// Blend Mode
		[HideInInspector]_BlendMode("Blend Mode", int)											= 2
		[HideInInspector]_StartColor("Start Color", Color)										= (0,0,0,0)
		_BlendSrc("Blend Src", int)																= 1
		_BlendDst("Blend Dst", int)																= 0
		[Toggle]_UseDitherTransparent("Use Dither Transparent", int)							= 0
		_ColorMagnification ("Color Magnification", float)										= 1

		// Other
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("Cull Mode", int)						= 0
		[Enum(UnityEngine.Rendering.CompareFunction)]_ZTest("ZTest Mode", int)					= 4
		[Toggle]_UseZWrite("ZWrite", int)														= 0
		_ZOffset("ZOffset", float)																= 0
		_HDRThresholdParticle("HDR Threshold", Range(0.0, 1.0))									= 0.9
		[HideInInspector]_ColorMask ("Color Mask", Float)										= 14

		// For Custom Data
		[HideInInspector]_MainUCoord ("__mainucoord", int)										= 2
		[HideInInspector]_MainUSwizzle ("__mainuswizzle", int)									= 0
		[HideInInspector]_MainVCoord ("__mainvcoord", int)										= 2
		[HideInInspector]_MainVSwizzle ("__mainvswizzle", int)									= 0
		[HideInInspector]_DistUCoord ("__dsitucoord", int)										= 2
		[HideInInspector]_DistUSwizzle ("__distuswizzle", int)									= 0
		[HideInInspector]_DistVCoord ("__distvcoord", int)										= 2
		[HideInInspector]_DistVSwizzle ("__distvswizzle", int)									= 0
		[HideInInspector]_DistPowCoord ("__distpowcoord", int)									= 2
		[HideInInspector]_DistPowSwizzle ("__dispowswizzle", int)								= 0
		[HideInInspector]_MaskUCoord ("__maskucoord", int)										= 2
		[HideInInspector]_MaskUSwizzle ("__maskuswizzle", int)									= 0
		[HideInInspector]_MaskVCoord ("__maskvcoord", int)										= 2
		[HideInInspector]_MaskVSwizzle ("__maskvswizzle", int)									= 0
		[HideInInspector]_MaskPowCoord ("__maskpowcooord", int)									= 2
		[HideInInspector]_MaskPowSwizzle ("__maskpowswizzle", int)								= 0
		[HideInInspector]_BlendRateRimSharpnessCoord ("__blendraterimsharpnesscoord", int)		= 2
		[HideInInspector]_BlendRateRimSharpnessSwizzle ("__blendraterimsharpnessswizzle", int)	= 0
		[HideInInspector]_ColorMagnificationCoord ("__colormagnificationcoord", int)			= 2
		[HideInInspector]_ColorMagnificationSwizzle ("__colormagnificationswizzle", int)		= 0
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent"  "IgnoreProjector"="True"}

		Pass
		{
			Cull [_CullMode]
			ZWrite [_UseZWrite]
			ZTest[_ZTest]
			Lighting Off
			Offset 0, [_ZOffset]
			Blend [_BlendSrc] [_BlendDst]
			ColorMask [_ColorMask]

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _ USE_DISTORTION
			#pragma shader_feature _ USE_REFLECTION
			#pragma shader_feature _ USE_MASK
			#pragma shader_feature _ BLEND_RATE_FROM_RGB
			#pragma shader_feature _ BLEND_RATE_FROM_RIM
			#pragma shader_feature _ BLEND_RATE_FROM_HEIGHT
			#pragma shader_feature _ BLEND_RATE_FROM_Z
			#pragma shader_feature _ BLENDMODE_ADD BLENDMODE_TRANSPARENTADD BLENDMODE_ALPHABLEND BLENDMODE_MULTIPLY BLENDMODE_TRANSPARENTMULTIPLY
			#pragma shader_feature _ _DITHER_TRANSPARENT
			#pragma shader_feature _ USE_HDR_THRESHOLD

			#include "UnityCG.cginc"
			#include "ApplibotParticleCG.cginc"

			struct appdata
			{
				float4 vertex			: POSITION;
				half4 texcoord			: TEXCOORD0;
				CUSTOM_DATA_VERTEX_INPUT(1, 2)
				half4 normal			: NORMAL;
				half4 color				: COLOR;
			};

			struct v2f
			{
				// xy: main, zw: distortion
				half4 uv01				: TEXCOORD0;
				// xy: mask
				half4 uv23				: TEXCOORD1;
				half4 params0			: TEXCOORD2;
				half4 params1			: TEXCOORD3;
				#ifdef USE_REFLECTION
					half3 reflectionDir	: TEXCOORD4;
				#endif
				#if BLEND_RATE_FROM_Z
					float4 projPos      : TEXCOORD5;
				#endif
				half4 color: COLOR0;
			};

			// Main Color
			sampler2D _MainTex;
			half4 _MainTex_ST;
			half4 _Color;

			// Distortion
			sampler2D _DistTex;
			half4 _DistTex_ST;
			half _DistPow;

			// Reflection
			half _ReflectionPower;
			half _ReflectionRoughness;

			// Mask
			sampler2D _MaskTex;
			half4 _MaskTex_ST;
			half _MaskPow;

			// Blend Rate
			half _BlendRateRimSharpness;
			half _BlendRateRimInverse;
			half _BlendRateHeightStart;
			half _BlendRateHeightStartOffset;
			half _BlendRateHeightDistance;
			half _BlendRateFromZDistance;
			UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
			
			// Blend Mode
			half _BlendMode;
			sampler3D _DitherMaskLOD;
			half4 _StartColor;
			half _ColorMagnification;

			// Other
			half _HDRThresholdParticle;

			// For Custom Data
			CUSTOM_DATA_DEFINE_UV(_Main)
			CUSTOM_DATA_DEFINE_UV(_Dist)
			CUSTOM_DATA_DEFINE(_DistPow)
			CUSTOM_DATA_DEFINE_UV(_Mask)
			CUSTOM_DATA_DEFINE(_MaskPow)
			CUSTOM_DATA_DEFINE(_BlendRateRimSharpness)
			CUSTOM_DATA_DEFINE(_ColorMagnification)

			v2f vert (appdata v, out float4 vertex : SV_POSITION)
			{
				v2f o			= (v2f)0;
				CUSTOM_DATA_VERTEX_INITIALIZATION(v, o)
				vertex		= UnityObjectToClipPos(v.vertex);
				#if BLEND_RATE_FROM_Z
					o.projPos   = ComputeScreenPos(vertex);
					COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.color			= v.color;
				o.uv01.xy		= TRANSFORM_TEX(v.texcoord.xy, _MainTex) + CUSTOM_DATA_VERTEX_GET_UV(_Main);

				#if defined(BLEND_RATE_FROM_RIM) || defined(BLEND_RATE_FROM_HEIGHT) || defined(USE_REFLECTION)
					float4 worldPos	= mul(unity_ObjectToWorld, v.vertex);
				#endif
				#if defined(BLEND_RATE_FROM_RIM) || defined(USE_REFLECTION)
					half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				#endif
				#if defined(BLEND_RATE_FROM_RIM) || defined(USE_REFLECTION)
					half3 worldViewDir = normalize(_WorldSpaceCameraPos - worldPos);
				#endif

				// Distortion
				#ifdef USE_DISTORTION
					o.uv01.zw		= TRANSFORM_TEX(v.texcoord.xy, _DistTex) + CUSTOM_DATA_VERTEX_GET_UV(_Dist);
					o.params0.x		= CUSTOM_DATA_VERTEX_GET(_DistPow);
				#endif

				// Mask
				#ifdef USE_MASK
					o.uv23.xy		= TRANSFORM_TEX(v.texcoord.xy, _MaskTex) + CUSTOM_DATA_VERTEX_GET_UV(_Mask);
					o.params0.y		= CUSTOM_DATA_VERTEX_GET(_MaskPow);
				#endif

				// Blend Rate
				#ifdef BLEND_RATE_FROM_RIM
					o.params1.x	= max(0.0, abs(dot(worldViewDir, worldNormal)));
					if (_BlendRateRimInverse == 0){
						o.params1.x		= 1.0 - o.params1.x;
					}
					o.params0.z		= _BlendRateRimSharpness + CUSTOM_DATA_VERTEX_GET(_BlendRateRimSharpness);
				#endif
				#ifdef BLEND_RATE_FROM_HEIGHT
					o.params1.y		= worldPos.y;
				#endif

				// Reflection
				#ifdef USE_REFLECTION
					o.reflectionDir	= reflect(-worldViewDir, worldNormal);
				#endif

				// Color Magnification
				o.params0.w		= CUSTOM_DATA_VERTEX_GET(_ColorMagnification);

				return o;
			}

			fixed4 frag (v2f i, UNITY_VPOS_TYPE vpos : VPOS) : SV_Target
			{
				half blendRate		= 1.0;

				// Distortionの値を計算する
				half dist			= 0.0;
				#ifdef USE_DISTORTION
					half2 distUv		= i.uv01.zw;
					half distPow		= _DistPow + i.params0.x;
					dist				= (tex2D(_DistTex, distUv)).r * distPow;
				#endif

				// Distortionを適用したテクスチャ色を得る
				half4 tex			= tex2D(_MainTex, i.uv01.xy + dist);

				// ベース色を計算する
				half4 color			= 0;
				#ifdef BLEND_RATE_FROM_RGB
					// 頂点色xTintを色とする
					color			= i.color * _Color;
				#else
					// テクスチャ色 x 頂点色 x Tintを色とする
					color			= tex * i.color * _Color;
				#endif

				// Maskの値を計算する
				#if USE_MASK
					blendRate		*= pow(tex2D(_MaskTex, i.uv23.xy).r, _MaskPow + i.params0.y);
				#endif

				// 明度をブレンド率に反映する
				#ifdef BLEND_RATE_FROM_RGB
					blendRate		*= (tex.r + tex.g + tex.b) / 3.0;
				#endif
				// リムの強さをブレンド率に反映する
				#ifdef BLEND_RATE_FROM_RIM
					blendRate		*= pow(i.params1.x, i.params0.z);
				#endif
				// 高さをブレンド率に反映する
				#if BLEND_RATE_FROM_HEIGHT
					blendRate		*= saturate((i.params1.y - _BlendRateHeightStart + _BlendRateHeightStartOffset) / _BlendRateHeightDistance);
				#endif
				// Z差分をブレンド率に反映する
				#if BLEND_RATE_FROM_Z
					float sceneZ    = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
					float partZ     = i.projPos.z;
					float fade      = saturate((2.0 - _BlendRateFromZDistance) * (sceneZ - partZ));// From HeightのDistanceと操作感を合わせたい要望のため2-1(デフォ)=1とする
					blendRate       *= fade;
				#endif

				// リフレクションの影響を加算合成として反映する
				#ifdef USE_REFLECTION
					i.reflectionDir			= normalize(i.reflectionDir);
					half4 reflectionColor	= UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, i.reflectionDir, _ReflectionRoughness * 8);
					color.rgb				+= reflectionColor.rgb * _ReflectionPower;
				#endif

				// 色を事前ブレンド
				color			= ApplyBlendRate(blendRate, color);
				color			= PreBlend(color);

				#ifdef _DITHER_TRANSPARENT
					// ディザ抜き
					vpos *= 0.25;
					clip(tex3D(_DitherMaskLOD, float3(vpos.xy, blendRate * color.a * 0.9375)).a - 0.5);
				#endif

				// 色に乗算値を適用
				color.rgb		*= _ColorMagnification + i.params0.w;

				#ifdef USE_HDR_THRESHOLD
					// 疑似HDR色に変換
					color.rgb = GetHdrColor(color.rgb, 0, _HDRThresholdParticle);
				#endif

				return color;
			}
			ENDCG
		}
	}

	CustomEditor "Applibot.VFX.ParticleStandardGUI"
}
