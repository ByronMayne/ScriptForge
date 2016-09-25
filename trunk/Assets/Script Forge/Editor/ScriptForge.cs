using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic; 
using System.Linq;

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
			if( !_widgets.OfType<T_WidgetType>().Any() )
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

			LoadWidgets();
		
		}

		private void LoadWidgets()
		{

			RemoveAllWidgets();

			if( EditorPrefs.GetBool(sf_EditorPrefs.EP_FIRST_LAUNCH_BOOL.Name, sf_EditorPrefs.EP_FIRST_LAUNCH_BOOL.Default ) )
			{
				System.Diagnostics.Process.Start(ExtenalLinks.SCRIPT_FORGE_GOOLGE_DOC_URL);
				EditorPrefs.SetBool(sf_EditorPrefs.EP_FIRST_LAUNCH_BOOL.Name, false );
			}
			
			if( EditorPrefs.GetBool( sf_EditorPrefs.EP_HAS_LAYERS_WIDGET_ACTIVE.Name, sf_EditorPrefs.EP_HAS_LAYERS_WIDGET_ACTIVE.Default ))
				AddWidget<LayersWidget>();
			
			if( EditorPrefs.GetBool( sf_EditorPrefs.EP_HAS_SORTING_LAYERS_WIDGET_ACTIVE.Name, sf_EditorPrefs.EP_HAS_SORTING_LAYERS_WIDGET_ACTIVE.Default ))
				AddWidget<SortingLayersWidget>();
			
			if( EditorPrefs.GetBool( sf_EditorPrefs.EP_HAS_TAGS_WIDGET_ACTIVE.Name, sf_EditorPrefs.EP_HAS_TAGS_WIDGET_ACTIVE.Default ))
				AddWidget<TagsWidget>();
			
			if( EditorPrefs.GetBool( sf_EditorPrefs.EP_HAS_SCENE_WIDGET_ACTIVE.Name, sf_EditorPrefs.EP_HAS_SCENE_WIDGET_ACTIVE.Default ))
				AddWidget<SceneWidget>();
			
			if( EditorPrefs.GetBool( sf_EditorPrefs.EP_HAS_INPUT_WIDGET_ACTIVE.Name, sf_EditorPrefs.EP_HAS_INPUT_WIDGET_ACTIVE.Default ))
				AddWidget<InputWidget>();
			
			AddWidget<SettingsWidget>();
			AddWidget<AboutWidget>();
		}

		private void SaveWidgets()
		{
			EditorPrefs.SetBool( sf_EditorPrefs.EP_HAS_LAYERS_WIDGET_ACTIVE.Name, _widgets.OfType<LayersWidget>().Any() );
			EditorPrefs.SetBool( sf_EditorPrefs.EP_HAS_SORTING_LAYERS_WIDGET_ACTIVE.Name, _widgets.OfType<SortingLayersWidget>().Any() );
			EditorPrefs.SetBool( sf_EditorPrefs.EP_HAS_TAGS_WIDGET_ACTIVE.Name, _widgets.OfType<TagsWidget>().Any() );
			EditorPrefs.SetBool( sf_EditorPrefs.EP_HAS_SCENE_WIDGET_ACTIVE.Name, _widgets.OfType<SceneWidget>().Any() );
			EditorPrefs.SetBool( sf_EditorPrefs.EP_HAS_INPUT_WIDGET_ACTIVE.Name, _widgets.OfType<InputWidget>().Any() );
		}

		private void RemoveAllWidgets()
		{
			foreach( EditorWidget wig in _widgets )
			{
				wig.Destroy();
			}

			_widgets.Clear();
		}

		public void RefreshWidgets()
		{
			SaveWidgets();

			RemoveAllWidgets();

			LoadWidgets();
		}

		public void OnDisable()
		{
			SaveWidgets();

			RemoveAllWidgets();
		}

		/// <summary>
		/// When the project has been changed we use 
		/// this event to fire all the auto build actions on our forges. 
		/// </summary>
		private void OnProjectChange()
		{
			RemoveAllWidgets();

			LoadWidgets();

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


			if( EditorPrefs.GetBool(sf_EditorPrefs.EP_FIRST_LAUNCH_BOOL.Name, sf_EditorPrefs.EP_FIRST_LAUNCH_BOOL.Default ) )
			{
				System.Diagnostics.Process.Start(ExtenalLinks.SCRIPT_FORGE_GOOLGE_DOC_URL);
				EditorPrefs.SetBool(sf_EditorPrefs.EP_FIRST_LAUNCH_BOOL.Name, false );
			}
		}

		private GUIContent openAllContent = new GUIContent("Open All Forges", "This will open all the forges in  Script Forge");
		private GUIContent closeAllConent = new GUIContent("Close All Forges", "This will close all open forges in  Script Forge");
		private GUIContent addForgeContent = new GUIContent("Add Forge", "This will add a new forge that is not already part of Script Forge");
		private GUIContent generateAllContent = new GUIContent("Generate All", "This will tell all forges to generate their scripts if they have changed since last time");
		private GUIContent setCommonPathContent = new GUIContent("Set Common Path", "This is the path that all forges will build their scripts to.");
		private GUIContent resetForgesContenet = new GUIContent("Reset Forges", "This will reset all forges to their default values.");

		public void OnGUI()
		{
	
			GUILayout.Space(-10.0f);
			GUILayout.BeginHorizontal();
			GUILayout.Space(-10.0f);
			GUILayout.Box( GUIContent.none, GUILayout.Width(Screen.width + 20.0f), GUILayout.Height(60.0f));
	
			GUILayout.EndHorizontal();
			GUILayout.Space(-55);
			GUILayout.Box( GUIContent.none, GUILayout.Width(Screen.width - 9), GUILayout.Height(45.0f));

			GUILayout.BeginArea(new Rect(0.0f, 0.0f, Screen.width, 65.0f ));

			GUILayout.BeginHorizontal();
				GUILayout.Space(10.0f);
				GUILayout.Label( FontAwesomeIcons.CUBES.ToString(), sf_Skins.FontAwesomLargeStyle, GUILayout.Width( 50) );
				GUILayout.BeginVertical();
					GUILayout.Label( sf_Descriptions.DESCRIPTION_SCRIPTFORGE_TITLE, sf_Skins.InspectorTitleStyle );
					GUILayout.Label( sf_Descriptions.DESCRIPTION_SCRIPTFORGE_SUBTITLE, sf_Skins.InspectorSubTitleStyle );
				GUILayout.EndVertical();
				GUILayout.Label( FontAwesomeIcons.CUBES.ToString(), sf_Skins.FontAwesomLargeStyle, GUILayout.Width( 50) );
			GUILayout.Space(10);
			GUILayout.EndHorizontal();

			GUILayout.EndArea();

			GUILayout.Space(2.0f);

		
			GUILayout.Space( 3.0f );

			GUILayout.BeginHorizontal();
				if( GUILayout.Button(generateAllContent, sf_Skins.Button, GUILayout.Height(30) ) )
				{
					if( ForgeWidget._OnGenerateAll != null )
						ForgeWidget._OnGenerateAll();
				}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();

				EditorGUI.BeginChangeCheck();
				AddForgeWidget.ButtonDownState = GUILayout.Toggle(AddForgeWidget.ButtonDownState, addForgeContent, EditorStyles.miniButtonLeft, GUILayout.Height(30));
				if( EditorGUI.EndChangeCheck())
				{
					if( AddForgeWidget.ButtonDownState )
						AddForgeWidget.Open();
					else
						AddForgeWidget.Close();
				}
	

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
			GUILayout.EndHorizontal();


			AddForgeWidget.OnGUI();

			//Top Header
			GUILayout.BeginHorizontal();
			if( GUILayout.Button(openAllContent, EditorStyles.miniButtonLeft, GUILayout.Height(20)) )
				if( ForgeWidget._OnOpen != null )
					ForgeWidget._OnOpen();
			if( GUILayout.Button(closeAllConent, EditorStyles.miniButtonRight, GUILayout.Height(20)))
				if( ForgeWidget._OnClose != null )
					ForgeWidget._OnClose();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Space( -5.0f );
			GUILayout.Box( GUIContent.none, GUILayout.Width(Screen.width + 5.0f ), GUILayout.Height(6));

			GUILayout.EndHorizontal();

			GUILayout.Space( -3.0f );

			_scrollPostion = EditorGUILayout.BeginScrollView( _scrollPostion, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, GUI.skin.scrollView);

			EditorWidget.Spacer();
			
			
			if( EditorWidget._OnGUI != null )
				ForgeWidget._OnGUI();

			
			EditorGUILayout.EndScrollView();


		}

	}
}