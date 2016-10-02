using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ScriptForge
{
    [System.Serializable]
    public class AboutWidget : Widget
    {
        public override GUIContent label
        {
            get
            {
                return ScriptForgeLabels.aboutWidgetTitle;
            }
        }

        /// <summary>
        /// Gets the sorting order for this widget.
        /// </summary>
        public override int sortOrder
        {
            get
            {
                return LayoutSettings.ABOUT_WIDGET_SORT_ORDER;
            }
        }

        protected override void DrawWidgetContent(ScriptForgeStyles style)
        {
            EditorGUILayout.LabelField(ScriptForgeLabels.aboutWidgetContent, EditorStyles.wordWrappedLabel);
        }


        protected override void DrawWidgetFooter(ScriptForgeStyles style)
        {
            if (GUILayout.Button("Documentation", style.button))
            {
                System.Diagnostics.Process.Start(ExtenalLinks.SCRIPT_FORGE_GOOLGE_DOC_URL);
            }
        }
	}
}