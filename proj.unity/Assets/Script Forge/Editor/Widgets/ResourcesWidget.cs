using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using System;

namespace ScriptForge
{
    [System.Serializable]
	public class ResourcesWidget : FolderFilterWidget
    {
        private GUIContent m_HeaderLabel = new GUIContent("Included Resource Folders");

        protected ReorderableList m_SerilaizedList;

        public override GUIContent label
        {
            get
            {
                return new GUIContent("Resources");
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
        /// Returns one string that contains all the names of all our assets to build
        /// our hash with.
        protected override string GetHashInputString()
        {
            string hashInput = string.Empty;
            hashInput += m_Namespace;
            hashInput += m_ClassName;
            foreach (var assetPath in GetResourceAssetGUIDs())
            {
                hashInput += assetPath;
            }
            return hashInput;
        }

        /// <summary>
        /// Invoked when this widget should generate it's content. 
        /// </summary>
        public override void OnGenerate()
        {
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
        }
    }
}