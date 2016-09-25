using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ScriptForge
{
    public abstract class ForgeWidgetV2 : Widget
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

        /// <summary>
        /// The default name of this class
        /// </summary>
        protected abstract string defaultName { get; }

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

        protected virtual void OnEnable()
        {
            if (string.IsNullOrEmpty(m_ClassName))
            {
                m_ClassName = defaultName;
            }
        }

        protected override void DrawWidgetFooter(ScriptForgeStyles style)
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(ScriptForgeLabels.generateForgeButton, style.miniButtonLeft))
                {
                    OnGenerate();
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
                        ClearErrors(ScriptForgeErrors.Codes.Script_Location_Not_Defined);
                    }
                }

            }
            GUILayout.EndHorizontal();

            m_Namespace = EditorGUILayoutEx.NamespaceTextField(ScriptForgeLabels.namespaceContent, m_Namespace);
            m_ClassName = EditorGUILayoutEx.ClassNameTextField(ScriptForgeLabels.classNameContent, m_ClassName, defaultName);
        }

        /// <summary>
        /// Checks to see if the input string matches our last asset hash. If it does returns false saying
        /// you don't have to regenerate. If returns true the hash is stored. 
        /// </summary>
        protected bool ShouldRegnerate(string input)
        {
            // Clear any old errors.
            ClearErrors(ScriptForgeErrors.Codes.Script_Location_Not_Defined);

            bool shouldRegenerate = false;

            // Our name space should also effect our hash
            input += m_Namespace;
            // Same with the class name.
            input += m_ClassName;

            // Get our location to where we want to save this file
            string systemLocation = GetSystemSaveLocation();

            // Do we have a path defined? 
            if (string.IsNullOrEmpty(systemLocation))
            {
                // Nope so we can't regenerate. 
                DisplayError(ScriptForgeErrors.Codes.Script_Location_Not_Defined, "No build location has been defined for " + defaultName);
                return false;
            }

            // If our file does not exist we can always skip the hash and force a rebuild. 
            if (!File.Exists(systemLocation))
            {
                // The file is missing so we must regenerate. 
                shouldRegenerate = true;
            }

            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

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
                string newHash = sBuilder.ToString();

                if (string.Compare(newHash, m_AssetHash) == 0)
                {
                    // We have the same hash so we don't have to regenerate. (Unless our file is already missing). 
                    shouldRegenerate |= false;
                }
                else
                {
                    // Save our new hash and force a regeneration.
                    m_AssetHash = newHash; 
                    shouldRegenerate = true;
                    // Hash changed so lets save.
                    ScriptableForge.SaveInstance();
                }
            }

            if(shouldRegenerate)
            {
                FlashColor(Color.green, 1.0f);
            }

            return shouldRegenerate;
        }

        /// <summary>
        /// Called when the settings for this forge should be reset to default. 
        /// </summary>
        public abstract void OnReset();

        /// <summary>
        /// Called when this forge should be removed.
        /// </summary>
        public abstract void OnRemove();
    }
}