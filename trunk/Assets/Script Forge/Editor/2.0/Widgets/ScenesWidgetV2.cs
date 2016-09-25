using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ScriptForge
{
    public class ScenesWidgetV2 : ForgeWidgetV2
    {
        [SerializeField]
        private string m_GeneratorHash = string.Empty;

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