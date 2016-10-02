using UnityEngine;
using UnityEditor;
using System;

namespace ScriptForge
{
    [RequiredWidget, Serializable]
    public class SettingsWidget : Widget
    {
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
        }


        protected override void DrawWidgetFooter(ScriptForgeStyles style)
        {

        }
    }
}