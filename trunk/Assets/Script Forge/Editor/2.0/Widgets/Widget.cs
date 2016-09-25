﻿using System;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;


namespace ScriptForge
{
    [System.Serializable]
    public abstract class Widget : ScriptableObject, System.IComparable<Widget>
    {
        [SerializeField]
        private bool m_IsOpen;
        private AnimBool m_OpenAnimation = new AnimBool();
        protected ScriptableForge m_ScriptableForge;

        // Flashing
        private float m_FlashUntil = 0f;
        private float m_FlashStarted = 0f;
        private Color m_FlashColor = Color.white;
        private Color m_BackgroundColor = Color.white;

        // Errors
        private ScriptForgeErrors.Codes m_ErrorCode;
        private string m_ErrorMessage; 

        /// <summary>
        /// Is this widget current open?
        /// </summary>
        public bool isOpen
        {
            get { return m_IsOpen; }
            set { m_IsOpen = value; }
        }

        /// <summary>
        /// Gets the sorting order for this widget.
        /// </summary>
        public virtual int sortOrder
        {
            get
            {
                return LayoutSettings.DEFAULT_WIDGET_SORT_ORDER;
            }
        }

        public virtual GUIContent label
        {
            get
            {
                return ScriptForgeLabels.defaultWidgetTitle;
            }
        }

        public virtual string iconString
        {
            get
            {
                return FontAwesomeIcons.INFO_CIRCLE;
            }
        }

        protected void OnDisable()
        {
            if(m_ScriptableForge != null && m_OpenAnimation != null)
            {
                m_OpenAnimation.valueChanged.RemoveListener(m_ScriptableForge.Repaint);
            }

        }

        public void Initalize(ScriptableForge instance)
        {
            m_ScriptableForge = instance;
            m_BackgroundColor = Color.white;
            m_OpenAnimation.value = m_IsOpen;
            m_OpenAnimation.valueChanged.AddListener(m_ScriptableForge.Repaint);
            m_ScriptableForge.Repaint();
        }

        /// <summary>
        /// All drawing logic is placed inside of this method. 
        /// </summary>
        public virtual void OnWidgetGUI(ScriptForgeStyles style)
        {
            GUI.backgroundColor = m_BackgroundColor;
            Rect headerRect = EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(iconString, style.widgetHeaderIcon);
                    GUILayout.Label(label, style.widgetHeaderText);
                }
                GUILayout.EndHorizontal();

                headerRect.height = 30f;

                if (Event.current.type == EventType.MouseDown && headerRect.Contains(Event.current.mousePosition))
                {
                    FlashColor(Color.gray, 0.25f);
                    m_IsOpen = !m_IsOpen;
                    GUIUtility.hotControl = -1;
                    GUIUtility.keyboardControl = -1;
                }

                if (EditorGUILayout.BeginFadeGroup(m_OpenAnimation.faded))
                {
                    GUILayout.Box(GUIContent.none, style.spacer);
                    if (m_ErrorCode != ScriptForgeErrors.Codes.None)
                    {
                        EditorGUILayout.HelpBox(m_ErrorMessage, MessageType.Error);
                    }
                    DrawWidgetContent(style);
                    GUILayout.Box(GUIContent.none, style.spacer);
                    DrawWidgetFooter(style);
                }
                EditorGUILayout.EndFadeGroup();
            }
            EditorGUILayout.EndVertical();
            m_OpenAnimation.target = m_IsOpen;
            GUI.backgroundColor = Color.white;
        }

        protected virtual void DrawWidgetContent(ScriptForgeStyles style)
        {
            GUILayout.Label("The content has not been set up yet");
        }

        protected virtual void DrawWidgetFooter(ScriptForgeStyles style)
        {

        }

        /// <summary>
        /// Flashes this forge, opens it up, and displays
        /// the error message to the user. 
        /// </summary>
        protected void DisplayError(ScriptForgeErrors.Codes code, string errorMessage)
        {
            m_ErrorCode = code;
            m_ErrorMessage = errorMessage;
            FlashColor(Color.red, 1.0f);
            m_IsOpen = true;
        }

        /// <summary>
        /// Removes any active error messages.
        /// </summary>
        protected void ClearErrors(ScriptForgeErrors.Codes code)
        {
            if(code == m_ErrorCode)
            {
                m_ErrorCode = ScriptForgeErrors.Codes.None;
                m_ErrorMessage = string.Empty;
            }
        }

        public void FlashColor(Color color, float time = 2.0f)
        {
            m_FlashColor = color;
            m_FlashStarted = Time.realtimeSinceStartup;
            m_FlashUntil = m_FlashStarted + time;
            EditorApplication.update += FlashUpdate;
        }


        private void FlashUpdate()
        {
            float lerpTime = m_FlashUntil - Time.realtimeSinceStartup;
            lerpTime = Mathf.Sin(lerpTime * 20f);
            lerpTime += 1f;
            lerpTime /= 2f;
            m_BackgroundColor = Color.Lerp(Color.white, m_FlashColor, lerpTime);
            m_ScriptableForge.Repaint();
            if(m_FlashUntil < Time.realtimeSinceStartup)
            {
                EditorApplication.update -= FlashUpdate;
                m_BackgroundColor = Color.white;
            }
        }

        /// <summary>
        /// Called when this forge should generate it's content.
        /// </summary>
        public virtual void OnGenerate()
        {
            // By default does nothing.
        }

        /// <summary>
        /// Called once per frame by the editor.
        /// </summary>
        protected virtual void Update()
        {

        }

        /// <summary>
        /// Used for the Sort method on our list. 
        /// </summary>
        public int CompareTo(Widget other)
        {
            return sortOrder - other.sortOrder;
        }
    }
}
