using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

namespace ScriptForge
{
	public enum SkinTypes
	{
		Pro = 0,
		Free = 1,
		Custom = 3
	}

    public class SettingsWidget : EditorWidget
    {
		private GUIContent _animateContent = new GUIContent("Animate Widgets", "If this is true the Widgets will grow and shink and fade in and out. This does not effect the code and only makes it look nice.");
		private GUIContent _skinTypeContent = new GUIContent("Skin Type", "This is the skin that ScriptForge will use to show everything. This will be set automaticly on first launch");
		private SkinTypes _skinType;
		private Color _customColor = Color.grey;
		private const char COLOR_SPLIT_CHAR = ',';

		public Color CustomColor 
		{
			get 
			{
				return _customColor;
			}
			set 
			{
				_customColor = value;
			}
		}

		public SkinTypes SkinType
		{
			get
			{
				return _skinType;
			}

			set
			{
				_skinType = value; 

				switch (value) 
				{
					case SkinTypes.Pro:
							sf_Skins.ChangeToUnityProSkin();
						break;
					case SkinTypes.Free:
							sf_Skins.ChangeToUnitySkin();
						break;
					case SkinTypes.Custom:
							sf_Skins.ChangeToCustomSkin( _customColor );
						break;
					default:
						throw new System.ArgumentOutOfRangeException ();
				}
			}
		}

		public SettingsWidget() 
		{ 

		}

		public override void Destroy()
		{ 
			base.Destroy();

			_OnGUI -= OnGUI; 
		}

		protected override void LoadPrefValues ()
		{
			base.LoadPrefValues ();

			string[] ColorString = EditorPrefs.GetString( this.GetType().ToString() + sf_PrefNames.EP_CUSTOM_INSPECTOR_COLOR, sf_PrefNames.EP_CUSTOM_INSPECTOR_COLOR.Default ).Split(COLOR_SPLIT_CHAR);

			try
			{
				CustomColor = new Color( float.Parse( ColorString[0] ), float.Parse( ColorString[1] ), float.Parse( ColorString[2] ) );
			}
			catch( Exception e )
			{
				CustomColor = Color.gray;
			}

			SkinType = (SkinTypes)EditorPrefs.GetInt(this.GetType().ToString() + sf_PrefNames.EP_INSPECTOR_SKIN_TYPE, EditorGUIUtility.isProSkin ? 0 : 1);

			ScriptForge.Instance.Repaint();
		}


		protected override void SavePrefValues ()
		{
			base.SavePrefValues ();

			string ColorString = CustomColor.r.ToString() + COLOR_SPLIT_CHAR + CustomColor.g.ToString() + COLOR_SPLIT_CHAR + CustomColor.b.ToString();

			EditorPrefs.SetString( this.GetType().ToString() + sf_PrefNames.EP_CUSTOM_INSPECTOR_COLOR, ColorString);

			EditorPrefs.SetInt( this.GetType().ToString() + sf_PrefNames.EP_INSPECTOR_SKIN_TYPE, (int)_skinType) ;
		}

       

        protected override void DrawWindowContent()
        {
			SkinTypes tempSkin = SkinType; 
			Color tempColor = _customColor;

			GUILayout.Space(5.0f);
			GUILayout.Box( GUIContent.none, GUILayout.Width(_widgetRect.width - EDITOR_WINDOW_INSET * 2), GUILayout.Height(4));
			GUILayout.BeginHorizontal();
				GUILayout.Label(_animateContent, EditorStyles.boldLabel, GUILayout.Width(147));
            	_animateBoxes = GUILayout.Toggle(_animateBoxes, GUIContent.none);
			GUILayout.EndHorizontal();
			EditorGUI.BeginChangeCheck();

			GUILayout.BeginHorizontal();
				GUILayout.Label(_skinTypeContent, EditorStyles.boldLabel, GUILayout.Width(147));
				tempSkin =  (SkinTypes)EditorGUILayout.EnumPopup( tempSkin );
				if( tempSkin == SkinTypes.Custom )
				{
					tempColor = EditorGUILayout.ColorField( tempColor );
				}
			GUILayout.EndHorizontal();

			if( EditorGUI.EndChangeCheck() )
			{
				CustomColor = tempColor;
				SkinType = tempSkin;
				ScriptForge.Instance.Repaint();
				SavePrefValues();
			}
        }

		protected override string WidgetIcon ()
		{
			return sf_FontAwesome.fa_Cog.ToString();
		}

		protected override GUIContent Description ()
		{
			return sf_Descriptions.DESCRIPTION_SETTINGS_WIDGET;
		}
    } 
}
