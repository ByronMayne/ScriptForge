﻿using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ScriptForge
{
    public class SettingsWidgetV2 : Widget
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
            GUILayout.Label("Tags");
        }


        protected override void DrawWidgetFooter(ScriptForgeStyles style)
        {

        }
	}
}