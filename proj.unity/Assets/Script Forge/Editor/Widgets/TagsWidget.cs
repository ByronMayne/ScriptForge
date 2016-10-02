using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace ScriptForge
{
    [System.Serializable]
    public class TagsWidget : ForgeWidget
    {
        public override GUIContent label
        {
            get
            {
                return ScriptForgeLabels.tagsWidgetTitle;
            }
        }

        public override string iconString
        {
            get
            {
                return FontAwesomeIcons.BOOKMARK;
            }
        }

        /// <summary>
        /// Gets the sorting order for this widget.
        /// </summary>
        public override int sortOrder
        {
            get
            {
                return LayoutSettings.TAG_WIDGET_SORT_ORDER;
            }
        }

        /// <summary>
        /// The default name of this script
        /// </summary>
        protected override string defaultName
        {
            get
            {
                return "Tags";
            }
        }

        /// <summary>
        /// Invoked to allow us to draw are GUI content for this forge.
        /// </summary>
        protected override void DrawWidgetContent(ScriptForgeStyles style)
        {
            base.DrawWidgetContent(style);
        }

        /// <summary>
        /// Returns a list of all the valid tags that we want to include in our generated class.
        /// </summary>
        /// <returns></returns>
        private string[] GetValidTagNames()
        {
            List<string> validTags = new List<string>();
            string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

            for (int i = 0; i < tags.Length; i++)
            {
                string tag = tags[i];
                tag = tag.Replace(' ', '_');

                if (!string.IsNullOrEmpty(tag))
                {
                    validTags.Add(tag);
                }
            }
            return validTags.ToArray();
        }

        /// <summary>
        /// Returns one string that contains all the names of all our assets to build
        /// our hash with.
        protected override string GetHashInputString()
        {
            string hashInput = string.Empty;

            hashInput += m_Namespace;
            hashInput += m_ClassName;

            foreach (var tag in GetValidTagNames())
            {
                hashInput += tag;
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
                string[] validTags = GetValidTagNames();
                string savePath = GetSystemSaveLocation();

                // Build the generator with the class name and data source.
                TagsGenerator generator = new TagsGenerator(m_ClassName, validTags, m_Namespace);

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
    }
}