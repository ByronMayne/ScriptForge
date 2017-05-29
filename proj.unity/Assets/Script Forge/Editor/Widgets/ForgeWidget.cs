﻿using UnityEngine;
using UnityEditor;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using ScriptForge.Widgets.Components;
using System.IO;
using Type = System.Type;
using Attribute = System.Attribute;
using System.Reflection;

namespace ScriptForge
{
    [System.Serializable]
    public abstract class ForgeWidget : Widget
    {
        [SerializeField]
        protected bool m_AutomaticallyGenerate = false;

        [SerializeField]
        protected string m_ScriptLocation = string.Empty;

        [SerializeField]
        protected string m_Namespace = "";

        [SerializeField]
        protected string m_ClassName = "";

        [SerializeField]
        protected string m_AssetHash;

		[SerializeField]
		protected List<ForgeComponent> m_Components = new List<ForgeComponent>();

        private bool m_IsUpToDate = false;

        /// <summary>
        /// The default name of this class
        /// </summary>
        protected abstract string defaultName { get; }

        /// <summary>
        /// Invoked when this instance is loaded from disk.
        /// </summary>
        public override void OnLoaded()
        {
            if (m_AutomaticallyGenerate)
            {
                OnGenerate(false);
            }
            OnContentChanged();
        }

        /// <summary>
        /// Returns the path to the save location on disk for this class.
        /// </summary>
        /// <returns></returns>
        protected string GetSystemSaveLocation()
        {
            // If we don't have a path defined we should not build.
            if (string.IsNullOrEmpty(m_ScriptLocation))
            {
                return null;
            }

            return Application.dataPath.Replace("/Assets", "/" + m_ScriptLocation);
        }

        /// <summary>
        /// Invoked on the widget when it's first initialized.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (string.IsNullOrEmpty(m_ClassName))
            {
                m_ClassName = defaultName;
            }
            SetupComponents();
        }

        /// <summary>
        /// When we 
        /// </summary>
        /// <param name="saveList"></param>
        public override void PopulateSaveFile(List<ScriptableObject> saveList)
        {
            // Add ourself
            base.PopulateSaveFile(saveList);
            // And all our components
            for(int i = 0; i < m_Components.Count; i++)
            {
                saveList.Add(m_Components[i]);
            }
        }

        /// <summary>
        /// Invoked when the forge is enabled. We us this to set all the components
        /// we require for our forge.
        /// </summary>
        protected void SetupComponents()
        {
            // Removed null entries (script changes can make this happen)
            for (int i = m_Components.Count - 1; i >= 0; i--)
            {
                if (m_Components[i] == null)
                {
                    m_Components.RemoveAt(i);
                }
            }
            // Get our current type
            Type forgeType = GetType();
            // Get the attribute
            RequiredWidgetComponetsAttribute requiredWidgets = Attribute.GetCustomAttribute(forgeType, typeof(RequiredWidgetComponetsAttribute)) as RequiredWidgetComponetsAttribute;
            // If it's not null
            if(requiredWidgets != null)
            {
                // Loop over all required types
                foreach (Type requiredType in requiredWidgets.requiredTypes)
                {
                    // Set a flag to see if we have a match.
                    bool foundType = false;

                    // loop over all our components
                    foreach (ForgeComponent component in m_Components)
                    {
                        // Check if the type matches
                        if(component.GetType() == requiredType)
                        {
                            // We have a match. 
                            foundType = true;
                            break;
                        }
                    }
                    // If we did not find a match we have to create one
                    if(!foundType)
                    {
                        // Create the instance.
                        ForgeComponent component = CreateInstance(requiredType) as ForgeComponent;
                        // Add it to our list
                        m_Components.Add(component);
                    }
                }
            }
        }


        /// <summary>
        /// Draws the title of the widget and it's icons.
        /// </summary>
        /// <param name="style">The style we use to draw it's content.</param>
        public override void OnTitleBarGUI(ScriptForgeStyles style)
        {
            base.OnTitleBarGUI(style);

            GUILayout.FlexibleSpace();

            if (errorCode != ScriptForgeErrors.Codes.None)
            {
                GUILayout.Label(FontAwesomeIcons.WARNING, style.widgetHeaderIcon);
            }
            else if (m_IsUpToDate)
            {
                GUILayout.Label(FontAwesomeIcons.CHECKBOX, style.widgetHeaderIcon);
            }
            else
            {
                GUILayout.Label(FontAwesomeIcons.REFRESH, style.widgetHeaderIcon);
            }
        }

