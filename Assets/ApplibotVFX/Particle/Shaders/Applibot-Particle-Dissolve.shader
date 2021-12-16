Shader "Applibot/Particles/Dissolve"
{
	Properties
	{
		// Main Color
		_MainTex ("Base (RGBA)", 2D)															= "white" {}
		[HDR]_Color ("Tint Color (RGBA)", Color) = (1,1,1,1)

		// Dissolve
		_DissolveTex("Dissolve Texture (R)", 2D)												= "white" {}
		[Toggle]_InverseDissolveTex ("Inverse", int)											= 0
		[Enum(PROGRESS, 0, ALPHA, 1)]_VertexAlphaMode ("Vertex Alpha Mode", int)				= 0
		_DissolveProgress("Dissolve Progress", Range(0,1))										= 0
		_DissolveEdge("Edge Width", Range(0.01,0.5))											= 0.1
		_DissolveEdgeAlphaPower("Edge Alpha Power", Range(0.0, 1.0))							= 1.0

		// Edge Color
		[Toggle]_UseEdgeColor ("Use Edge Color", int)											= 0
		_DissolveEdgeRamp ("Edge Ramp (RGB)", 2D)												= "white" {}
		_DissolveEdgeLuminance ("Edge Luminance", Range(0.0, 20.0))								= 1.0

		// Reflection
		[Toggle]_UseReflection ("Use Reflection", int)											= 0
		_ReflectionRoughness ("Reflection Roughness", Range(0.0, 1.0))							= 1
		_ReflectionPower ("Reflection Power", Range(0.0, 1.0))									= 1.0

		// Blend Rate
		[Toggle]_UseBlendRateFromRGB("Blend Rate From RGB", int)								= 0
		[Toggle]_UseBlendRateFromRim("Blend Rate From Rim", int)								= 0
		_BlendRateRimSharpness("Blend Rate Rim Sharpness", float)								= 1
		[Toggle]_BlendRateRimInverse("Blend Rate Rim Inverse", int)								= 0
		[Toggle]_UseBlendRateFromHeight ("Blend Rate From Height", int)							= 0
		_BlendRateHeightStartOffset ("Blend Rate Height Start", float)								= 0
		_BlendRateHeightDistance ("Blend Rate Height Distance", float)							= 1
		[Toggle]_UseBlendRateFromZ ("Blend Rate From Z", int)									= 0
		_BlendRateFromZDistance ("Blend Rate From Z Distance", float)							= 1
		
		// Blend Mode
		[HideInInspector]_BlendMode("Blend Mode", int)											= 2
		[Enum(UnityEngine.Rendering.BlendMode)]_BlendSrc("Blend Src", int)						= 1
		[Enum(UnityEngine.Rendering.BlendMode)]_BlendDst("Blend Dst", int)						= 0
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
		[HideInInspector]_DissolveUCoord ("__dissolveucoord", int)								= 2
		[HideInInspector]_DissolveUSwizzle ("__dissolveuswizzle", int)							= 0
		[HideInInspector]_DissolveVCoord ("__dissolvevcoord", int)								= 2
		[HideInInspector]_DissolveVSwizzle ("__dissolvevswizzle", int)							= 0
		[HideInInspector]_DissolveProgressCoord ("__dissolveprogresscoord", int)				= 2
		[HideInInspector]_DissolveProgressSwizzle ("__dissolveprogressswizzle", int)			= 0
		[HideInInspector]_DissolveEdgeLuminanceCoord ("__dissolveedgeluminancecoord", int)		= 2
		[HideInInspector]_DissolveEdgeLuminanceSwizzle ("__dissolveedgeluminanceswizzle", int)	= 0
		[HideInInspector]_BlendRateRimSharpnessCoord ("__blendraterimsharpnesscoord", int)		= 2
		[HideInInspector]_BlendRateRimSharpnessSwizzle ("__blendraterimsharpnessswizzle", int)	= 0
		[HideInInspector]_ColorMagnificationCoord ("__colormagnificationcoord", int)			= 2
		[HideInInspector]_ColorMagnificationSwizzle ("__colormagnificationswizzle", int)		= 0
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent"  "IgnoreProjector"="True"}

		Cull [_CullMode]
		ZWrite [_UseZWrite]
		ZTest[_ZTest]
		Lighting Off
		Offset 0, [_ZOffset]
		Blend [_BlendSrc] [_BlendDst]
		ColorMask [_ColorMask]

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _ USE_EDGE_COLOR
			#pragma shader_feature _ USE_REFLECTION
			#pragma shader_feature _VERTEXALPHAMODE_PROGRESS _VERTEXALPHAMODE_ALPHA
			#pragma shader_feature _ BLEND_RATE_FROM_RGB
			#pragma shader_feature _ BLEND_RATE_FROM_RIM
			#pragma shader_feature _ BLEND_RATE_FROM_HEIGHT
			#pragma shader_feature _ BLEND_RATE_FROM_Z
			#pragma shader_feature _ BLENDMODE_ADD BLENDMODE_TRANSPARENTADD BLENDMODE_ALPHABLEND BLENDMODE_MULTIPLY BLENDMODE_TRANSPARENTMULTIPLY
			#pragma shader_feature _ _DITHER_TRANSPARENT
			#pragma shader_feature _ USE_HDR_THRESHOLD

			#include "UnityCG.cginc"
			#include "ApplibotParticleCG.cginc"

			struct appdata {
				half4 vertex		: POSITION;
				half4 normal		: NORMAL;
				half4 color			: COLOR;
				half4 texcoord		: TEXCOORD0;
				CUSTOM_DATA_VERTEX_INPUT(1, 2)
			};

			struct v2f {
				half4 color				: COLOR;
				half4 uv01				: TEXCOORD0;
				// x: progress, y: luminance, z: color magnification
				half4 params0			: TEXCOORD1;
				// x: view normal, y: world height, z: rim sharpness
				half4 params1			: TEXCOORD2;
				#ifdef USE_REFLECTION
					half3 reflectionDir	: TEXCOORD3;
				#endif
				#if BLEND_RATE_FROM_Z
					float4 projPos      : TEXCOORD4;
				#endif
			};

			// Main Color
			sampler2D _MainTex;
			half4 _MainTex_ST;
			half4 _Color;

			// Dissolve
			sampler2D _DissolveTex;
			half4 _DissolveTex_ST;
			int _InverseDissolveTex;
			half _DissolveProgress;
			half _DissolveEdge;
			half _DissolveEdgeAlphaPower;

			// Reflection
			half _ReflectionPower;
			half _ReflectionRoughness;

			// Edge Color
			sampler2D _DissolveEdgeRamp;
			half _DissolveEdgeLuminance;

			// Blend Rate
			half _BlendRateRimSharpness;
			half _BlendRateRimInverse;
			half _BlendRateHeightStart;
			half _BlendRateHeightStartOffset;
			half _BlendRateHeightDistance;
			half _BlendRateFromZDistance;
			UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
			
			// Blend Mode
			sampler3D _DitherMaskLOD;
			half _ColorMagnification;

			// Other
			half _HDRThresholdParticle;

			// For Custom Data
			CUSTOM_DATA_DEFINE_UV(_Main)
			CUSTOM_DATA_DEFINE_UV(_Dissolve)
			CUSTOM_DATA_DEFINE(_DissolveProgress)
			CUSTOM_DATA_DEFINE(_DissolveEdgeLuminance)
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
				o.uv01.zw		= TRANSFORM_TEX(v.texcoord.xy, _DissolveTex) + CUSTOM_DATA_VERTEX_GET_UV(_Dissolve);

				#if defined(BLEND_RATE_FROM_RIM) || defined(BLEND_RATE_FROM_HEIGHT) || defined(USE_REFLECTION)
					float4 worldPos	= mul(unity_ObjectToWorld, v.vertex);
				#endif
				#if defined(BLEND_RATE_FROM_RIM) || defined(USE_REFLECTION)
					half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				#endif
				#if defined(BLEND_RATE_FROM_RIM) || defined(USE_REFLECTION)
					half3 worldViewDir = normalize(_WorldSpaceCameraPos - worldPos);
				#endif

				// Dissolve
				o.params0.x		= CUSTOM_DATA_VERTEX_GET(_DissolveProgress);
				o.params0.y		= CUSTOM_DATA_VERTEX_GET(_DissolveEdgeLuminance);

				// Blend Rate
				#ifdef BLEND_RATE_FROM_RIM
					o.params1.x	= max(0.0, abs(dot(worldViewDir, worldNormal)));
					if (_BlendRateRimInverse == 0){
						o.params1.x		= 1.0 - o.params1.x;
					}
					o.params1.z		= _BlendRateRimSharpness + CUSTOM_DATA_VERTEX_GET(_BlendRateRimSharpness);
				#endif
				#ifdef BLEND_RATE_FROM_HEIGHT
					o.params1.y		= worldPos.y;
				#endif

				// Reflection
				#ifdef USE_REFLECTION
					o.reflectionDir	= reflect(-worldViewDir, worldNormal);
				#endif

				// Color Magnification
				o.params0.z		= CUSTOM_DATA_VERTEX_GET(_ColorMagnification);

				return o;
			}

			fixed4 frag (v2f i, UNITY_VPOS_TYPE vpos : VPOS) : SV_Target
			{
				half4 tex		= tex2D(_MainTex, i.uv01.xy);
				half4 color		= tex;
				color.rgb		*= _Color.rgb * i.color.rgb;

				// 適用率を求める
				half progress	= (_DissolveProgress + i.params0.x);
				#ifdef _VERTEXALPHAMODE_PROGRESS
					progress		*= i.color.a;
				#endif

				// ブレンド率を求める
				half blendRate	= tex2D(_DissolveTex, i.uv01.zw).r;
				blendRate		= smoothstep(blendRate + _DissolveEdge, blendRate - _DissolveEdge, progress);
				blendRate		= smoothstep(progress + _DissolveEdge, progress - _DissolveEdge, blendRate);
				blendRate		= pow(blendRate, _DissolveEdgeAlphaPower);
				if (_InverseDissolveTex == 1){
					blendRate		= 1.0 - blendRate;
				}

				// エッジ色を反映する
				#ifdef USE_EDGE_COLOR
					half4 edgeColor	= tex2D(_DissolveEdgeRamp, 1.0 - half2(blendRate, 0.5));
					edgeColor		= (color + edgeColor) * edgeColor * (_DissolveEdgeLuminance + i.params0.y);
					color			= lerp(edgeColor, color, blendRate);
				#endif

				// 明度をブレンド率に反映する
				#ifdef BLEND_RATE_FROM_RGB
					blendRate		*= (tex.r + tex.g + tex.b) / 3.0;
				#endif
				// リムの強さをブレンド率に反映する
				#ifdef BLEND_RATE_FROM_RIM
					blendRate		*= pow(i.params1.x, i.params1.z);
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

				// 頂点アルファを使う設定だったら適用
				#ifdef _VERTEXALPHAMODE_ALPHA
					blendRate		*= i.color.a;
				#endif

				// 色をブレンド
				color			= ApplyBlendRate(blendRate, color);
				color			= PreBlend(color);

				#ifdef _DITHER_TRANSPARENT
					// ディザ抜き
					vpos *= 0.25;
					clip(tex3D(_DitherMaskLOD, float3(vpos.xy, blendRate * color.a * 0.9375)).a - 0.5);
				#endif

				// 色に乗算値を適用
				color.rgb		*= _ColorMagnification + i.params0.z;

				#ifdef USE_HDR_THRESHOLD
					// 疑似HDR色に変換
					color.rgb		= GetHdrColor(color.rgb, 0, _HDRThresholdParticle);
				#endif

				return color;
			}
			ENDCG
		}
	}

	CustomEditor "Applibot.VFX.ParticleDissolveGUI"
}
