﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace ScriptForge
{
    [System.Serializable]
    public class TagsWidgetV2 : ForgeWidgetV2
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
        /// Invoked when this widget should generate it's content. 
        /// </summary>
        public override void OnGenerate()
        {
            string hashInput = string.Empty;
            List<string> validTags = new List<string>();
            string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

            for (int i = 0; i < tags.Length; i++)
            {
                string tag = tags[i];
                tag = tag.Replace(' ', '_');

                if (!string.IsNullOrEmpty(tag))
                {
                    validTags.Add(tag);
                    hashInput += tag;
                }
            }

            if (ShouldRegnerate(hashInput))
            {
                // Build the generator with the class name and data source.
                TagsGenerator generator = new TagsGenerator(m_ClassName, validTags.ToArray(), m_Namespace);

                // Generate output (class definition).
                var classDefintion = generator.TransformText();

                try
                {
                    // Save new class to assets folder.
                    File.WriteAllText(GetSystemSaveLocation(), classDefintion);

                    // Refresh assets.
                    AssetDatabase.Refresh();
                }
                catch (System.Exception e)
                {
                    Debug.Log("An error occurred while saving file: " + e);
                }
            }
        }

        /// <summary>
        /// Invoked when this forge should be reset to the default values. 
        /// </summary>
        public override void OnReset()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Invoked when this forge should be removed. 
        /// </summary>
        public override void OnRemove()
        {
            throw new System.NotImplementedException();
        }
    }
}