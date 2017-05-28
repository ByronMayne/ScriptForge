using UnityEngine;
using UnityEditor;
using System;

namespace ScriptForge
{
    [RequiredWidget, Serializable]
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
            GUILayout.BeginHorizontal();
            {

                if (GUILayout.Button(ScriptForgeLabels.documentationButtonLabel, style.fontAwesomeButton))
                {
                    ExtenalLinks.OpenDocumentationPage();
                }

                if(GUILayout.Button(ScriptForgeLabels.issuesButtonLabel, style.fontAwesomeButton))
                {
                    ExtenalLinks.OpenIssuesPage();
                }

                if (GUILayout.Button(ScriptForgeLabels.repoButtonLabel, style.fontAwesomeButton))
                {
                    ExtenalLinks.OpenRepoPage();
                }

                if (GUILayout.Button(ScriptForgeLabels.twitterButtonLabel, style.fontAwesomeButton))
                {
                    ExtenalLinks.OpenTwitter();
                }
            }
            GUILayout.EndHorizontal();
        }


    }
}