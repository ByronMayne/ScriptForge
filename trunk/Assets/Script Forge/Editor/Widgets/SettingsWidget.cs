using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ScriptForge
{
	public enum SkinTypes
	{
		Pro = 0,
		Free = 1
	}

    public class SettingsWidget : EditorWidget
    {
		public SettingsWidget(string Name, string Tooltip, float Height) : base(Name, Tooltip, Height) 
		{ 
			_widgetSkinName = "Settings"; 
		}
        ~SettingsWidget() 
		{ 
			_OnGUI -= OnGUI; 
		}

		protected override void LoadPrefValues ()
		{
			base.LoadPrefValues ();

			_skinType = (SkinTypes)EditorPrefs.GetInt("Skin Type", EditorGUIUtility.isProSkin ? 0 : 1);
		}

		protected override void SavePrefValues ()
		{
			base.SavePrefValues ();

			EditorPrefs.GetInt("Skin Type", (int)_skinType) ;
		}

        private GUIContent _animateContent = new GUIContent("Animate Widgets", "If this is true the Widgets will grow and shink and fade in and out. This does not effect the code and only makes it look nice.");
		private GUIContent _skinTypeContent = new GUIContent("Skin Type", "This is the skin that ScriptForge will use to show everything. This will be set automaticly on first launch");
		private SkinTypes _skinType;

        protected override void DrawWindowContent()
        {
			GUILayout.Space(5.0f);
			GUILayout.Box( GUIContent.none, GUILayout.Width(_widgetRect.width - EDITOR_WINDOW_INSET * 2), GUILayout.Height(4));
			GUILayout.BeginHorizontal();
				GUILayout.Label(_animateContent, GUILayout.Width(147));
            	_animateBoxes = GUILayout.Toggle(_animateBoxes, GUIContent.none);
			GUILayout.EndHorizontal();
			EditorGUI.BeginChangeCheck();

			_skinType =  (SkinTypes)EditorGUILayout.EnumPopup(_skinTypeContent, _skinType );

			if( EditorGUI.EndChangeCheck() )
			{
				if( _skinType == SkinTypes.Pro )
				{
					_editorSkin = Resources.Load<GUISkin>("UnityForgeSkinPro");
				}
				else
				{
					_editorSkin = Resources.Load<GUISkin>("UnityForgeSkin");
				}
				ScriptForge.Instance.Repaint();
				SavePrefValues();
			}
        }
    } 
}
