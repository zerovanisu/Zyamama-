//------------------------------------------------
// <copyright file="ParticleShaderToolPanel.cs">
// Copyright (c) Applibot, Inc. All right reserved
// </copyright>
// <summary>ParticleShaderToolPanel</summary>
//------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Applibot.VFX
{
	/// <summary>
	/// ParticleShaderToolPanel
	/// </summary>
	public class ParticleShaderToolPanel : EditorWindow
	{

		#region define

		private const string SHADER_NAME_STANDARD						= "Applibot/Particles/Standard";
		private const string SHADER_NAME_DISSOLVE						= "Applibot/Particles/Dissolve";
		private const string SHADER_NAME_DISTORTIONGRABPASS				= "Applibot/Particles/Distortion (GrabPass)";

		#endregion

		#region method

		[MenuItem("Window/Applibot/Particle Tool Panel")]
		private static void Open()
		{
			GetWindow<ParticleShaderToolPanel>(ObjectNames.NicifyVariableName(typeof(ParticleShaderToolPanel).Name));
		}

		/// <summary>
		/// GUIを描画する
		/// </summary>
		private void OnGUI()
		{
			using (new EditorGUILayout.HorizontalScope()) {
				if (GUILayout.Button("Replace Obsolete Shaders") && EditorUtility.DisplayDialog("確認", "古いシェーダを置換します。\nこの処理には時間が掛かる場合があります。", "OK", "CANCEL")) {
					ReplaceObsoleteShaders();
					RefreshKeywords();
				}
				if (GUILayout.Button("Refresh Keywords") && EditorUtility.DisplayDialog("確認", "全てのParticle用マテリアルのキーワードを更新します。\nこの処理には時間が掛かる場合があります。", "OK", "CANCEL")) {
					RefreshKeywords();
				}
			}
		}

		/// <summary>
		/// Obsoleteなシェーダを置き換えるボタンを描画する
		/// </summary>
		private void ReplaceObsoleteShaders()
		{
			try {
				var materials			= AssetDatabase
					.FindAssets("t: Material")
					.Select(guid => AssetDatabase.GUIDToAssetPath(guid))
					.Select(assetPath => AssetDatabase.LoadAssetAtPath<Material>(assetPath))
					.Where(material => material.shader.name.Contains("Particles"))
					.ToArray();

				for (int i = 0; i < materials.Length; i++) {
					var material = materials[i];

					EditorUtility.DisplayProgressBar("進捗", "マテリアルのシェーダを置換中... ", (float)i / materials.Length);

					if (material.shader.name.Contains(SHADER_NAME_STANDARD.Replace("Applibot", "")) && material.shader.name != SHADER_NAME_STANDARD) {
						material.shader			= Shader.Find(SHADER_NAME_STANDARD);
					}
					if (material.shader.name.Contains(SHADER_NAME_DISSOLVE.Replace("Applibot", "")) && material.shader.name != SHADER_NAME_DISSOLVE) {
						material.shader			= Shader.Find(SHADER_NAME_DISSOLVE);
					}
					if (material.shader.name.Contains(SHADER_NAME_DISTORTIONGRABPASS.Replace("Applibot", "")) && material.shader.name != SHADER_NAME_DISTORTIONGRABPASS) {
						material.shader			= Shader.Find(SHADER_NAME_DISTORTIONGRABPASS);
					}
				}

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
			finally {
				EditorUtility.ClearProgressBar();
			}
		}

		/// <summary>
		/// 全てのマテリアルのキーワードを更新するボタンを描画する
		/// </summary>
		private void RefreshKeywords()
		{
			try {
				var shaderNames			= new List<string>{ SHADER_NAME_STANDARD, SHADER_NAME_DISSOLVE, SHADER_NAME_DISTORTIONGRABPASS };
				var materials			= AssetDatabase
					.FindAssets("t: Material")
					.Select(guid => AssetDatabase.GUIDToAssetPath(guid))
					.Select(assetPath => AssetDatabase.LoadAssetAtPath<Material>(assetPath))
					.Where(material => shaderNames.Contains(material.shader.name))
					.ToArray();

				for (int i = 0; i < materials.Length; i++) {
					var material = materials[i];

					EditorUtility.DisplayProgressBar("進捗", "マテリアルのキーワードを更新中... ", (float)i / materials.Length);

					if (material.shader.name == SHADER_NAME_STANDARD) {
						material.shaderKeywords		= ParticleStandardGUI.GetKeywords(material);
					}
					if (material.shader.name == SHADER_NAME_DISSOLVE) {
						material.shaderKeywords		= ParticleDissolveGUI.GetKeywords(material);
					}
					if (material.shader.name == SHADER_NAME_DISTORTIONGRABPASS) {
						material.shaderKeywords		= ParticleDistortionGrabPassGUI.GetKeywords(material);
					}
				}

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
			finally {
				EditorUtility.ClearProgressBar();
			}
		}

		#endregion

	}
}
