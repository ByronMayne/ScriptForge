﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

namespace ScriptForge
{
	public class SceneWidget : ForgeWidget
	{
		private string enumName = "Scenes"; 
		private GUIContent enumContent = new GUIContent ("Enum Name:", "This is the name of the enum that lists all the scenes");

		/// <summary>
		/// The constructor we use to build our Forge items. 
		/// </summary>
		/// <param name="Name">This is the name that will showup on the foldout at the top.</param>
		/// <param name="Tooltip">This is the message that the user will see when they put their mouse over the foldout.</param>
		/// <param name="Height">How tall will the editor box be when fully opened?</param>
		public SceneWidget(string Name, string Tooltip, float Height) : base(Name, Tooltip, Height)
		{
			_OnGenerateAll += OnGenerate;
			_widgetSkinName = "Scene";
		}

		/// <summary>
		/// This is the deconstructor. It's only used to unsubscribe our OnGenerate method from the static delegate. (It will cause errors if you don't);
		/// </summary>
		~SceneWidget()
		{
			_OnGenerateAll -= OnGenerate;
		}

		public override void GenerateCode()
		{
			List<string> sceneNames = new List<string> ();
	
			//TODO: Make Gen Cod
			foreach( EditorBuildSettingsScene scene in EditorBuildSettings.scenes )
			{	
				string sceneName = scene.path.Substring( scene.path.LastIndexOf('/') + 1);
				sceneName = sceneName.Replace(".unity", "" );

				sceneNames.Add( sceneName );
			}

			// Build the generator with the class name and data source.
			ScenesGenerator generator = new ScenesGenerator(_scriptName, _buildPath, sceneNames.ToArray(), enumName, _namespace);
			
			// Generate output (class definition).
			var classDefintion = generator.TransformText();
			
			var outputPath = Path.Combine(Application.dataPath + _buildPath, _scriptName + ".cs");
			
			try
			{
				// Save new class to assets folder.
				File.WriteAllText(outputPath, classDefintion);
				
				// Refresh assets.
				AssetDatabase.Refresh();
			}
			catch (Exception e)
			{
				Debug.Log("An error occurred while saving file: " + e);
			}
		}

		protected override void LoadPrefValues ()
		{
			base.LoadPrefValues ();
			enumName = EditorPrefs.GetString(this.GetType().ToString() + _ID.ToString() + sf_PrefNames.EP_SCENES_ENUMNAME.Name, sf_PrefNames.EP_SCENES_ENUMNAME.Default);
		}

		protected override void SavePrefValues ()
		{
			base.SavePrefValues ();
			EditorPrefs.SetString(this.GetType().ToString() + _ID.ToString() + sf_PrefNames.EP_SCENES_ENUMNAME.Name, enumName );
		}

		protected override void DrawForgeContent ()
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(enumContent, EditorStyles.boldLabel, GUILayout.Width (CONTENT_TITLE_WIDTH));
			enumName = GUILayout.TextField (enumName);
			GUILayout.EndHorizontal();
		}


	}
}
