using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using System;

namespace ScriptForge
{
    [Serializable, InDevelopment]
    public class ResourcesWidget : FolderFilterWidget
    {
        private const string RESOURCES_FOLDER_NAME = "/Resources";
        private GUIContent m_HeaderLabel = new GUIContent("Included Resource Folders");

        protected ReorderableList m_SerilaizedList;

        public override GUIContent label
        {
            get
            {
                return ScriptForgeLabels.resourcesWidgetTitle; ;
            }
        }

        public override string iconString
        {
            get
            {
                return FontAwesomeIcons.PUZZLE_PIECE;
            }
        }

        /// <summary>
        /// Gets the sorting order for this widget.
        /// </summary>
        public override int sortOrder
        {
            get
            {
                return LayoutSettings.RESOURCES_WIDGET_SORT_ORDER;
            }
        }

        /// <summary>
        /// The default name of this script
        /// </summary>
        protected override string defaultName
        {
            get
            {
                return "ResourcePaths";
            }
        }

        /// <summary>
        /// Invoked when we are drawing our header.
        /// </summary>
        private void OnDrawHeader(Rect rect)
        {
            GUI.Label(rect, m_HeaderLabel);
        }

        /// <summary>
        /// Returns back our complete list of assets inside our selected folders. 
        /// </summary>
        /// <returns></returns>
        protected string[] GetResourceAssetGUIDs()
        {
            return AssetDatabase.FindAssets("", folders.ToArray());
        }


        /// <summary>
        /// When a new folder is added this will be invoked. If it returns true it will
        /// be added and if false it will not be added.
        /// </summary>
        /// <param name="assetPath">The asset path to the folder being added.</param>
        protected override bool IsValidFolder(string assetPath)
        {
            // Make sure it's a resources path
            int resourcesIndex = assetPath.LastIndexOf(RESOURCES_FOLDER_NAME + "/");
            // Get our end index
            // Check to see if the index is great then -1
            if (resourcesIndex >= 0 || assetPath.EndsWith(RESOURCES_FOLDER_NAME))
            {
                if (!folders.Contains(assetPath))
                {
                    return true;
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Invalid Resource Path", "The path '" + assetPath + "' does not contain a resources folder. Please try again", "Okay");
            }
            // It's not a valid folder
            return false;
        }

        /// <summary>
        /// Invoked when this widget should generate it's content. 
        /// </summary>
        public override void OnGenerate()
        {
            string savePath = GetSystemSaveLocation();
            List<string> result = new List<string>();
            foreach(string guid in GetResourceAssetGUIDs())
            {
                // Convert to our asset path
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                // Add it 
                if(!result.Contains(assetPath))
                {
                    result.Add(assetPath);
                }
            }

            // Create our session data
            Dictionary<string, object> session = new Dictionary<string, object>();
            session["m_ClassName"] = m_ClassName;
            session["m_Namespace"] = m_Namespace;
            session["m_IsStaticClass"] = true;
            session["m_IsPartialClass"] = false;
            session["m_SaveLocation"] = savePath;
            session["m_ResourcePaths"] = result.ToArray();
            session["m_Indent"] = "    ";
            session["m_AssetHash"] = m_AssetHash;
            session["m_IsEnumDefinedInClass"] = false;

            // Build the template
            ResourcesTemplate generator = new ResourcesTemplate();
            // set the session
            generator.Session = session;

            // Initialize our template to load the values
            generator.Initialize();

            // Generate output (class definition).
            var classDefintion = generator.TransformText();

            WriteToDisk(savePath, classDefintion);

            base.OnGenerate();
        }
    }
}