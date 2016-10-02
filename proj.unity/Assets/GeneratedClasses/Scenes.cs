
/// <summary>
/// This is an enum used for all your scenes. It can be
/// used as an int to represent the scene ID number. 
/// </summary>
namespace Scriptforge.Generated
{
	public enum Types
	{
		TestScene = 0,
		TestScene2 = 1,
	}

	public static class Scenes
	{	
		private static string[] sceneNames = new string[2] { "TestScene", "TestScene2",  };
		public const string TestScene  = "TestScene";
	  public const string TestScene2  = "TestScene2";
	  

		/// <summary>
		/// This function takes in a string name and returns 
		/// the scene ID with that name. If the name is invalid
		/// it return -1.
		/// </summary>
		/// <returns>The ID of the requested scene name.</returns>
		/// <param name="aName">A name of the scene you want the ID for.</param>
		public static int SceneNameToID(string aName)
		{
			for( int i = 0; i < 2; i++)
			{
				if( sceneNames[i] == aName )
					return i;
			}
			
			//No scene found with that ID.
			return -1;
		}
		
		/// <summary>
		/// This takes a scene ID and returns the name. If
		/// the ID is invalid it returns 'None'.
		/// </summary>
		/// <returns>The identifier of the scene.</returns>
		/// <param name="anID">An name of the scene with the requested ID</param>
		public static string SceneIDToName(int anID)
		{
			if( anID >= 0 && anID < 2 )
				return sceneNames[anID];
			else
				return "None";
		}
		
		/// <summary>
		/// Determines if it is valid scene name.
		/// </summary>
		/// <returns><c>true</c> if is valid scene name the specified aName; otherwise, <c>false</c>.</returns>
		/// <param name="aName">A name.</param>
		public static bool IsValidSceneName(string aName)
		{
			for( int i = 0; i < 2; i++)
			{
				if( sceneNames[i] == aName )
					return true;
			}
			
			return false;
		}
		
		/// <summary>
		/// Determines if is valid scene ID.
		/// </summary>
		/// <returns><c>true</c> if is valid scene I the specified anID; otherwise, <c>false</c>.</returns>
		/// <param name="anID">An I.</param>
		public static bool IsValidSceneID(int anID)
		{
			if( anID >= 0 && anID < 2 )
					return true;
				else
					return false;
		}
	}
}
		
	
		
