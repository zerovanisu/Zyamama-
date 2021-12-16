Shader "Applibot/Particles/Distortion (GrabPass)"
{
	Properties
	{
		[Normal]_NormalTex ("Normalmap", 2D)											= "white" {}
		[PowerSlider(5.0)] _DistPow ("Distortion Power", Range(0,10))					= 0
		[Toggle(USE_MASK)]_UseMask ("Use Mask", int)									= 0
		_MaskTex ("Alpha Mask", 2D)														= "white" {}

		// For Custom Data
		[HideInInspector]_DistUCoord ("Distortion U Custom Coord", int)					= 2
		[HideInInspector]_DistUSwizzle ("Distortion U Custom Coord Swizzle", int)		= 0
		[HideInInspector]_DistVCoord ("Distortion V Custom Coord", int)					= 2
		[HideInInspector]_DistVSwizzle ("Distortion V Custom Coord Swizzle", int)		= 0
		[HideInInspector]_DistPowCoord ("Distortion Power Custom Coord", int)			= 2
		[HideInInspector]_DistPowSwizzle ("Distortion Power Custom Coord Swizzle", int)	= 0
	}

	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent"  "IgnoreProjector"="True"}

		Cull Back
		ZWrite Off
		ZTest LEqual
		Lighting Off
		Blend One Zero
		ColorMask RGB

		// LWRPを使う場合はこれをコメントアウトしてください
		GrabPass { "_CameraOpaqueTexture" }

		Pass {

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _ USE_MASK

			#include "UnityCG.cginc"
			#include "ApplibotParticleCG.cginc"

			struct appdata {
				half4 vertex			: POSITION;
				half4 texcoord			: TEXCOORD0;
				CUSTOM_DATA_VERTEX_INPUT(1, 2)
				half4 color				: COLOR;
			};

			struct v2f {
				half4 vertex			: SV_POSITION;
				half4 uv01				: TEXCOORD0;
				half4 grabPos			: TEXCOORD1;
				half distPow			: TEXCOORD2;
				half4 color				: COLOR;
			};

			sampler2D _NormalTex;
			sampler2D _CameraOpaqueTexture;
			sampler2D _MaskTex;
			half4 _NormalTex_ST;
			half4 _MaskTex_ST;
			half _DistPow;
			CUSTOM_DATA_DEFINE_UV(_Dist)
			CUSTOM_DATA_DEFINE(_DistPow)

			v2f vert (appdata v)
			{
				v2f o					= (v2f)0;
				CUSTOM_DATA_VERTEX_INITIALIZATION(v, o)
				o.vertex				= UnityObjectToClipPos(v.vertex);
				o.grabPos				= ComputeGrabScreenPos(o.vertex);
				o.color					= v.color;

				o.uv01.xy				= TRANSFORM_TEX(v.texcoord.xy, _NormalTex) + CUSTOM_DATA_VERTEX_GET_UV(_Dist);
				o.distPow				= _DistPow + CUSTOM_DATA_VERTEX_GET(_DistPow);
				#ifdef USE_MASK
					o.uv01.zw			= TRANSFORM_TEX(v.texcoord.xy, _MaskTex);
				#endif

				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				half2 grabUv			= half2(i.grabPos.x / i.grabPos.w, i.grabPos.y / i.grabPos.w);
				half3 normal			= UnpackNormal(tex2D(_NormalTex, i.uv01.xy));
				#if defined(UNITY_NO_DXT5nm)
					normal				= normalize(normal);
				#endif

				half distPow			= i.distPow * i.color.a;
				#ifdef USE_MASK
					distPow				*= tex2D(_MaskTex, i.uv01.zw).r;
				#endif
				grabUv					+= normal.xy * distPow;
				half4 color				= tex2D(_CameraOpaqueTexture, grabUv);

				return color;
			}
			ENDCG
		}
	}

	CustomEditor "Applibot.VFX.ParticleDistortionGrabPassGUI"
}
