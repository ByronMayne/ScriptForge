using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace ScriptForge
{
 
    public abstract class ForgeWidget : EditorWidget
    {
        /// <summary>
        /// Since we want to be able to build all scripts at once we have a static event for that. 
        /// </summary>
        public static Action _OnGenerateAll;
		/// <summary>
		/// This static function is used to set a common build path to all forges. 
		/// </summary>
		public static Action<string> _OnSetCommonPath;
		/// <summary>
		/// This is called to close the foldout
		/// </summary>
		public static Action _OnClose;
		/// <summary>
		/// This is called to open the foldout.  
		/// </summary>
		public static Action _OnOpen;
		/// <summary>
		/// This event is called by the ScriptForge.cs whenever the project has changed. 
		/// </summary>
		public static Action _OnAutoBuild;
		/// <summary>
		/// This is called by ScriptForge.cs when all widgets should be reset. 
		/// </summary>
		public static Action _OnReset;
        /// <summary>
        /// This is the location that the script will be built too.
        /// </summary>
		public string _buildPath = "/";
        /// <summary>
        /// This is the name of the script that we are saving.
        /// </summary>
		public string _scriptName = "ForgedScript";
        /// <summary>
        /// If this is true whenever the project changes the scripts will be built. 
        /// </summary>
		public bool _autoBuild = false;
        /// <summary>
        /// If the directory is invlid this will be true;
        /// </summary>
        public bool _isValidDirectory = true;
        /// <summary>
        /// This is the language that this editor compiles the script too.
        /// </summary>
        public ProgrammingLanguages _language; 
		/// <summary>
		/// The namespace of the call created
		/// </summary>
		public string _namespace = "";
		/// <summary>
		/// This is used to limit the size of the content labels. 
		/// </summary>
		protected const int CONTENT_TITLE_WIDTH = 110;
		/// <summary>
		/// This is used to check the last source of data. If the data is the name we don't want to rebulid the scripts. 
		/// </summary>
		protected string[] _lastSourceInfo;

		#region -= GUI Content =- 
            /// <summary>
            /// The label for our autobuild. 
            /// </summary>
            private GUIContent _autoBuildContent = new GUIContent("Auto Build:", "If this is set to true ScriptForge will compile your code everytime it changes");
            /// <summary>
            /// The label for our build path. 
            /// </summary>
            private GUIContent _buildPathContent = new GUIContent("Build Path:", "This is the location that ScriptForge will compile your scripts to. Please make sure its correct. example ' /Scripts/Tag/ ' will go in Assets->Scipts->Tag");
            /// <summary>
            /// The label for our scipts name
            /// </summary>
            private GUIContent _compiledScriptContent = new GUIContent("Build Name:", "This is the name of the script generated from this Forge");
			/// <summary>
			/// This is the namespace the compiled script will be placed under.
		    /// </summary>
		    private GUIContent _namespaceConent = new GUIContent("Namespace:", "This is the namespace of the generated class. This is not required");
		    /// <summary>
		    /// This is the language of the script that will be generated. 
		    /// </summary>
            private GUIContent _languageContent = new GUIContent("Language:", "This the language that you would like the editor to compile your scripts too. Right now the only option is C# but JavaScript and Boo will be added in the future.");
       		/// <summary>
       		/// The is used to remove a forge. 
       		/// </summary>
			private GUIContent _removeForgeContent = new GUIContent("Remove Forge", "This will removed the forge from Script Forge and the scripts will not be generated.");
		#endregion 

        #region -= Editor Prefs =-
        #endregion 

        /// <summary>
        /// The constructor we use to build our Forge items. 
        /// </summary>
        /// <param name="Name">This is the name that will showup on the foldout at the top.</param>
        /// <param name="Tooltip">This is the message that the user will see when they put their mouse over the foldout.</param>
        /// <param name="Height">How tall will the editor box be when fully opened?</param>
        public ForgeWidget() : base()
        {
            _OnGenerateAll += OnGenerate;
			_OnAutoBuild += OnAutoBuild; 
			_OnSetCommonPath += SetPath; 
			_OnOpen += OpenFoldout;
			_OnClose += CloseFoldout;
			_OnReset += OnReset; 
        }

        /// <summary>
        /// This is the deconstructor. It's only used to unsubscribe our OnGenerate method from the static delegate. (It will cause errors if you don't);
        /// </summary>
		public override void Destroy()
        {
			base.Destroy();
            _OnGenerateAll -= OnGenerate;
			_OnAutoBuild -= OnAutoBuild; 
			_OnSetCommonPath -= _OnSetCommonPath; 
			_OnOpen -= OpenFoldout;
			_OnClose -= CloseFoldout;
			_OnReset -= OnReset; 
        }

        protected override void LoadPrefValues()
        {
            base.LoadPrefValues();

			_buildPath  = sf_EditorPrefs.GetPref( sf_EditorPrefs.EP_BUILD_PATH, this );
			_namespace  = sf_EditorPrefs.GetPref( sf_EditorPrefs.EP_NAMESPACE,   this );
			_scriptName = sf_EditorPrefs.GetPref( sf_EditorPrefs.EP_SCRIPT_NAME, this );
			_autoBuild  = sf_EditorPrefs.GetPref( sf_EditorPrefs.EP_AUTO_BUILD,  this );
        }


        protected override void SavePrefValues()
        {
            base.SavePrefValues(); 

			sf_EditorPrefs.SetPref( sf_EditorPrefs.EP_BUILD_PATH,  _buildPath,  this );
			sf_EditorPrefs.SetPref( sf_EditorPrefs.EP_SCRIPT_NAME, _scriptName, this );
			sf_EditorPrefs.SetPref( sf_EditorPrefs.EP_NAMESPACE,   _namespace,  this );
			sf_EditorPrefs.SetPref( sf_EditorPrefs.EP_AUTO_BUILD,  _isOpen,     this );
        }

        /// <summary>
        /// This is called so that the default items will be drawn before and after. Overload this to remove the middle content
        /// </summary>
        protected override void DrawWindowContent()
        {

			GUILayout.Space(4);
			
			GUILayout.Box( GUIContent.none, GUILayout.Width(_widgetRect.width - EDITOR_WINDOW_INSET * 2), GUILayout.Height(4));
			
			GUILayout.Space(4);

            EditorGUI.BeginChangeCheck();

			GUILayout.BeginHorizontal();
			GUILayout.Label(_autoBuildContent, EditorStyles.boldLabel, GUILayout.Width(CONTENT_TITLE_WIDTH) );
            	_autoBuild = GUILayout.Toggle(_autoBuild, GUIContent.none);
			GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
			GUILayout.Label(_buildPathContent, EditorStyles.boldLabel, GUILayout.Width (CONTENT_TITLE_WIDTH));
				GUILayout.Label(_buildPath, EditorStyles.miniLabel);
			if( GUILayout.Button(changePathContent, sf_Skins.Button, GUILayout.Width(CONTENT_TITLE_WIDTH) ))
					SetPath( EditorUtility.OpenFolderPanel( "Set Path", _buildPath, Application.dataPath + "/") );
            GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label(_namespaceConent,  EditorStyles.boldLabel, GUILayout.Width(CONTENT_TITLE_WIDTH));
			_namespace = GUILayout.TextArea(_namespace);
			GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
			GUILayout.Label(_compiledScriptContent,  EditorStyles.boldLabel, GUILayout.Width(CONTENT_TITLE_WIDTH));
            	_scriptName = GUILayout.TextArea(_scriptName);
            GUILayout.EndHorizontal();


			GUILayout.BeginHorizontal();
			GUILayout.Label(_languageContent, EditorStyles.boldLabel, GUILayout.Width(CONTENT_TITLE_WIDTH) );
            _language = (ProgrammingLanguages)EditorGUILayout.EnumPopup(GUIContent.none, _language);

			GUILayout.EndHorizontal();
            DrawForgeContent();

			GUILayout.Space(4);

			GUILayout.Box( GUIContent.none, GUILayout.Width(_widgetRect.width - EDITOR_WINDOW_INSET * 2), GUILayout.Height(4));

			GUILayout.Space(4);

            GUILayout.BeginHorizontal();
			if (GUILayout.Button(generateContent, sf_Skins.MiniButtonLeft, GUILayout.Height(25)))
				OnGenerate();
			if (GUILayout.Button(resetContent, sf_Skins.MiniButtonMiddle, GUILayout.Height(25)))
				OnReset();
			if (GUILayout.Button(_removeForgeContent, sf_Skins.MiniButtonRight, GUILayout.Height(25)))
				OnRemoved();
            GUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
                SavePrefValues();

        }

		private GUIContent generateContent = new GUIContent("Generate", "This will make this forge generate its script if the values have changed since last build.");
		private GUIContent resetContent = new GUIContent("Reset Forges", "This will reset the forge to its default values");
		private GUIContent changePathContent = new GUIContent("Change Path", "This will open a file explorer to pick where you want your script to build to");

        /// <summary>
        /// This is the gui content that draw between DrawWindowContent which is draw between OnGUI from the base class; 
        /// </summary>
        protected virtual void DrawForgeContent() {}

        /// <summary>
        /// This is the code that will generate our scripts. 
        /// </summary>
        protected void OnGenerate()
        {
            if ( !CheckSciptName() ) return;

			FlashBuild();

			_buildPath = _buildPath.Replace("Assets", "");

            GenerateCode();

			_buildPath = "Assets" + _buildPath;
        }

		/// <summary>
		/// Raises the removed event.
		/// </summary>
		protected virtual void OnRemoved()
		{

			ScriptForge.RemoveWidget( this );
			Destroy();

		}

		/// <summary>
		/// This function is called from ScriptForge.cs whenever
	    /// the project changes. It will thene generate code only 
		/// if auto build is true.
		/// </summary>
		private void OnAutoBuild()
		{
			if( _autoBuild )
				OnGenerate();
		}

		private void SetPath(string newPath)
		{
			newPath = newPath.Replace(Application.dataPath, "");
			_buildPath = "Assets" + newPath;

			SavePrefValues();

		}
		
		public abstract void GenerateCode();

        /// <summary>
        /// This will reset all the values of the forge
        /// </summary>
        public virtual void OnReset()
        {
			_buildPath = "./";
			_autoBuild = false;
			_scriptName = "Not Set";
			_namespace = "";
			_language = ProgrammingLanguages.CSharp;
        }

		/// <summary>
		/// This is called to close the foldout
		/// </summary>
		public virtual void CloseFoldout()
		{
			_isOpen = false;
		}

		/// <summary>
		/// This is called to open the foldout
		/// </summary>
		public virtual void OpenFoldout()
		{
			_isOpen = true;
		}
  
        private GUIContent csErrorNotificationContent = new GUIContent("Error! Please remove '.cs' from script name");
        private GUIContent spacesErrorNotificationContent = new GUIContent("Error! Please remove spaces from script name.");
		private GUIContent noNameSetErrorNotificationContent = new GUIContent("Error! Please give the script a name.");


        public virtual bool CheckSciptName()
        {
			if( _scriptName == "" || _scriptName == "Not Set" )
			{
				ScriptForge.Instance.ShowNotification(noNameSetErrorNotificationContent);
				FlashError();
				return false;
			}
            else if (_scriptName.Contains(".cs"))
            {
                ScriptForge.Instance.ShowNotification(csErrorNotificationContent);
                FlashError();
                return false;
            }
            else if (_scriptName.Contains(" "))
            {
                ScriptForge.Instance.ShowNotification(spacesErrorNotificationContent);
                FlashError();
                return false;
            }
            else
                return true;
        }

    } 
}
