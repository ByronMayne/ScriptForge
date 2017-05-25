﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;
using System.IO;

namespace ScriptForge
{
    [System.Serializable]
    public class LayersWidget : ForgeWidget
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
        /// Returns an array of all the layer names, including unset ones.
        /// </summary>
        /// <returns></returns>
        private string[] GetLayerNames()
        {
            List<string> layers = new List<string>();

            for (int i = 0; i < 32; i++)
            {
                string layerName = InternalEditorUtility.GetLayerName(i);
                layerName = layerName.Replace(' ', '_');

                layers.Add(layerName);
            }
            return layers.ToArray();
        }

        /// <summary>
        /// Returns one string that contains all the names of all our assets to build
        /// our hash with.
        protected override string GetHashInputString()
        {
            string hashInput = string.Empty;
            hashInput += m_Namespace;
            hashInput += m_ClassName;
            foreach(var layer in GetLayerNames())
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
                string[] validLayerNames = GetLayerNames();
                string savePath = GetSystemSaveLocation();

                // Build the generator with the class name and data source.
                LayersGenerator generator = new LayersGenerator(m_ClassName, savePath, validLayerNames, m_Namespace);

                // Generate output (class definition).
                var classDefintion = generator.TransformText();

                // Write our class to disk.
                WriteToDisk(savePath, classDefintion);
            }
            base.OnGenerate();
        }
    }
}