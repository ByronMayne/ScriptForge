using UnityEngine;
using UnityEditor;
using System;

namespace ScriptForge
{
    [RequiredWidget, Serializable]
    public class SettingsWidget : Widget
    {
        public const int MIN_INDENT_SIZE = 0;
        public const int MAX_INDENT_SIZE = 10;

        public override GUIContent label
        {
            get
            {
                return ScriptForgeLabels.settingsWidgetTitle;
            }
        }

        public override string iconString
        {
            get
            {
                return FontAwesomeIcons.COG;
            }
        }

        /// <summary>
        /// Gets the sorting order for this widget.
        /// </summary>
        public override int sortOrder
        {
            get
            {
                return LayoutSettings.SETTINGS_WIDGET_SORT_ORDER;
            }
        }

        protected override void DrawWidgetContent(ScriptForgeStyles style)
        {
            m_ScriptableForge.animateWidgets = EditorGUILayout.Toggle(ScriptForgeLabels.animateWidgetsContent, m_ScriptableForge.animateWidgets);

            EditorGUI.BeginChangeCheck();
            {
                m_ScriptableForge.indentCount = EditorGUILayout.IntSlider(ScriptForgeLabels.indentCountLabel, m_ScriptableForge.indentCount, MIN_INDENT_SIZE, MAX_INDENT_SIZE);
            }
            if(EditorGUI.EndChangeCheck())
            {
                // Force users to give a valid input. They could type in a negative number which would throw an exception in the generation process.
                m_ScriptableForge.indentCount = Mathf.Clamp(m_ScriptableForge.indentCount, MIN_INDENT_SIZE, MAX_INDENT_SIZE);
            }
        }


        protected override void DrawWidgetFooter(ScriptForgeStyles style)
        {

        }
    }
}