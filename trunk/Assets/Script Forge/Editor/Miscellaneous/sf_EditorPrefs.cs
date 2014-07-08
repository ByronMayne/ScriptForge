using System;
using UnityEditor;
using UnityEngine;

namespace ScriptForge
{
		/// <summary>
		/// This class is used to keep track of player prefs and their default values.
		/// This stops us for using magic numbers or constants at the top of every class.
		/// </summary>
		public sealed class PrefInfo <T>
		{
			public PrefInfo ( string prefName, T prefValue )
			{
				Name = prefName;
				Default = prefValue;
			}
			public readonly string Name;
			public readonly T Default; 
		}



		public static class sf_EditorPrefs
		{
			/// <summary>
			/// This pref is used to tell if the user is launching 
			/// Script Forge for the first time. If so it will open
			/// the Google Doc.
			/// </summary>
			public static readonly PrefInfo<bool> EP_FIRST_LAUNCH_BOOL = new PrefInfo<bool>("First Launch", true);
			/// <summary>
			/// This pref is used to keep track of which
			/// Widgets are open or closed.
			/// </summary>
			public static readonly PrefInfo<bool> EP_IS_OPEN       = new PrefInfo<bool>( "IsOpen", false );
			/// <summary>
			/// This pref is used to save the default build loction for each forge.
			/// </summary>
			public static readonly PrefInfo<string> EP_BUILD_PATH  = new PrefInfo<string>( "buildpath", "/" );
			/// <summary>
			/// This pref is used to save teh default namespace of the generated script.
			/// </summary>
			public static readonly PrefInfo<string> EP_NAMESPACE   = new PrefInfo<string>( "namespace", "" );
			/// <summary>
			/// This pref is used to store the script name.
			/// </summary>
			public static readonly PrefInfo<string> EP_SCRIPT_NAME = new PrefInfo<string>( "scriptName", "Insert Name" );
			/// <summary>
			/// This script is used to store the toggle for auto build.
			/// </summary>
			public static readonly PrefInfo<bool> EP_AUTO_BUILD  = new PrefInfo<bool>( "autoBuild", false );
			/// <summary>
			/// This is the enum used to list all the scenes in the Unity project.
			/// </summary>
			public static readonly PrefInfo<string> EP_SCENES_ENUMNAME = new PrefInfo<string>( "Scene Enum Name", "Scenes" );
			/// <summary>
			/// This is used to store the custom color of the inspector. 
			/// </summary>
			public static readonly PrefInfo<Color> EP_CUSTOM_INSPECTOR_COLOR = new PrefInfo<Color>( "Scene Type", Color.white );
			/// <summary>
			/// This is used to store the default skin type of the inspector. 
			/// </summary>
			public static readonly PrefInfo<int> EP_INSPECTOR_SKIN_TYPE = new PrefInfo<int>( "Skin Type", -1 );
		                                                                               

			public static readonly PrefInfo<bool> EP_HAS_LAYERS_WIDGET_ACTIVE = new PrefInfo<bool>( "Layers Widget Active", false );
			public static readonly PrefInfo<bool> EP_HAS_SORTING_LAYERS_WIDGET_ACTIVE = new PrefInfo<bool>( "Sorting Layers Widget Active", false );
			public static readonly PrefInfo<bool> EP_HAS_TAGS_WIDGET_ACTIVE = new PrefInfo<bool>( "Tags Widget Active", false );
			public static readonly PrefInfo<bool> EP_HAS_SCENE_WIDGET_ACTIVE = new PrefInfo<bool>( "Scene Widget Active", false );
			public static readonly PrefInfo<bool> EP_HAS_INPUT_WIDGET_ACTIVE = new PrefInfo<bool>( "Input Widget Active", false );

