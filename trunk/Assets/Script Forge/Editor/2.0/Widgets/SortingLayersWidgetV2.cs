using UnityEngine;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Reflection;
using Type = System.Type;
using UnityEditor;
using System.IO;

namespace ScriptForge
{
    public class SortingLayersWidgetV2 : ForgeWidgetV2
    {
        public override GUIContent label
        {
            get
            {
                return ScriptForgeLabels.sortingLayersTitle;
            }
        }

        public override string iconString
        {
            get
            {
                return FontAwesomeIcons.SORT_AMOUNT;
            }
        }

        /// <summary>
        /// Gets the sorting order for this widget.
        /// </summary>
        public override int sortOrder
        {
            get
            {
                return LayoutSettings.SORTING_LAYERS_WIDGET_SORT_ORDER;
            }
        }

        /// <summary>
        /// The default name of this script
        /// </summary>
        protected override string defaultName
        {
            get
            {
                return "SortingLayers";
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
            List<string> layers = new List<string>();
            // Sorting layers is hidden so we have to use reflection. 
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            // Grab our static property. 
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            // Get the string values.
            string[] sortingLayers = (string[])sortingLayersProperty.GetValue(null, new object[0]);

            // Loop over everyone and make sure they are not null or empty. 
            for (int i = 0; i < sortingLayers.Length; i++)
            {
                string layerName = sortingLayers[i];
                layerName = layerName.Replace(' ', '_');

                if (!string.IsNullOrEmpty(layerName))
                {
                    layers.Add(layerName);
                    hashInput += layerName;
                }
            }

            if (ShouldRegnerate(hashInput))
            {
                // Build the generator with the class name and data source.
                TagsGenerator generator = new TagsGenerator(m_ClassName, layers.ToArray(), m_Namespace);

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