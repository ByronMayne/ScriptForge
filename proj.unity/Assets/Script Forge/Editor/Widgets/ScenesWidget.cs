using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace ScriptForge
{
    [System.Serializable]
    public class ScenesWidget : ForgeWidget
    {
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
                // Invoke the base.
                base.OnGenerate();
                // Build the template
                ScenesTemplate generator = new ScenesTemplate();
                // TODO: Fix this. 
                // Populate it's session
                CreateSession(generator);
                // Write it to disk. 
                WriteToDisk(generator);
            }
            base.OnGenerate();
        }

        /// <summary>
        /// Invoked when this forge should be reset to the default values.
        /// </summary>
        public override void OnReset()
        {
            m_EnumName = "SceneTypes";
            m_CreateEnum = true;
        }

        /// <summary>
        /// Used to send our paths for our session.
        /// </summary>
        /// <param name="session"></param>
        protected override void PopulateSession(IDictionary<string, object> session)
        {
            // Create our base session 
            base.PopulateSession(session);
            // Get our layers
            string[] sceneNames = GetValidSceneNames();
            // Set our session
            session["m_Scenes"] = sceneNames;
        }
    }
}