        /// <summary>
        /// Invoked when the Widget should create it's content.
        /// </summary>
        public override void OnGenerate(bool forced)
        {
            m_AssetHash = CreateAssetHash();
            m_IsUpToDate = true;
        }

        /// <summary>
        /// Invoked whenever we update any of our content.
        /// </summary>
        protected override void OnContentChanged()
        {
            m_IsUpToDate = string.Compare(m_AssetHash, CreateAssetHash()) == 0;
        }

        /// <summary>
        /// Draws the buttons for th bottom of the widget. By default we have three buttons. Generate, Reset, and Remove.
        /// </summary>
        /// <param name="style">The style we use to draw our butons.</param>
        protected override void DrawWidgetFooter(ScriptForgeStyles style)
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(ScriptForgeLabels.forceGenerateForgeButton, style.miniButtonLeftIcon))
                {
                    OnGenerate(true);
                }

                if (GUILayout.Button(ScriptForgeLabels.generateForgeButton, style.miniButtonMiddle))
                {
                    OnGenerate(false);
                }

                if (GUILayout.Button(ScriptForgeLabels.resetForgeButton, style.miniButtonMiddle))
                {
                    OnReset();
                }

                if (GUILayout.Button(ScriptForgeLabels.removeForgeButton, style.miniButtonRight))
                {
                    OnRemove();
                }
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Used to draw all our settings for forges.
        /// </summary>
        /// <param name="style"></param>
        protected override void DrawWidgetContent(ScriptForgeStyles style)
        {
            m_AutomaticallyGenerate = EditorGUILayout.Toggle(ScriptForgeLabels.autoBuildContent, m_AutomaticallyGenerate);

            GUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(true);
                {
                    EditorGUILayout.LabelField(ScriptForgeLabels.scriptLocation, new GUIContent(m_ScriptLocation), EditorStyles.textField);
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button(ScriptForgeLabels.changePathContent, style.changePathButton))
                {
                    string buildPath = EditorUtility.SaveFilePanelInProject("Save Location", defaultName, "cs", "Where would you like to save this class?");

                    if (!string.IsNullOrEmpty(buildPath))
                    {
                        m_ScriptLocation = buildPath;
                        ClearError(ScriptForgeErrors.Codes.Script_Location_Not_Defined);
                    }
                }

            }
            GUILayout.EndHorizontal();

            m_Namespace = EditorGUILayoutEx.NamespaceTextField(ScriptForgeLabels.namespaceContent, m_Namespace);
            m_ClassName = EditorGUILayoutEx.ClassNameTextField(ScriptForgeLabels.classNameContent, m_ClassName, defaultName);

            for (int i = 0; i < m_Components.Count; i++)
            {
                EditorGUI.BeginChangeCheck();
                {
                    m_Components[i].DrawContent(style);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    // Since components are sub objects we need to force tell them they are dirty
                    // or we will not save.
                    EditorUtility.SetDirty(m_Components[i]);
                }
            }
        }

        /// <summary>
        /// Generates a hash based on all our current assets.
        /// </summary>
        protected string CreateAssetHash()
        {
            return ComputeAssetHash(GetHashInputString());
        }

        /// <summary>
        /// Returns one string that contains all the names of all our assets to build
        /// our hash with.
		protected virtual string GetHashInputString()
		{
			string hash = string.Empty;
			for(int i = 0; i < m_Components.Count; i++)
			{
				hash = m_Components[i].AppendHashInput(hash);
			}
			return hash; 
		}

        /// <summary>
        /// Takes an input string an computes it's hash code.
        /// </summary>
        protected string ComputeAssetHash(string hashInput)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(hashInput));

                // Create a new string builder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Get our new hash.
                return sBuilder.ToString();
            }
        }

        /// <summary>
        /// Checks to see if the input string matches our last asset hash. If it does returns false saying
        /// you don't have to regenerate. If returns true the hash is stored.
        /// </summary>
        protected bool ShouldRegnerate()
        {
            // Clear any old errors.
            ClearError(ScriptForgeErrors.Codes.Script_Location_Not_Defined);

            bool shouldRegenerate = false;

            // Get our location to where we want to save this file
            string systemLocation = GetSystemSaveLocation();

            // Do we have a path defined?
            if (string.IsNullOrEmpty(systemLocation))
            {
                // Nope so we can't regenerate.
                DisplayError(ScriptForgeErrors.Codes.Script_Location_Not_Defined, "No script location has been defined for " + defaultName + " forge.");
                return false;
            }

            // If our file does not exist we can always skip the hash and force a rebuild.
            if (!File.Exists(systemLocation))
            {
                // The file is missing so we must regenerate.
                shouldRegenerate = true;
            }
            else
            {
                string hash = ComputeAssetHash(GetHashInputString());

                if (string.Compare(hash, m_AssetHash) != 0)
                {
                    shouldRegenerate = true;
                }
            }

            return shouldRegenerate;
        }

        /// <summary>
        /// Invoked when a user right clicks the widget title bar at the top.
        /// </summary>
        /// <param name="menu">The menu we want to add options too.</param>
        protected override void OnGenerateContexMenu(GenericMenu menu)
        {
            menu.AddItem(ScriptForgeLabels.generateForgeButton, false, ()=> OnGenerate(false));
            menu.AddItem(ScriptForgeLabels.forceGenerateForgeButton, false, () => OnGenerate(true));
            menu.AddItem(ScriptForgeLabels.resetForgeButton, false, OnReset);
            menu.AddItem(ScriptForgeLabels.removeForgeButton, false, OnRemove);
        }

        /// <summary>
        /// Given a system file path and a clss defintion this will write the contents
        /// to disk. If the folder does not exist one will be created for you.
        /// </summary>
        /// <param name="savePath">The system save path for the generated class.</param>
        /// <param name="classDefintion">The string content of the class.</param>
        protected void WriteToDisk(BaseTemplate template)
        {
            try
            {
                // Get our path
                string savePath = (string)template.Session["m_SaveLocation"];
                // Get our directory
                string directory = Path.GetDirectoryName(savePath);

                // Check if it exists
                if (!Directory.Exists(directory))
                {
                    // Create one if it does not.
                    Directory.CreateDirectory(directory);
                }
                // Build our text
                string classDefintion = template.TransformText();
                // Save new class to assets folder.
                File.WriteAllText(savePath, classDefintion);

                // Refresh assets.
                AssetDatabase.Refresh();
            }
            catch (System.Exception e)
            {
                Debug.LogError("An error occurred while saving file: " + e);
            }
        }

        /// <summary>
        /// Given a template this creates a new session and assigns it. 
        /// </summary>
        /// <param name="template">The template you want to load the session into.</param>
        protected void CreateSession(BaseTemplate template)
        {
            // Create the new session
            IDictionary<string, object> session = new Dictionary<string, object>();
            // Populate it
            PopulateSession(session);
            // Assign it
            template.Session = session;
            // Initialize it
            template.Initialize();
        }

        /// <summary>
        /// Invoked when a new session is created and is requesting to be filled with data. 
        /// </summary>
        /// <param name="session">The session we want to build.</param>
        protected virtual void PopulateSession(IDictionary<string, object> session)
        {
            // Create our char array for our indent.
            char[] indent = new char[m_ScriptableForge.indentCount];
            // Loop over every one and make it a space.
            for(int i = 0; i < indent.Length; i++)
            {
                indent[i] = ' ';
            }

			for(int i = 0; i < m_Components.Count; i++)
			{
				m_Components[i].PopulateSession(session); 
			}

            // Set our sessions.
            session["m_Indent"] = new string(indent);
            session["m_ClassName"] = m_ClassName;
            session["m_Namespace"] = m_Namespace;
            session["m_AssetHash"] = m_AssetHash;
            session["m_SaveLocation"] = GetSystemSaveLocation();
            session["m_IsStaticClass"] = true;
            session["m_IsPartialClass"] = false;
        }

        /// <summary>
        /// Called when the settings for this forge should be reset to default.
        /// </summary>
        public virtual void OnReset()
        {
            m_ClassName = defaultName;
            m_Namespace = string.Empty;
            m_ScriptLocation = string.Empty;
            m_AssetHash = string.Empty;
            ClearErrors();
            OnContentChanged();
        }

        /// <summary>
        /// Called when this forge should be removed.
        /// </summary>
        public virtual void OnRemove()
        {
            m_ScriptableForge.Widgets.Remove(this);
            DestroyImmediate(this);
        }

    }
}