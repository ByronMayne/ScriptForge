using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;

namespace ScriptForge
{
    public class ScenesWidgetV2 : ForgeWidgetV2
    {
        [SerializeField]
        private string m_GeneratorHash = string.Empty;
        [SerializeField]
        private string m_EnumName = "Types";

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
                return LayoutSettings.SCENES_WIDGET_SORT_ORDER ;
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

        protected override void DrawWidgetContent(ScriptForgeStyles style)
        {
            base.DrawWidgetContent(style);
            m_EnumName = EditorGUILayoutEx.ClassNameTextField(ScriptForgeLabels.enumNameContent, m_EnumName, "Types");
        }

        /// <summary>
        /// Invoked when this widget should generate it's content. 
        /// </summary>
        public override void OnGenerate()
        {
            string hashInput = string.Empty;
            List<string> sceneNames = new List<string>();
            foreach( var scene in EditorBuildSettings.scenes )
            {
                string sceneName = scene.path.Substring(scene.path.LastIndexOf('/') + 1);
                sceneName = sceneName.Replace(".unity", "");
                sceneNames.Add(sceneName);
                hashInput += sceneName;
            }

            if(ShouldRegnerate(hashInput))
            {
                // Build the generator with the class name and data source.
                ScenesGenerator generator = new ScenesGenerator(m_ClassName, GetSystemSaveLocation(), sceneNames.ToArray(), m_EnumName, m_Namespace);

                // Generate output (class definition).
                var classDefintion = generator.TransformText();
                try
                {
                    // Save new class to assets folder.
                    File.WriteAllText(GetSystemSaveLocation(), classDefintion);

                    // Refresh assets.
                    AssetDatabase.Refresh();
                }
                catch (Exception e)
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