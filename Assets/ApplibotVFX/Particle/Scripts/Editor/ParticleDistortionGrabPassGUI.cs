//------------------------------------------------
// <copyright file="ParticleDistortionGrabPassGUI.cs">
// Copyright (c) Applibot, Inc. All right reserved
// </copyright>
// <summary>ParticleDistortionGrabPassGUI</summary>
//------------------------------------------------
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Applibot.VFX
{
	/// <summary>
	/// ParticleDistortionGrabPassGUI
	/// </summary>
	public class ParticleDistortionGrabPassGUI : ParticleShaderGUI
	{

		#region method

		/// <summary>
		/// 適用するシェーダキーワードを取得する
		/// スクリプトでマテリアルを制御するとき用にpublic staticとして定義
		/// </summary>
		public static string[] GetKeywords(Material material)
		{
			var keywords			= new List<string>();

			var useMaskProp			= material.GetFloat("_UseMask");
			if (useMaskProp == 1) {
				keywords.Add("USE_MASK");
			}

			return keywords.ToArray();
		}

		/// <summary>
		/// 描画処理
		/// </summary>
		protected override void DrawGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			// 基本設定領域を描画
			var normalTexProp				= FindProperty("_NormalTex", properties);
			var distPowProp					= FindProperty("_DistPow", properties);
			var distUCoordProp				= FindProperty("_DistUCoord", properties);
			var distUSwizzleProp			= FindProperty("_DistUSwizzle", properties);
			var distVCoordProp				= FindProperty("_DistVCoord", properties);
			var distVSwizzleProp			= FindProperty("_DistVSwizzle", properties);
			DrawTextureCustomCoordProperty(materialEditor, normalTexProp, distUCoordProp, distUSwizzleProp, distVCoordProp, distVSwizzleProp);

			// ディストーションの強さ領域を描画
			EditorGUILayout.Space();
			var distPowCoordProp			= FindProperty("_DistPowCoord", properties);
			var distPowSwizzleProp			= FindProperty("_DistPowSwizzle", properties);
			DrawFloatCustomCoordProperty(materialEditor, distPowProp, distPowCoordProp, distPowSwizzleProp);

			// マスク領域を描画
			using (new EditorGUILayout.VerticalScope()) {
				var useMaskProp				= FindProperty("_UseMask", properties);
				var maskTexProp				= FindProperty("_MaskTex", properties);
				DrawLeftToggleProperty(materialEditor, useMaskProp);
				if (useMaskProp.floatValue == 1) {
					DrawDefaultProperty(materialEditor, maskTexProp);
					EditorGUILayout.HelpBox("Mask を使用するためには ParticleSystem > Renderer > Custom Vertex Streams にUV2 (TEXCOORD0.zw)を設定する必要があります。", MessageType.Info);
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