			private const string PREF_ARG_SPLITTER = ",";
			/// <summary>
			/// Sets the preference for a bool.
			/// </summary>
			/// <param name="Pref">Preference.</param>
			/// <param name="Value">If set to <c>true</c> value.</param>
			/// <param name="Owner">Owner.</param>
			/// <param name="UniqueID">Unique I.</param>
			public static void SetPref(PrefInfo<bool> Pref, bool Value, object Owner, string UniqueID = "" )
			{
				EditorPrefs.SetBool( Pref.Name + UniqueID + Owner.GetType().Name, Value );
			}
			/// <summary>
			/// Sets the preference for an Int
			/// </summary>
			/// <param name="Pref">Preference.</param>
			/// <param name="Value">Value.</param>
			/// <param name="Owner">Owner.</param>
			/// <param name="UniqueID">Unique I.</param>
			public static void SetPref(PrefInfo<int> Pref, int Value, object Owner, string UniqueID = "" )
			{
				EditorPrefs.SetInt( Pref.Name + UniqueID + Owner.GetType().Name, Value );
			}
			/// <summary>
			/// Sets the preference for an string
			/// </summary>
			/// <param name="Pref">Preference.</param>
			/// <param name="Value">Value.</param>
			/// <param name="Owner">Owner.</param>
			/// <param name="UniqueID">Unique I.</param>
			public static void SetPref(PrefInfo<string> Pref, string Value, object Owner, string UniqueID = "" )
			{
				EditorPrefs.SetString( Pref.Name + UniqueID + Owner.GetType().Name, Value );
			}
			/// <summary>
			/// Sets the preference for a float.
			/// </summary>
			/// <param name="Pref">Preference.</param>
			/// <param name="Value">Value.</param>
			/// <param name="Owner">Owner.</param>
			/// <param name="UniqueID">Unique I.</param>
			public static void SetPref(PrefInfo<float> Pref, float Value, object Owner, string UniqueID = "" )
			{
				EditorPrefs.SetFloat( Pref.Name + UniqueID + Owner.GetType().Name, Value );
			}
			/// <summary>
			/// Sets the preference for a Color.
			/// </summary>
			/// <param name="Pref">Preference.</param>
			/// <param name="Value">Value.</param>
			/// <param name="Owner">Owner.</param>
			/// <param name="UniqueID">Unique I.</param>
			public static void SetPref(PrefInfo<Color> Pref, Color Value, object Owner, string UniqueID = "" )
			{
				string ColorString = Value.r + PREF_ARG_SPLITTER + Value.g + PREF_ARG_SPLITTER + Value.b + PREF_ARG_SPLITTER + Value.a;
				EditorPrefs.SetString( Pref.Name + UniqueID + Owner.GetType().Name, ColorString);
			}
			/// <summary>
			/// Gets the preference for a bool
			/// </summary>
			/// <returns><c>true</c>, if preference was gotten, <c>false</c> otherwise.</returns>
			/// <param name="Pref">Preference.</param>
			/// <param name="Owner">Owner.</param>
			/// <param name="UniqueID">Unique I.</param>
			public static bool GetPref(PrefInfo<bool> Pref, object Owner, string UniqueID = "" )
			{
				return EditorPrefs.GetBool( Pref.Name + UniqueID + Owner.GetType().Name, Pref.Default );
			}
			/// <summary>
			/// Gets the preference for an Int.
			/// </summary>
			/// <returns>The preference.</returns>
			/// <param name="Pref">Preference.</param>
			/// <param name="Owner">Owner.</param>
			/// <param name="UniqueID">Unique I.</param>
			public static int GetPref(PrefInfo<int> Pref, object Owner, string UniqueID = "" )
			{
				return EditorPrefs.GetInt( Pref.Name + UniqueID + Owner.GetType().Name, Pref.Default );
			}
			/// <summary>
			/// Gets the preference for a float.
			/// </summary>
			/// <returns>The preference.</returns>
			/// <param name="Pref">Preference.</param>
			/// <param name="Owner">Owner.</param>
			/// <param name="UniqueID">Unique I.</param>
			public static float GetPref(PrefInfo<float> Pref, object Owner, string UniqueID = "" )
			{
				return EditorPrefs.GetFloat( Pref.Name + UniqueID + Owner.GetType().Name, Pref.Default );
			}
			/// <summary>
			/// Gets the preference for a string.
			/// </summary>
			/// <returns>The preference.</returns>
			/// <param name="Pref">Preference.</param>
			/// <param name="Owner">Owner.</param>
			/// <param name="UniqueID">Unique I.</param>
			public static string GetPref(PrefInfo<string> Pref, object Owner, string UniqueID = "" )
			{
				return EditorPrefs.GetString( Pref.Name + UniqueID + Owner.GetType().Name, Pref.Default );
			}
			/// <summary>
			/// Gets the preference for a color. 
			/// </summary>
			/// <returns>The preference.</returns>
			/// <param name="Pref">Preference.</param>
			/// <param name="Owner">Owner.</param>
			/// <param name="UniqueID">Unique I.</param>
			public static Color GetPref(PrefInfo<Color> Pref, object Owner, string UniqueID = "" )
			{
				
				string defaultColor = Pref.Default.r + PREF_ARG_SPLITTER + Pref.Default.g + PREF_ARG_SPLITTER + Pref.Default.b + PREF_ARG_SPLITTER + Pref.Default.a;

				string ColorString = EditorPrefs.GetString( Pref.Name + UniqueID + Owner.GetType().Name, defaultColor );
				
				string[] ColorElements = ColorString.Split( new string [1]{PREF_ARG_SPLITTER}, StringSplitOptions.None );
				
				return new Color(float.Parse(ColorElements[0]),  //RED
				                 float.Parse(ColorElements[1]),  //GREEN
				                 float.Parse(ColorElements[2]),  //BLUE
				                 float.Parse(ColorElements[3])); //ALPHA
			}
		}


}

