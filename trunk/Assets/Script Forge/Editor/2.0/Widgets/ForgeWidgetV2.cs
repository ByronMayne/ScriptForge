using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ScriptForge
{
    public abstract class ForgeWidgetV2 : Widget
    {
        [SerializeField]
        protected bool m_AutomaticallyGenerate = false;

        [SerializeField]
        protected string m_BuildPath = string.Empty;

        [SerializeField]
        protected string m_Namespace = "ScriptForge";

        protected override void DrawWidgetFooter(ScriptForgeStyles style)
        {
            GUILayout.BeginHorizontal();
            {
                if(GUILayout.Button(ScriptForgeLabels.generateForgeButton, style.miniButtonLeft))
                {
                    OnGenerate();
                }

                if (GUILayout.Button(ScriptForgeLabels.resetForgeButton, style.miniButtonMiddle))
                {
                    OnReset();
                }

                if (GUILayout.Button(ScriptForgeLabels.removeForgeButton, style.miniButtonRight))
                {
                    OnRemove();
                }
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Used to draw all our settings for forges.
        /// </summary>
        /// <param name="style"></param>
        protected override void DrawWidgetContent(ScriptForgeStyles style)
        {
            m_AutomaticallyGenerate = EditorGUILayout.Toggle(ScriptForgeLabels.autoBuildContent, m_AutomaticallyGenerate);

            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(ScriptForgeLabels.buildPathContent, new GUIContent(m_BuildPath));
                if(GUILayout.Button(ScriptForgeLabels.changePathContent, style.changePathButton))
                {

                }

            }
            GUILayout.EndHorizontal();

            m_Namespace = EditorGUILayout.TextField(ScriptForgeLabels.namespaceContent, m_Namespace);
        }

        /// <summary>
        /// Called when this forge should generate it's content.
        /// </summary>
        public abstract void OnGenerate();

        /// <summary>
        /// Called when the settings for this forge should be reset to default. 
        /// </summary>
        public abstract void OnReset();

        /// <summary>
        /// Called when this forge should be removed.
        /// </summary>
        public abstract void OnRemove();
	}
}