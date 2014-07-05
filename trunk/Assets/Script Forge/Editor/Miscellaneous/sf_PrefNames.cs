
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



		public static class sf_PrefNames
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
			public static readonly PrefInfo<string> EP_CUSTOM_INSPECTOR_COLOR = new PrefInfo<string>( "Scene Type", "0.5,0.5,0.5" );
			/// <summary>
			/// This is used to store the default skin type of the inspector. 
			/// </summary>
			public static readonly PrefInfo<int> EP_INSPECTOR_SKIN_TYPE = new PrefInfo<int>( "Skin Type", 0 );
		                                                                               

			public static readonly PrefInfo<bool> EP_HAS_LAYERS_WIDGET_ACTIVE = new PrefInfo<bool>( "Layers Widget Active", false );
			public static readonly PrefInfo<bool> EP_HAS_SORTING_LAYERS_WIDGET_ACTIVE = new PrefInfo<bool>( "Sorting Layers Widget Active", false );
			public static readonly PrefInfo<bool> EP_HAS_TAGS_WIDGET_ACTIVE = new PrefInfo<bool>( "Tags Widget Active", false );
			public static readonly PrefInfo<bool> EP_HAS_SCENE_WIDGET_ACTIVE = new PrefInfo<bool>( "Scene Widget Active", false );
			public static readonly PrefInfo<bool> EP_HAS_INPUT_WIDGET_ACTIVE = new PrefInfo<bool>( "Input Widget Active", false );

		}
}

