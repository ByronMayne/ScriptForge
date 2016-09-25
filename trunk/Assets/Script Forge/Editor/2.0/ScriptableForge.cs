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
    public class ScriptableForge : Editor
    {
        #region -= Static =-
        /// <summary>
        /// What will this object be saved as?
        /// </summary>
        public const string SAVE_NAME = "ScriptableForge.asset";

        /// <summary>
        /// This is the internal instance we use to save ScriptForge Settings.
        /// </summary>
        private static ScriptableForge m_Instance;

        /// <summary>
        /// Gets the instance for ScriptableForge or creates a new one. 
        /// </summary>
        public static ScriptableForge instance
        {
            get
            {
                if (m_Instance == null)
                {
                    LoadOrCreateInstance();
                }
                return m_Instance;
            }
        }

        /// <summary>
        /// Gets the path where we save our scriptable object singleton. 
        /// </summary>
        /// <returns></returns>
        private static string GetSavePath()
        {
            // Get the root path
            string savePath = Application.dataPath;
            // Remove /Assets
            savePath = savePath.Replace("/Assets", "/ProjectSettings/" + SAVE_NAME);
            // Return it
            return savePath;
        }

        /// <summary>
        /// Called by Unity when we want to initialize our object. 
        /// </summary>
        [InitializeOnLoadMethod]
        public static void LoadOrCreateInstance()
        {
            // Store our path
            string savePath = GetSavePath();

            // Does it exist already?
            if (File.Exists(savePath))
            {
                // It does so lets load it
                Object[] loadedObject = InternalEditorUtility.LoadSerializedFileAndForget(savePath);

                // Loop over all our objects and find the one we care about (There should really only be one)
                for (int i = 0; i < loadedObject.Length; i++)
                {
                    if (loadedObject[i] is ScriptableForge)
                    {
                        // Save our instance
                        m_Instance = loadedObject[i] as ScriptableForge;
                    }
                    else
                    {
                        if (loadedObject[i] is EditorWidget)
                        {
                            m_Instance.m_Widgets.Add(loadedObject[i] as Widget);
                        }
                    }
                }
            }
            else
            {
                // We don't have a save file so we create a new one.
                m_Instance = CreateInstance<ScriptableForge>();
            }
        }

        /// <summary>
        /// Takes the ScriptableForge and all it's widgets and writes them to disk. 
        /// </summary>
        public static void SaveInstance()
        {
            // Get our path
            string savePath = GetSavePath();
            // Get a handle to our widgets
            Widget[] widgets = m_Instance.m_Widgets.ToArray();
            // Create an array of objects to save. 
            Object[] savingObjects = new Object[widgets.Length + 1];
            // Set this first instance to our data. 
            savingObjects[0] = m_Instance;
            // Copy our widgets into our array.
            widgets.CopyTo(savingObjects, 1);
            // Save them to disk.
            InternalEditorUtility.SaveToSerializedFileAndForget(savingObjects, savePath, true);
        }

        [MenuItem("Edit/Project Settings/Script Forge")]
        public static void OpenSettings()
        {
            Selection.activeObject = instance;
        }
        #endregion

        #region -= Data =-
        [System.NonSerialized]
        private bool m_AddForgeButtonSelected = false;

        [SerializeField]
        private List<Widget> m_Widgets = new List<Widget>();

        [SerializeField]
        private bool m_AnimateValues;

        [System.NonSerialized]
        private ScriptForgeStyles m_Styles;
        #endregion

        #region -= Drawing =-
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
        }

        /// <summary>
        /// Draws the header for script forge.
        /// </summary>
        private void DrawButtons()
        {
            if (GUILayout.Button(ScriptForgeLabels.generateAllForgesLabel, m_Styles.button))
            {

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
                    for (int i = 0; i < m_Widgets.Count; i++)
                    {
                        m_Widgets[i].isOpen = true;
                    }
                }

                if (GUILayout.Button(ScriptForgeLabels.closeAllWidgetsLabel, m_Styles.miniButtonMiddle))
                {
                    for (int i = 0; i < m_Widgets.Count; i++)
                    {
                        m_Widgets[i].isOpen = false;
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
            for (int i = 0; i < m_Widgets.Count; i++)
            {
                m_Widgets[i].OnWidgetGUI(m_Styles);

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

                        for (int x = 0; x < m_Widgets.Count; x++)
                        {
                            if (m_Widgets[x].GetType() == types[i])
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
            Type type = (Type)widgetType;
            Widget newWidget = (Widget)CreateInstance(type);
            m_Widgets.Add(newWidget);
            m_AddForgeButtonSelected = false;
            m_Widgets.Sort();
        }
        #endregion
    }
}
