using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;
using System.IO;

namespace ScriptForge
{
    [System.Serializable]
    public class LayersWidgetV2 : ForgeWidgetV2
    {
        /// <summary>
        /// The label that will be shown on the title of the widget.
        /// </summary>
        public override GUIContent label
        {
            get
            {
                return ScriptForgeLabels.layersWidgetTitle;
            }
        }

        /// <summary>
        /// The string icon for FontAwesome to be shown on the title bar.
        /// </summary>
        public override string iconString
        {
            get
            {
                return FontAwesomeIcons.BARS;
            }
        }

        /// <summary>
        /// Gets the sorting order for this widget.
        /// </summary>
        public override int sortOrder
        {
            get
            {
                return LayoutSettings.LAYERS_WIDGET_SORT_ORDER;
            }
        }

        /// <summary>
        /// The default name of this script
        /// </summary>
        protected override string defaultName
        {
            get
            {
                return "Layers";
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

            for (int i = 0; i < 32; i++)
            {
                string layerName = InternalEditorUtility.GetLayerName(i);
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
                LayersGenerator generator = new LayersGenerator(m_ClassName, GetSystemSaveLocation(), layers.ToArray(), m_Namespace);

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