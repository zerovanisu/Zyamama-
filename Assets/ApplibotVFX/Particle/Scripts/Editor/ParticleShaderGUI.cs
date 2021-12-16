//------------------------------------------------
// <copyright file="ParticleShaderGUI.cs">
// Copyright (c) Applibot, Inc. All right reserved
// </copyright>
// <summary>ParticleShaderGUI</summary>
//------------------------------------------------
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Applibot.VFX
{
	/// <summary>
	/// ParticleShaderGUI
	/// </summary>
	public abstract class ParticleShaderGUI : ShaderGUI
	{

		#region define

		/// <summary>
		/// ParticleSystemのCustomDataのindex定義
		/// </summary>
		public enum CustomCoord
		{
			Unused				= 2,
			CustomCoord1		= 0,
			CustomCoord2		= 1,
		}

		/// <summary>
		/// ParticleSystemのCustomDataのswizzle定義
		/// </summary>
		public enum ShaderVector
		{
			X					= 0,
			Y					= 1,
			Z					= 2,
			W					= 3,
		}

		/// <summary>
		/// ブレンドモードの定義
		/// </summary>
		public enum ShaderBlendMode
		{
			// ブレンドしない（上書き）
			None				= 0,
			// 加算合成
			Add					= 1,
			// アルファ値を乗算してから加算合成
			TransparentAdd		= 2,
			// アルファブレンド
			AlphaBlend			= 3,
			// 乗算合成
			Multiply			= 4,
			// アルファ値を乗算してから乗算合成
			TransparentMultiply	= 5,
		}

		/// <summary>
		/// ParticleSystemのCustomDataの定義
		/// </summary>
		public enum ShaderCustomCoordVector
		{
			Unused				= 0,
			Coord1X				= 1,
			Coord1Y				= 2,
			Coord1Z				= 3,
			Coord1W				= 4,
			Coord2X				= 5,
			Coord2Y				= 6,
			Coord2Z				= 7,
			Coord2W				= 8,
		}

		#endregion

		#region variable

		// 初期化済みか
		private bool _didInitialize = false;

		#endregion

		#region method

		/// <summary>
		/// GUIを描画する
		/// </summary>
		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			if (!_didInitialize) {
				// キーワードを更新する
				// 本来はMaterialが生成された時にだけ行いたい処理だが
				// そのコールバックが取れないのでここで行っている
				materialEditor.RegisterPropertyChangeUndo("Refresh Material Keyword");
				foreach (Material material in materialEditor.targets) {
					material.shaderKeywords		= GetKeywordsInternal(material);
					material.renderQueue		= (int)GetRenderQueueInternal(material);
				}
				_didInitialize = true;
			}

			using (var ccs = new EditorGUI.ChangeCheckScope()) {
				// GUI描画
				DrawGUI(materialEditor, properties);
				if (ccs.changed) {
					// Keywordを更新
					materialEditor.RegisterPropertyChangeUndo("Refresh Material Keyword");
					foreach (Material material in materialEditor.targets) {
						material.shaderKeywords		= GetKeywordsInternal(material);
						material.renderQueue		= (int)GetRenderQueueInternal(material);
					}
				}
			}

			// ドキュメントへのリンク
			using (new EditorGUILayout.HorizontalScope("box")) {
				var helpIcon				= EditorGUIUtility.IconContent("_Help");
				GUILayout.Label(helpIcon, GUILayout.Width(helpIcon.image.width + 3));
				EditorGUILayout.LabelField("Documentation");
				if (GUILayout.Button("Open", GUILayout.Width(100))) {
					Application.OpenURL("https://docs.google.com/spreadsheets/d/1v4T9G9VKspXr3EN2Zf047RsnJpXBsr6W3WReeBxqTM0/edit#gid=2046931065");
				}
			}
		}

		/// <summary>
		/// GUIを描画する
		/// </summary>
		protected abstract void DrawGUI(MaterialEditor materialEditor, MaterialProperty[] properties);

		/// <summary>
		/// 今の設定に応じたキーワードのリストを取得する
		/// </summary>
		protected abstract string[] GetKeywordsInternal(Material material);

		/// <summary>
		/// 今の設定に応じたRenderQueueを取得する
		/// </summary>
		protected virtual RenderQueue GetRenderQueueInternal(Material material)
		{
			if (material.HasProperty("_UseDitherTransparent")) {
				var useDitherTransparent				= material.GetInt("_UseDitherTransparent") == 1;
				return useDitherTransparent ? RenderQueue.AlphaTest : RenderQueue.Transparent;
			}
			else {
				return RenderQueue.Transparent;
			}
		}

		/// <summary>
		/// ベースカラー領域を描画する
		/// </summary>
		protected void DrawBaseColorBlock(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			var mainTexProp							= FindProperty("_MainTex", properties);
			var mainUCoordProp						= FindProperty("_MainUCoord", properties);
			var mainUSwizzleProp					= FindProperty("_MainUSwizzle", properties);
			var mainVCoordProp						= FindProperty("_MainVCoord", properties);
			var mainVSwizzleProp					= FindProperty("_MainVSwizzle", properties);
			var tintProp							= FindProperty("_Color", properties);

			DrawTextureCustomCoordProperty(materialEditor, mainTexProp, mainUCoordProp, mainUSwizzleProp, mainVCoordProp, mainVSwizzleProp);
			EditorGUILayout.Space();
			DrawDefaultProperty(materialEditor, tintProp);
			EditorGUILayout.Space();
		}

		/// <summary>
		/// ブレンド設定領域を描画する
		/// </summary>
		protected void DrawBlendBlock(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			var colorMagnificationProp				= FindProperty("_ColorMagnification", properties);
			var colorMagnificationCoordProp			= FindProperty("_ColorMagnificationCoord", properties);
			var colorMagnificationSwizzleProp		= FindProperty("_ColorMagnificationSwizzle", properties);
			var blendModeProp						= FindProperty("_BlendMode", properties);
			var blendSrc							= FindProperty("_BlendSrc", properties);
			var blendDest							= FindProperty("_BlendDst", properties);
			var useDitherTransparent				= FindProperty("_UseDitherTransparent", properties);
			var zWriteProp							= FindProperty("_UseZWrite", properties);

			using (new EditorGUILayout.VerticalScope("box")) {
				GUI.enabled = useDitherTransparent.floatValue != 1;
				DrawBlendMode(blendModeProp, blendSrc, blendDest, properties);
				GUI.enabled = true;
				using (new EditorGUI.IndentLevelScope()) {
					using (var ccs = new EditorGUI.ChangeCheckScope()) {
						DrawDefaultProperty(materialEditor, useDitherTransparent);
						if (ccs.changed) {
							// ディザ半透明が有効だったらブレンドモードを強制的にNoneにする
							if (blendModeProp.floatValue != (int)ShaderBlendMode.None) {
								blendModeProp.floatValue			= (int)ShaderBlendMode.None;
								SetBlendMode(ShaderBlendMode.None, blendSrc, blendDest);
							}
							// ZWriteを切り替える
							zWriteProp.floatValue		= useDitherTransparent.floatValue == 1 ? 1 : 0;
						}
					}
				}
			}
			using (new EditorGUILayout.VerticalScope("box")) {
				DrawFloatCustomCoordProperty(materialEditor, colorMagnificationProp, colorMagnificationCoordProp, colorMagnificationSwizzleProp, "Color Magnification", "CustomCoord (Add)");

				if (colorMagnificationProp.floatValue != 1) {
					EditorGUILayout.HelpBox("ここまでの色に倍率を掛けます。", MessageType.Info);
				}
			}
		}

		/// <summary>
		/// ブレンドモードを描画する
		/// </summary>
		protected void DrawBlendMode(MaterialProperty blendModeProp, MaterialProperty blendSrcProp, MaterialProperty blendDstProp, MaterialProperty[] properties)
		{
			var blendMode = (ShaderBlendMode)blendModeProp.floatValue;
			using (var s = new EditorGUI.ChangeCheckScope()) {
				blendMode = (ShaderBlendMode)EditorGUILayout.EnumPopup(blendModeProp.displayName, blendMode);
				if (s.changed) {
					blendModeProp.floatValue	= (int)blendMode;
					SetBlendMode(blendMode, blendSrcProp, blendDstProp);
				}
			}
		}

		/// <summary>
		/// ShaderBlendModeからマテリアルのBlendModeを設定する
		/// </summary>
		private void SetBlendMode(ShaderBlendMode shaderBlendMode, MaterialProperty blendSrcProp, MaterialProperty blendDstProp)
		{
			switch (shaderBlendMode) {
			case ShaderBlendMode.None:
				blendSrcProp.floatValue = (float)BlendMode.One;
				blendDstProp.floatValue = (float)BlendMode.Zero;
				break;
			case ShaderBlendMode.Add:
				blendSrcProp.floatValue = (float)BlendMode.One;
				blendDstProp.floatValue = (float)BlendMode.One;
				break;
			case ShaderBlendMode.TransparentAdd:
				blendSrcProp.floatValue = (float)BlendMode.SrcAlpha;
				blendDstProp.floatValue = (float)BlendMode.One;
				break;
			case ShaderBlendMode.AlphaBlend:
				blendSrcProp.floatValue = (float)BlendMode.SrcAlpha;
				blendDstProp.floatValue = (float)BlendMode.OneMinusSrcAlpha;
				break;
			case ShaderBlendMode.Multiply:
				blendSrcProp.floatValue = (float)BlendMode.DstColor;
				blendDstProp.floatValue = (float)BlendMode.Zero;
				break;
			case ShaderBlendMode.TransparentMultiply:
				blendSrcProp.floatValue = (float)BlendMode.DstColor;
				blendDstProp.floatValue = (float)BlendMode.Zero;
				break;
			default:
				break;
			}
		}

		/// <summary>
		/// その他の設定領域を描画する
		/// </summary>
		protected void DrawOtherBlock(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			var cullModeProp			= FindProperty("_CullMode", properties);
			var zTestProp				= FindProperty("_ZTest", properties);
			var zOffsetProp				= FindProperty("_ZOffset", properties);
			var hdrThresholdProp		= FindProperty("_HDRThresholdParticle", properties);
			var blendModeProp			= FindProperty("_BlendMode", properties);

			// clling
			EditorGUILayout.BeginVertical("box");
			DrawDefaultProperty(materialEditor, cullModeProp);
			EditorGUILayout.EndVertical();

			// z control
			EditorGUILayout.BeginVertical("box");
			DrawDefaultProperty(materialEditor, zOffsetProp);
			DrawDefaultProperty(materialEditor, zTestProp);
			EditorGUILayout.EndVertical();

			// hdr
			using (new EditorGUILayout.VerticalScope("box")) {
				DrawDefaultProperty(materialEditor, hdrThresholdProp);

				var blendMode	= (ShaderBlendMode)blendModeProp.floatValue;
				if (blendMode == ShaderBlendMode.Multiply || blendMode == ShaderBlendMode.TransparentMultiply) {
					EditorGUILayout.HelpBox("ブレンドモードが乗算のときはHDRの設定値は適用されません", MessageType.Error);
				}
			}
		}

		/// <summary>
		/// デフォルトのプロパティ描画をする
		/// </summary>
		protected void DrawDefaultProperty(MaterialEditor editor, MaterialProperty property, string overrideName = null)
		{
			editor.ShaderProperty(property, overrideName ?? property.displayName);
		}

		/// <summary>
		/// 左側にチェックボックスがついたトグルを描画する
		/// </summary>
		protected void DrawLeftToggleProperty(MaterialEditor editor, MaterialProperty property)
		{
			EditorGUI.showMixedValue = property.hasMixedValue;

			using (var ccs = new EditorGUI.ChangeCheckScope()) {
				var boolValue = property.floatValue == 0 ? false : true;
				var value = EditorGUILayout.ToggleLeft(property.displayName, boolValue) ? 1.0f : 0.0f;
				if (ccs.changed) {
					editor.RegisterPropertyChangeUndo(property.name);
					property.floatValue = value;
				}
			}

			EditorGUI.showMixedValue = false;
		}

		/// <summary>
		/// CustomCoord設定値付きのfloatプロパティを描画する
		/// </summary>
		protected void DrawFloatCustomCoordProperty(MaterialEditor editor, MaterialProperty property, MaterialProperty coordProperty, MaterialProperty swizzleProperty, string overrideName = null, string overrideCustomCoordName = null)
		{

			var coord = (CustomCoord)coordProperty.floatValue;
			var swizzle = (ShaderVector)swizzleProperty.floatValue;

			DrawDefaultProperty(editor, property, overrideName);

			EditorGUI.showMixedValue = coordProperty.hasMixedValue;
			using (new EditorGUI.IndentLevelScope()) {
				using (new EditorGUILayout.HorizontalScope()) {
					EditorGUILayout.LabelField(overrideCustomCoordName ?? "CustomCoord", GUILayout.MaxWidth(150));

					using (var ccs = new EditorGUI.ChangeCheckScope()) {
						var customCoordVector = (ShaderCustomCoordVector) EditorGUILayout.EnumPopup(GetCustomCoordVector(coord, swizzle));
						if (ccs.changed) {
							editor.RegisterPropertyChangeUndo(property.name);
							SetCustomCoordVector(customCoordVector, coordProperty, swizzleProperty);
						}
					}
				}
			}
			EditorGUI.showMixedValue = false;
		}

		/// <summary>
		/// CustomCoord設定値付きのuvプロパティを描画する
		/// </summary>
		protected void DrawTextureCustomCoordProperty(MaterialEditor editor, MaterialProperty property, MaterialProperty coordUProperty, MaterialProperty swizzleUProperty, MaterialProperty coordVProperty, MaterialProperty swizzleVProperty, string overrideName = null, string overrideCustomCoordUName = null, string overrideCustomCoordVName = null)
		{

			var coordU = (CustomCoord)coordUProperty.floatValue;
			var swizzleU = (ShaderVector)swizzleUProperty.floatValue;
			var coordV = (CustomCoord)coordVProperty.floatValue;
			var swizzleV = (ShaderVector)swizzleVProperty.floatValue;

			DrawDefaultProperty(editor, property, overrideName);

			using (new EditorGUI.IndentLevelScope()) {
				using (new EditorGUILayout.HorizontalScope()) {
					EditorGUILayout.LabelField(overrideCustomCoordUName ?? "CustomCoordU", GUILayout.MaxWidth(150));

					EditorGUI.showMixedValue = coordUProperty.hasMixedValue;
					using (var ccs = new EditorGUI.ChangeCheckScope()) {
						var customCoordUVector = (ShaderCustomCoordVector) EditorGUILayout.EnumPopup(GetCustomCoordVector(coordU, swizzleU));
						if (ccs.changed) {
							SetCustomCoordVector(customCoordUVector, coordUProperty, swizzleUProperty);
						}
					}
					EditorGUI.showMixedValue = false;
				}
				using (new EditorGUILayout.HorizontalScope()) {
					EditorGUILayout.LabelField( overrideCustomCoordVName ?? "CustomCoordV", GUILayout.MaxWidth(150));

					EditorGUI.showMixedValue = coordVProperty.hasMixedValue;
					using (var ccs = new EditorGUI.ChangeCheckScope()) {
						var customCoordVVector = (ShaderCustomCoordVector) EditorGUILayout.EnumPopup(GetCustomCoordVector(coordV, swizzleV));
						if (ccs.changed) {
							SetCustomCoordVector(customCoordVVector, coordVProperty, swizzleVProperty);
						}
					}
					EditorGUI.showMixedValue = false;
				}
			}
		}

		/// <summary>
		/// カスタムデータのIdnexとSwizzleからShaderCustomCoordVectorを取得する
		/// </summary>
		protected ShaderCustomCoordVector GetCustomCoordVector(CustomCoord coord, ShaderVector vector)
		{
			if (coord == CustomCoord.CustomCoord1) {
				switch (vector) {
				case ShaderVector.X:
					return ShaderCustomCoordVector.Coord1X;
				case ShaderVector.Y:
					return ShaderCustomCoordVector.Coord1Y;
				case ShaderVector.Z:
					return ShaderCustomCoordVector.Coord1Z;
				case ShaderVector.W:
					return ShaderCustomCoordVector.Coord1W;
				default:
					break;
				}
			}
			else if (coord == CustomCoord.CustomCoord2) {
				switch (vector) {
				case ShaderVector.X:
					return ShaderCustomCoordVector.Coord2X;
				case ShaderVector.Y:
					return ShaderCustomCoordVector.Coord2Y;
				case ShaderVector.Z:
					return ShaderCustomCoordVector.Coord2Z;
				case ShaderVector.W:
					return ShaderCustomCoordVector.Coord2W;
				default:
					break;
				}
			}
			return ShaderCustomCoordVector.Unused;
		}

		/// <summary>
		/// ShaderCustomCoordVectorをカスタムデータのIndexとSwizzleのインデックスに変換して代入する
		/// </summary>
		protected void SetCustomCoordVector(ShaderCustomCoordVector coordVector, MaterialProperty coordProperty, MaterialProperty swizzleProperty)
		{
			switch (coordVector) {
			case ShaderCustomCoordVector.Unused:
				coordProperty.floatValue = (float)CustomCoord.Unused;
				break;
			case ShaderCustomCoordVector.Coord1X:
				coordProperty.floatValue = (float)CustomCoord.CustomCoord1;
				swizzleProperty.floatValue = (float)ShaderVector.X;
				break;
			case ShaderCustomCoordVector.Coord1Y:
				coordProperty.floatValue = (float)CustomCoord.CustomCoord1;
				swizzleProperty.floatValue = (float)ShaderVector.Y;
				break;
			case ShaderCustomCoordVector.Coord1Z:
				coordProperty.floatValue = (float)CustomCoord.CustomCoord1;
				swizzleProperty.floatValue = (float)ShaderVector.Z;
				break;
			case ShaderCustomCoordVector.Coord1W:
				coordProperty.floatValue = (float)CustomCoord.CustomCoord1;
				swizzleProperty.floatValue = (float)ShaderVector.W;
				break;
			case ShaderCustomCoordVector.Coord2X:
				coordProperty.floatValue = (float)CustomCoord.CustomCoord2;
				swizzleProperty.floatValue = (float)ShaderVector.X;
				break;
			case ShaderCustomCoordVector.Coord2Y:
				coordProperty.floatValue = (float)CustomCoord.CustomCoord2;
				swizzleProperty.floatValue = (float)ShaderVector.Y;
				break;
			case ShaderCustomCoordVector.Coord2Z:
				coordProperty.floatValue = (float)CustomCoord.CustomCoord2;
				swizzleProperty.floatValue = (float)ShaderVector.Z;
				break;
			case ShaderCustomCoordVector.Coord2W:
				coordProperty.floatValue = (float)CustomCoord.CustomCoord2;
				swizzleProperty.floatValue = (float)ShaderVector.W;
				break;
			default:
				break;
			}
		}

		#endregion
	}
}
