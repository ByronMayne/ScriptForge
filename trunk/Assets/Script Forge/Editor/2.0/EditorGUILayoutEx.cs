using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ScriptForge
{
    public static class EditorGUILayoutEx
    {
        /// <summary>
        /// Creates a text field that a user can modify but when input is confirmed the name
        /// is validated to make sure it does not have any invalid characters. 
        /// </summary>
        public static string ClassNameTextField(GUIContent label, string scriptName, string defaultIfNullOrEmpty)
        {
            EditorGUI.BeginChangeCheck();
            {
                scriptName = EditorGUILayout.DelayedTextField(label, scriptName);
            }
            if(EditorGUI.EndChangeCheck())
            {
                scriptName = Regex.Replace(scriptName, "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled);

                if(string.IsNullOrEmpty(scriptName))
                {
                    scriptName = defaultIfNullOrEmpty;
                }
            }
            return scriptName; 
        }

        /// <summary>
        /// Creates a text field that a user can modify but when input is confirmed the name
        /// is validated to make sure it does not have any invalid characters this is setup for name spaces. 
        /// </summary>
        public static string NamespaceTextField(GUIContent label, string @namespace)
        {
            EditorGUI.BeginChangeCheck();
            {
                @namespace = EditorGUILayout.DelayedTextField(label, @namespace);
            }
            if (EditorGUI.EndChangeCheck())
            {
                // Remove any invalid characters. 
                @namespace = Regex.Replace(@namespace, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
                // Remove sequential periods, two or more are replaced with one.
                @namespace = Regex.Replace(@namespace, "[.{2,}]+", ".", RegexOptions.Compiled);
            }
            return @namespace;
        }
    }
}
