using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Reflection;
using Type = System.Type;

namespace ScriptForge
{
    [CustomEditor(typeof(ScriptableForge))]
    public class ScriptableForgeEditor : Editor
    {

        private bool m_AddForgeButtonSelected;
        private ScriptableForge m_Target;
        private ScriptForgeStyles m_Styles; 

        private void OnEnable()
        {
            m_Target = target as ScriptableForge;
        }

        private void OnDisable()
        {
        }
        

        /// <summary>
        /// Draws the header for our instance.
        /// </summary>
        protected override void OnHeaderGUI()
        {
            if (m_Styles == null)
            {
                m_Styles = new ScriptForgeStyles();
            }

            GUILayout.BeginHorizontal(GUI.skin.box);
            {
                GUILayout.Label(FontAwesomeIcons.CUBES, m_Styles.titleBarIcon);
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                {
                    GUILayout.Label(ScriptForgeLabels.HEADER_TITLE, m_Styles.title);
                    GUILayout.Label(ScriptForgeLabels.HEADER_SUB_TITLE, m_Styles.subTitle);
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.Label(FontAwesomeIcons.CUBES, m_Styles.titleBarIcon);
            }
            GUILayout.EndHorizontal();
        }
        public override void OnInspectorGUI()
        {
            DrawButtons();

            if(GUILayout.Button("Save"))
            {
                ScriptableForge.SaveInstance();
            }
        }

        /// <summary>
        /// Draws the header for script forge.
        /// </summary>
        private void DrawButtons()
        {
            if (GUILayout.Button(ScriptForgeLabels.generateAllForgesLabel, m_Styles.button))
            {
                for(int i = 0; i < m_Target.Widgets.Count; i++)
                {
                    m_Target.Widgets[i].OnGenerate();
                }
            }
            EditorGUILayout.BeginHorizontal();
            {
                DrawAddForgeButton();

                if (GUILayout.Button(ScriptForgeLabels.setCommonPathLabel, m_Styles.buttonMiddle))
                {

                }

                if (GUILayout.Button(ScriptForgeLabels.setCommonPathLabel, m_Styles.buttonMiddle))
                {

                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(ScriptForgeLabels.openAllWidgetsLabel, m_Styles.miniButtonMiddle))
                {
                    for (int i = 0; i < m_Target.Widgets.Count; i++)
                    {
                        m_Target.Widgets[i].isOpen = true;
                    }
                }

                if (GUILayout.Button(ScriptForgeLabels.closeAllWidgetsLabel, m_Styles.miniButtonMiddle))
                {
                    for (int i = 0; i < m_Target.Widgets.Count; i++)
                    {
                        m_Target.Widgets[i].isOpen = false;
                    }
                }
            }
            GUILayout.EndHorizontal();


            GUILayout.Space(5.0f);
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, m_Styles.spacer);
            GUI.Label(rect, GUIContent.none, m_Styles.spacer);
            GUI.Label(rect, GUIContent.none, m_Styles.spacer);
            GUI.Label(rect, GUIContent.none, m_Styles.spacer);
            GUILayout.Space(5.0f);
            for (int i = 0; i < m_Target.Widgets.Count; i++)
            {
                m_Target.Widgets[i].OnWidgetGUI(m_Styles);

                GUILayout.Space(5.0f);
                rect = GUILayoutUtility.GetRect(GUIContent.none, m_Styles.spacer);
                GUI.Label(rect, GUIContent.none, m_Styles.spacer);
                GUI.Label(rect, GUIContent.none, m_Styles.spacer);
                GUI.Label(rect, GUIContent.none, m_Styles.spacer);
                GUILayout.Space(5.0f);
            }
        }

        /// <summary>
        /// Default buttons can't handle generic menus as a result so we have to make our own buttons. Below
        /// this is how it's done. 
        /// </summary>
        private void DrawAddForgeButton()
        {
            Event current = Event.current;
            Rect buttonRect = GUILayoutUtility.GetRect(ScriptForgeLabels.addForgeLabel, m_Styles.buttonLeft);
            int controlID = GUIUtility.GetControlID(ScriptForgeLabels.addForgeLabel, FocusType.Keyboard, buttonRect);

            if (current.type == EventType.MouseDown && buttonRect.Contains(current.mousePosition))
            {
                m_AddForgeButtonSelected = true;
                GUIUtility.hotControl = controlID;
                current.Use();
                Repaint();

                GenericMenu menu = new GenericMenu();

                Assembly assembly = Assembly.GetCallingAssembly();
                Type[] types = assembly.GetTypes();

                for (int i = 0; i < types.Length; i++)
                {
                    if (!types[i].IsAbstract && typeof(Widget).IsAssignableFrom(types[i]))
                    {
                        bool hasInstance = false;

                        for (int x = 0; x < m_Target.Widgets.Count; x++)
                        {
                            if (m_Target.Widgets[x].GetType() == types[i])
                            {
                                hasInstance = true;
                                break;
                            }
                        }
                        if (hasInstance)
                        {
                            menu.AddDisabledItem(new GUIContent(types[i].Name));
                        }
                        else
                        {
                            menu.AddItem(new GUIContent(types[i].Name), false, OnWidgetAdded, types[i]);
                        }
                        menu.ShowAsContext();
                    }
                }
            }

            else if (m_AddForgeButtonSelected && current.type == EventType.MouseUp)
            {
                m_AddForgeButtonSelected = false;
                current.Use();
                GUIUtility.hotControl = 0;
                Repaint();
            }

            if (current.type == EventType.Repaint)
            {
                m_Styles.buttonLeft.Draw(buttonRect, ScriptForgeLabels.addForgeLabel, isHover: m_AddForgeButtonSelected, isActive: m_AddForgeButtonSelected, on: false, hasKeyboardFocus: false);
            }
        }

        /// <summary>
        /// Called by our generic menu when a new forge is added. 
        /// </summary>
        private void OnWidgetAdded(object widgetType)
        {
            m_Target.OnWidgetAdded(widgetType);
            m_AddForgeButtonSelected = false;
         
        }
    }
}
