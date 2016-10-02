using UnityEngine;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Reflection;
using Type = System.Type;
using UnityEditor;
using System.IO;

namespace ScriptForge
{
    public class SortingLayersWidget : ForgeWidget
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
        /// Returns an array of all the valid layer names. 
        /// </summary>
        /// <returns></returns>
        private string[] GetValidSortingLayerNames()
        {
            List<string> validSortingLayers = new List<string>();
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
                    validSortingLayers.Add(layerName);
                }
            }
            return validSortingLayers.ToArray();
        }

        /// <summary>
        /// Returns one string that contains all the names of all our assets to build
        /// our hash with.
        protected override string GetHashInputString()
        {
            string hashInput = string.Empty;
            hashInput += m_Namespace;
            hashInput += m_ClassName;
            foreach (var layer in GetValidSortingLayerNames())
            {
                hashInput += layer;
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
                string[] sortingLayerNames = GetValidSortingLayerNames();
                string savePath = GetSystemSaveLocation();

                // Build the generator with the class name and data source.
                TagsGenerator generator = new TagsGenerator(m_ClassName, sortingLayerNames, m_Namespace);

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