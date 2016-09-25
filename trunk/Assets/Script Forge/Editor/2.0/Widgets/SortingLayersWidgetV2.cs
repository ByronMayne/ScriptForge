﻿using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ScriptForge
{
    public class SortingLayersWidgetV2 : ForgeWidgetV2
    {
        public override GUIContent label
        {
            get
            {
                return ScriptForgeLabels.sortingLayersTitle;
            }
        }

        public override string iconString
        {
            get
            {
                return FontAwesomeIcons.SORT_AMOUNT;
            }
        }

        /// <summary>
        /// Gets the sorting order for this widget.
        /// </summary>
        public override int sortOrder
        {
            get
            {
                return LayoutSettings.SORTING_LAYERS_WIDGET_SORT_ORDER;
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
            throw new System.NotImplementedException();
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