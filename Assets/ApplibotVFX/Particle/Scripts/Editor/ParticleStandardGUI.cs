//------------------------------------------------
// <copyright file="ParticleStandardGUI.cs">
// Copyright (c) Applibot, Inc. All right reserved
// </copyright>
// <summary>ParticleStandardGUI</summary>
//------------------------------------------------
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Applibot.VFX
{
	/// <summary>
	/// ParticleStandardGUI
	/// </summary>
	public class ParticleStandardGUI : ParticleShaderGUI
	{

		#region variable

		/// <summary>
		/// その他メニューの畳みこみ状態
		/// </summary>
		private bool _otherSettingFolding = false;

		#endregion

		#region method

		/// <summary>
		/// 適用するシェーダキーワードを取得する
		/// スクリプトでマテリアルを制御するとき用にpublic staticとして定義
		/// </summary>
		public static string[] GetKeywords(Material material)
		{
			var keywords			= new List<string>();
			var blendMode			= (ShaderBlendMode)material.GetFloat("_BlendMode");
			switch (blendMode) {
			case ShaderBlendMode.Add:
				keywords.Add("BLENDMODE_ADD");
				break;
			case ShaderBlendMode.TransparentAdd:
				keywords.Add("BLENDMODE_TRANSPARENTADD");
				break;
			case ShaderBlendMode.AlphaBlend:
				keywords.Add("BLENDMODE_ALPHABLEND");
				break;
			case ShaderBlendMode.Multiply:
				keywords.Add("BLENDMODE_MULTIPLY");
				break;
			case ShaderBlendMode.TransparentMultiply:
				keywords.Add("BLENDMODE_TRANSPARENTMULTIPLY");
				break;
			default:
				break;
			}

			var useDist					= material.GetFloat("_UseDist");
			var useMask					= material.GetFloat("_UseMask");
			var useReflection			= material.GetFloat("_UseReflection");
			var useBlendRateFromRGB		= material.GetFloat("_UseBlendRateFromRGB");
			var useBlendRateFromRim		= material.GetFloat("_UseBlendRateFromRim");
			var useBlendRateFromHeight	= material.GetFloat("_UseBlendRateFromHeight");
			var useBlendRateFromZ		= material.GetFloat("_UseBlendRateFromZ");
			var useDitherTransparent	= material.GetInt("_UseDitherTransparent");
			if (useDist == 1) {
				keywords.Add("USE_DISTORTION");
			}
			if (useReflection == 1) {
				keywords.Add("USE_REFLECTION");
			}
			if (useMask == 1) {
				keywords.Add("USE_MASK");
			}
			if (useBlendRateFromRGB == 1) {
				keywords.Add("BLEND_RATE_FROM_RGB");
			}
			if (useBlendRateFromRim == 1) {
				keywords.Add("BLEND_RATE_FROM_RIM");
			}
			if (useBlendRateFromHeight == 1) {
				keywords.Add("BLEND_RATE_FROM_HEIGHT");
			}
			if (useBlendRateFromZ == 1)
			{
				keywords.Add("BLEND_RATE_FROM_Z");
			}
			if (useDitherTransparent == 1) {
				keywords.Add("_DITHER_TRANSPARENT");
			}

			return keywords.ToArray();
		}

		/// <summary>
		/// 描画処理
		/// </summary>
		protected override void DrawGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			// メインテクスチャ領域を描画
			DrawBaseColorBlock(materialEditor, properties);

			// ディストーション領域を描画
			using (new EditorGUILayout.VerticalScope("box")) {
				var useDistProp					= FindProperty("_UseDist", properties);
				var distTexProp					= FindProperty("_DistTex", properties);
				var distPowProp					= FindProperty("_DistPow", properties);
				var distUCoordProp				= FindProperty("_DistUCoord", properties);
				var distUSwizzleProp			= FindProperty("_DistUSwizzle", properties);
				var distVCoordProp				= FindProperty("_DistVCoord", properties);
				var distVSwizzleProp			= FindProperty("_DistVSwizzle", properties);
				var distPowCoordProp			= FindProperty("_DistPowCoord", properties);
				var distPowSwizzleProp			= FindProperty("_DistPowSwizzle", properties);
				DrawLeftToggleProperty(materialEditor, useDistProp);
				if (useDistProp.floatValue == 1) {
					using (new EditorGUILayout.VerticalScope("box")) {
						DrawTextureCustomCoordProperty(materialEditor, distTexProp, distUCoordProp, distUSwizzleProp, distVCoordProp, distVSwizzleProp);
					}
					using (new EditorGUILayout.VerticalScope("box")) {
						DrawFloatCustomCoordProperty(materialEditor, distPowProp, distPowCoordProp, distPowSwizzleProp, "Power", "CustomCoord (Add)");
					}
					EditorGUILayout.HelpBox("Distortion を使用するためには ParticleSystem > Renderer > Custom Vertex Streams にUV2 (TEXCOORD0.zw)を設定する必要があります。", MessageType.Info);
				}
			}

			// マスク領域を描画
			using (new EditorGUILayout.VerticalScope("box")) {
				var useMaskProp					= FindProperty("_UseMask", properties);
				var maskTexProp					= FindProperty("_MaskTex", properties);
				var maskPowProp					= FindProperty("_MaskPow", properties);
				var maskUCoordProp				= FindProperty("_MaskUCoord", properties);
				var maskUSwizzleProp			= FindProperty("_MaskUSwizzle", properties);
				var maskVCoordProp				= FindProperty("_MaskVCoord", properties);
				var maskVSwizzleProp			= FindProperty("_MaskVSwizzle", properties);
				var maskPowCoordProp			= FindProperty("_MaskPowCoord", properties);
				var maskPowSwizzleProp			= FindProperty("_MaskPowSwizzle", properties);
				DrawLeftToggleProperty(materialEditor, useMaskProp);
				if (useMaskProp.floatValue == 1) {
					using (new EditorGUILayout.VerticalScope("box")) {
						DrawTextureCustomCoordProperty(materialEditor, maskTexProp, maskUCoordProp, maskUSwizzleProp, maskVCoordProp, maskVSwizzleProp);
					}
					using (new EditorGUILayout.VerticalScope("box")) {
						DrawFloatCustomCoordProperty(materialEditor, maskPowProp, maskPowCoordProp, maskPowSwizzleProp, "Power", "CustomCoord (Add)");
					}
					EditorGUILayout.HelpBox("Mask を使用するためには ParticleSystem > Renderer > Custom Vertex Streams にUV2 (TEXCOORD0.zw)を設定する必要があります。", MessageType.Info);
				}
			}

			// ブレンド率の領域を描画
			using (new EditorGUILayout.VerticalScope("box")) {
				var useBlendRateFromRGBProp					= FindProperty("_UseBlendRateFromRGB", properties);
				var useBlendRateFromRimProp					= FindProperty("_UseBlendRateFromRim", properties);
				var blendRateRimSharpnessProp				= FindProperty("_BlendRateRimSharpness", properties);
				var blendRateRimSharpnessCoordProp			= FindProperty("_BlendRateRimSharpnessCoord", properties);
				var blendRateRimSharpnessSwizzleProp		= FindProperty("_BlendRateRimSharpnessSwizzle", properties);
				var blendRateRimInverseProp					= FindProperty("_BlendRateRimInverse", properties);
				var useBlendRateFromHeightProp				= FindProperty("_UseBlendRateFromHeight", properties);
				var blendRateHeightStartOffsetProp			= FindProperty("_BlendRateHeightStartOffset", properties);
				var blendRateHeightDistanceProp				= FindProperty("_BlendRateHeightDistance", properties);
				var useBlendRateFromZProp					= FindProperty("_UseBlendRateFromZ", properties);
				var blendRateFromZDistanceProp				= FindProperty("_BlendRateFromZDistance", properties);
				EditorGUILayout.LabelField("Blend Rate");
				using (new EditorGUILayout.VerticalScope("box")) {
					DrawDefaultProperty(materialEditor, useBlendRateFromRGBProp, "From RGB");
					if (useBlendRateFromRGBProp.floatValue == 1) {
						EditorGUILayout.HelpBox("ここまでの色の輝度をブレンド率として使用します。\nメインテクスチャの色は反映されません。", MessageType.Info);
					}
				}
				using (new EditorGUILayout.VerticalScope("box")) {
					DrawDefaultProperty(materialEditor, useBlendRateFromRimProp, "From Rim");
					if (useBlendRateFromRimProp.floatValue == 1) {
						DrawFloatCustomCoordProperty(materialEditor, blendRateRimSharpnessProp, blendRateRimSharpnessCoordProp, blendRateRimSharpnessSwizzleProp, "Sharpness", "CustomCoord (Add)");
						DrawDefaultProperty(materialEditor, blendRateRimInverseProp, "Inverse");
					}
				}
				using (new EditorGUILayout.VerticalScope("box")) {
					DrawDefaultProperty(materialEditor, useBlendRateFromHeightProp, "From Height (Soft Particle)");
					if (useBlendRateFromHeightProp.floatValue == 1) {
						DrawDefaultProperty(materialEditor, blendRateHeightStartOffsetProp, "Start Y");
						DrawDefaultProperty(materialEditor, blendRateHeightDistanceProp, "Distance");
					}
				}
				using (new EditorGUILayout.VerticalScope("box"))
				{
					DrawDefaultProperty(materialEditor, useBlendRateFromZProp, "From Z (Soft Particle)");
					if (useBlendRateFromZProp.floatValue == 1)
					{
						DrawDefaultProperty(materialEditor, blendRateFromZDistanceProp, "Distance");
					}
				}
			}

			// リフレクション指定領域を描画
			using (new EditorGUILayout.VerticalScope("box")) {
				var useReflectionProp					= FindProperty("_UseReflection", properties);
				var reflectionPowerProp					= FindProperty("_ReflectionPower", properties);
				var reflectionRoughnessProp				= FindProperty("_ReflectionRoughness", properties);
				DrawLeftToggleProperty(materialEditor, useReflectionProp);
				if (useReflectionProp.floatValue == 1) {
					DrawDefaultProperty(materialEditor, reflectionPowerProp);
					DrawDefaultProperty(materialEditor, reflectionRoughnessProp);
				}
			}

			// ブレンド設定の領域を描画
			using (new EditorGUILayout.VerticalScope("box")) {
				DrawBlendBlock(materialEditor, properties);
			}

			// その他のオプション領域を描画
			using (new EditorGUILayout.VerticalScope("box")) {
				using (new EditorGUI.IndentLevelScope()) {
					_otherSettingFolding = EditorGUILayout.Foldout(_otherSettingFolding, "OtherSettings");
				}
				if (_otherSettingFolding) {
					DrawOtherBlock(materialEditor, properties);
				}
			}

		}

		/// <summary>
		/// 適用するシェーダキーワードを取得する
		/// </summary>
		protected override string[] GetKeywordsInternal(Material material)
		{
			return GetKeywords(material);
		}

		#endregion

	}
}
