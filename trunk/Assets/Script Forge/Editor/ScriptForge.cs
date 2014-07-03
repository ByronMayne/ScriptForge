using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic; 

namespace ScriptForge
{
	/// <summary>
	/// A helper class for code generation in the editor.
	/// </summary>
	public class ScriptForge : EditorWindow 
	{
		private static Vector2 _scrollPostion = Vector2.zero;

		public static ScriptForge Instance { get; protected set; }

		public static List<EditorWidget> _widgets; 

		public static void AddWidget<T_WidgetType>() where T_WidgetType : EditorWidget, new()
		{
			_widgets.Add( new T_WidgetType() );
		}

		public static void RemoveWidget(EditorWidget widget)
		{
			if( _widgets.Contains( widget ) )
			{
				widget.Destroy();

				_widgets.Remove( widget );
			}
		}

		public void OnEnable()
		{
			_widgets = new List<EditorWidget>();

			name = " Script Forge";
			Instance =  this; //EditorWindow.GetWindow<ScriptForge>();

			if( EditorPrefs.GetBool(sf_PrefNames.EP_FIRST_LAUNCH_BOOL.Name, sf_PrefNames.EP_FIRST_LAUNCH_BOOL.Default ) )
			{
				System.Diagnostics.Process.Start(sf_Links.SCRIPT_FORGE_GOOLGE_DOC_URL);
				EditorPrefs.SetBool(sf_PrefNames.EP_FIRST_LAUNCH_BOOL.Name, false );
			}

			AddWidget<LayersWidget>();
			AddWidget<SortingLayersWidget>();
			AddWidget<TagsWidget>();
			AddWidget<SceneWidget>();
			AddWidget<SettingsWidget>();
			AddWidget<AboutWidget>();
		}

		public void OnDisable()
		{
			_widgets = new List<EditorWidget>();
		}

		/// <summary>
		/// When the project has been changed we use 
		/// this event to fire all the auto build actions on our forges. 
		/// </summary>
		private void OnProjectChange()
		{
			if( ForgeWidget._OnAutoBuild != null )
				ForgeWidget._OnAutoBuild();
		}

		private void Update()
		{
			if( EditorWidget._OnUpdate != null )
				EditorWidget._OnUpdate();
		}

        [MenuItem("Window/Script Forge")]
		[MenuItem("CONTEXT/TagManager/Open Script Forge")]
		public static void ShowWindow()
		{
			Instance =  EditorWindow.GetWindow<ScriptForge>();
			Instance.minSize = new Vector2( 300, 80 );
			Instance.Show();


			if( EditorPrefs.GetBool(sf_PrefNames.EP_FIRST_LAUNCH_BOOL.Name, sf_PrefNames.EP_FIRST_LAUNCH_BOOL.Default ) )
			{
				System.Diagnostics.Process.Start(sf_Links.SCRIPT_FORGE_GOOLGE_DOC_URL);
				EditorPrefs.SetBool(sf_PrefNames.EP_FIRST_LAUNCH_BOOL.Name, false );
			}
		}

		private GUIContent openAllContent = new GUIContent("Open All", "This will open all the forges in  Script Forge");
		private GUIContent closeAllConent = new GUIContent("Close All", "This will close all open forges in  Script Forge");
		private GUIContent generateAllContent = new GUIContent("Generate All", "This will tell all forges to generate their scripts if they have changed since last time");
		private GUIContent setCommonPathContent = new GUIContent("Set Common Path", "This is the path that all forges will build their scripts to.");
		private GUIContent resetForgesContenet = new GUIContent("Reset Forges", "This will reset all forges to their default values.");

		public void OnGUI()
		{
			_scrollPostion = EditorGUILayout.BeginScrollView( _scrollPostion, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, GUI.skin.scrollView);
			GUILayout.Space( 5.0f );
			EditorWidget.Spacer();
			
			//Top Header
			GUILayout.BeginHorizontal();
			if( GUILayout.Button(openAllContent, EditorStyles.miniButtonLeft, GUILayout.Height(20)) )
				if( ForgeWidget._OnOpen != null )
					ForgeWidget._OnOpen();
			if( GUILayout.Button(closeAllConent, EditorStyles.miniButtonRight, GUILayout.Height(20)))
				if( ForgeWidget._OnClose != null )
					ForgeWidget._OnClose();
			
			GUILayout.EndHorizontal();
			
			EditorWidget.Spacer();
			
			if( EditorWidget._OnGUI != null )
				ForgeWidget._OnGUI();
			
			GUILayout.BeginHorizontal();
			if( GUILayout.Button(generateAllContent, EditorStyles.miniButtonLeft, GUILayout.Height(30)) )
				if( ForgeWidget._OnGenerateAll != null )
					ForgeWidget._OnGenerateAll();
			if( GUILayout.Button(setCommonPathContent, EditorStyles.miniButtonMid, GUILayout.Height(30)) )
			{
				if( ForgeWidget._OnSetCommonPath != null )
					ForgeWidget._OnSetCommonPath( EditorUtility.OpenFolderPanel( "Set Path", "", Application.dataPath + "/"));
			}
			if( GUILayout.Button(resetForgesContenet, EditorStyles.miniButtonRight, GUILayout.Height(30)) )
			{
				if( ForgeWidget._OnReset != null )
					ForgeWidget._OnReset(); 
			}
			GUILayout.Space(15);
			GUILayout.EndHorizontal();
			
			EditorWidget.Spacer();
			EditorGUILayout.EndScrollView();
		}
	}
}