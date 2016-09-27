using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace ScriptForge
{
    [System.Serializable]
    public class ScenesWidget : ForgeWidget
    {
        [SerializeField]
        private string m_GeneratorHash = string.Empty;
        [SerializeField]
        private string m_EnumName = "Types";

        public override GUIContent label
        {
            get
            {
                return ScriptForgeLabels.scenesWidgetTitle;
            }
        }

        public override string iconString
        {
            get
            {
                return FontAwesomeIcons.PICTURE;
            }
        }

        /// <summary>
        /// Gets the sorting order for this widget.
        /// </summary>
        public override int sortOrder
        {
            get
            {
                return LayoutSettings.SCENES_WIDGET_SORT_ORDER;
            }
        }

        /// <summary>
        /// The default name of this script
        /// </summary>
        protected override string defaultName
        {
            get
            {
                return "Scenes";
            }
        }

        protected override void DrawWidgetContent(ScriptForgeStyles style)
        {
            base.DrawWidgetContent(style);
            m_EnumName = EditorGUILayoutEx.ClassNameTextField(ScriptForgeLabels.enumNameContent, m_EnumName, "Types");
        }

        /// <summary>
        /// Returns a list of all the valid scenes that we want to include in our generated class.
        /// </summary>
        /// <returns></returns>
        private string[] GetValidSceneNames()
        {
            List<string> validScenes = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                // Make sure the scene exists on disk (this can happen if it was deleted)
                Object loadedScene = AssetDatabase.LoadAssetAtPath<Object>(scene.path);

                if (loadedScene != null)
                {
                    string sceneName = scene.path.Substring(scene.path.LastIndexOf('/') + 1);
                    sceneName = sceneName.Replace(".unity", "");
                    validScenes.Add(sceneName);
                }
            }
            return validScenes.ToArray();
        }

        /// <summary>
        /// Returns one string that contains all the names of all our assets to build
        /// our hash with.
        protected override string GetHashInputString()
        {
            string hashInput = string.Empty;

            hashInput += m_Namespace;
            hashInput += m_ClassName;
            hashInput += m_EnumName;

            foreach(var scene in GetValidSceneNames())
            {
                hashInput += scene;
            }

            return hashInput;
        }


        /// <summary>
        /// Invoked when this widget should generate it's content. 
        /// </summary>
        public override void OnGenerate()
        {
            if (ShouldRegnerate())
            {
                string[] sceneNames = GetValidSceneNames();
                string savePath = GetSystemSaveLocation();

                // Build the generator with the class name and data source.
                ScenesGenerator generator = new ScenesGenerator(m_ClassName, savePath, sceneNames, m_EnumName, m_Namespace);

                // Generate output (class definition).
                var classDefintion = generator.TransformText();
                try
                {
                    // Save new class to assets folder.
                    File.WriteAllText(savePath, classDefintion);

                    // Refresh assets.
                    AssetDatabase.Refresh();
                }
                catch (System.Exception e)
                {
                    Debug.Log("An error occurred while saving file: " + e);
                }
            }
            base.OnGenerate();
        }

        /// <summary>
        /// Invoked when this forge should be reset to the default values. 
        /// </summary>
        public override void OnReset()
        {
            m_EnumName = "Types";
        }
    }
}