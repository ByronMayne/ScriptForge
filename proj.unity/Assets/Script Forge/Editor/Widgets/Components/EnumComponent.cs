using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ScriptForge.Widgets.Components
{
    [Serializable]
    public class EnumComponent : ForgeComponent
    {
        [SerializeField]
        protected bool m_CreateEnum;

        [SerializeField]
        protected string m_EnumName;

        /// <summary>
        /// Allows this component to add data to the session. 
        /// </summary>
        /// <param name="session">Session.</param>
        public override void PopulateSession(IDictionary<string, object> session)
        {
            session["m_CreateEnum"] = m_CreateEnum;
            session["m_EnumName"] = m_EnumName;
        }

        /// <summary>
        /// Invoked when we are creating a hash for the parent Widget.
        /// </summary>
        /// <returns>The appended hash input.</returns>
        public override string AppendHashInput(string hashInput)
        {
            hashInput += m_CreateEnum.ToString();
            hashInput += m_EnumName;
            return hashInput;
        }

        /// <summary>
        /// Invoked in the Layout of each widget that has this component.
        /// </summary>
        /// <param name="style"></param>
        public override void DrawContent(ScriptForgeStyles style)
        {
            m_CreateEnum = EditorGUILayout.Toggle("Create Enum", m_CreateEnum);
            EditorGUI.BeginDisabledGroup(!m_CreateEnum);
            {
                m_EnumName = EditorGUILayoutEx.ClassNameTextField(ScriptForgeLabels.enumNameContent, m_EnumName, "Types");
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}
