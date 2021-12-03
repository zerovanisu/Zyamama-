#ifndef APPLIBOT_PARTICLE_INCLUDED
#define APPLIBOT_PARTICLE_INCLUDED

// プロジェクトごとの設定値
//#define USE_HDR_THRESHOLD // 疑似HDRを使う場合は有効にする


// ブレンドモードに応じてブレンド開始用の色を定義
#if defined(BLENDMODE_ADD) || defined(BLENDMODE_TRANSPARENTADD)
	#define BLEND_START_COLOR half4(0, 0, 0, 1)
#elif defined(BLENDMODE_ALPHABLEND)
	#define BLEND_START_COLOR half4(0, 0, 0, 0)
#elif defined(BLENDMODE_MULTIPLY) || defined(BLENDMODE_TRANSPARENTMULTIPLY)
	#define BLEND_START_COLOR half4(1, 1, 1, 1)
#else
	#define BLEND_START_COLOR half4(0, 0, 0, 1)
#endif

// Particle SystemのCustom Data用
#define CUSTOM_DATA_DEFINE(name) uniform int name##Coord; \
	uniform int name##Swizzle;
#define CUSTOM_DATA_DEFINE_UV(name) CUSTOM_DATA_DEFINE(name##U); \
	CUSTOM_DATA_DEFINE(name##V);
#define CUSTOM_DATA_VERTEX_INPUT(idx1, idx2) half4 texCoord1: TEXCOORD##idx1; \
	half4 texCoord2: TEXCOORD##idx2;
#define CUSTOM_DATA_VERTEX_INITIALIZATION(appdata, v2f) half4x4 texCoords = \
	{ \
		appdata.texCoord1, \
		appdata.texCoord2, \
		half4(0.0, 0.0, 0.0, 0.0), \
		half4(0.0, 0.0, 0.0, 0.0) \
	};
#define CUSTOM_DATA_VERTEX_GET(name) texCoords[name##Coord][name##Swizzle]
#define CUSTOM_DATA_VERTEX_GET_UV(name) half2(CUSTOM_DATA_VERTEX_GET(name##U), CUSTOM_DATA_VERTEX_GET(name##V))

// ブレンド率を適用する
inline half4 ApplyBlendRate(half blendRate, half4 color)
{
	#ifdef BLENDMODE_ALPHABLEND
		color.a			*= blendRate;
	#else
		color.rgb		= lerp(BLEND_START_COLOR.rgb, color.rgb, blendRate);
	#endif

	return color;
}

// GPUによるブレンド前処理
inline half4 PreBlend(half4 color)
{
	// アルファ値事前乗算の乗算ブレンドの場合は事前にアルファ値を乗算しておく
	#ifdef BLENDMODE_TRANSPARENTMULTIPLY
		color.rgb			= lerp(1.0, color.rgb, color.a);
	#endif

	return color;
}

// HDR色を取得する
inline half3 GetHdrColor(half3 col, half3 emission, half threshold){

	#if defined(BLENDMODE_MULTIPLY) || defined(BLENDMODE_TRANSPARENTMULTIPLY)
		threshold = 1.0;
	#endif

	col = saturate(col) * threshold;
	col += emission;
	return max(0, col);
}

#endif
