using System;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;


namespace ScriptForge
{
    [System.Serializable]
    public class Widget : ScriptableObject, System.IComparable<Widget>
    {
        [SerializeField]
        private bool m_IsOpen;
        private AnimBool m_OpenAnimation;

        // Flashing
        private float m_FlashUntil;
        private float m_FlashStarted;
        private Color m_FlashColor = Color.gray;
        private Color m_BackgroundColor;

        // Errors
        private bool m_HasError;
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

        private void OnEnable()
        {
            m_BackgroundColor = Color.white;
            m_OpenAnimation = new AnimBool(m_IsOpen);
            m_OpenAnimation.valueChanged.AddListener(ScriptableForge.instance.Repaint);

        }

        private void OnDisable()
        {
            m_OpenAnimation.valueChanged.RemoveListener(ScriptableForge.instance.Repaint);
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
                }

                if (EditorGUILayout.BeginFadeGroup(m_OpenAnimation.faded))
                {
                    GUILayout.Box(GUIContent.none, style.spacer);
                    if (m_HasError)
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
        protected void DisplayError(string errorMessage)
        {
            m_HasError = true;
            m_ErrorMessage = errorMessage;
            FlashColor(Color.red, 1.0f);
            m_IsOpen = true;
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
            ScriptableForge.instance.Repaint();
            if(m_FlashUntil < Time.realtimeSinceStartup)
            {
                EditorApplication.update -= FlashUpdate;
                m_BackgroundColor = Color.white;
            }
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
