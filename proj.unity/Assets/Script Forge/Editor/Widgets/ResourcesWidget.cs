using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using System;

namespace ScriptForge
{
    [System.Serializable]
    public class ResourcesWidget : ForgeWidget
    {
        private const float BUTTON_WIDTH = 40;
        private GUIContent m_HeaderLabel = new GUIContent("Included Resource Folders");

        [SerializeField]
        protected List<string> m_FoldersToInclude;

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

        protected override void OnEnable()
        {
            base.OnEnable();
            // Create our list. 
            m_SerilaizedList = new ReorderableList(m_FoldersToInclude, typeof(string));
            m_SerilaizedList.drawElementCallback += OnDrawElement;
            m_SerilaizedList.drawHeaderCallback += OnDrawHeader;
            m_SerilaizedList.onAddCallback += OnFolderAdded;
        }


        /// <summary>
        /// Invoked when we are drawing our header.
        /// </summary>
        private void OnDrawHeader(Rect rect)
        {
            GUI.Label(rect, m_HeaderLabel);
        }

        /// <summary>
        /// Invoked when we need to draw our element 
        /// </summary>
        private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            Rect contentRect = rect;
            contentRect.width -= BUTTON_WIDTH;
            GUI.Label(contentRect, m_FoldersToInclude[index]); 
        }

        /// <summary>
        /// Invoked to allow us to draw are GUI content for this forge.
        /// </summary>
        protected override void DrawWidgetContent(ScriptForgeStyles style)
        {
            base.DrawWidgetContent(style);
            m_SerilaizedList.DoLayoutList();
        }

        /// <summary>
        /// Invoked when we add a new element to our list. 
        /// </summary>
        private void OnFolderAdded(ReorderableList list)
        {
            // Get the path 
            string path = EditorUtility.OpenFolderPanel("Folder", Application.dataPath, "Resources");
            // Convert it to a unity path
            path = FileUtil.GetProjectRelativePath(path);
            // Add it
            m_FoldersToInclude.Add(path);
        }

        protected string[] GetResourceAssetGUIDs()
        {
            return AssetDatabase.FindAssets("", m_FoldersToInclude.ToArray());
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
                    Debug.Log("Adding: " + assetPath);
                }
            }
        }
    }
}