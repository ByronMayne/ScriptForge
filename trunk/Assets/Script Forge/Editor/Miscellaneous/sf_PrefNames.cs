
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
		}
}

