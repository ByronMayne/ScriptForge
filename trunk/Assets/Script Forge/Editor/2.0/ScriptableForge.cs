using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Reflection;
using Type = System.Type;

namespace ScriptForge
{
    [System.Serializable]
    public class ScriptableForge : Editor
    {
        /// <summary>
        /// What will this object be saved as?
        /// </summary>
        public const string SAVE_NAME = "ScriptableForge.asset";

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

            ScriptableForge m_Instance = null;
            List<Widget> m_LoadedWidgets = new List<Widget>();

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
                            m_LoadedWidgets.Add(loadedObject[i] as Widget);
                        }
                    }
                }
            }
            else
            {
                // We don't have a save file so we create a new one.
                m_Instance = CreateInstance<ScriptableForge>();
            }

            // Initialize all the widgets. 
            for(int i = 0; i < m_LoadedWidgets.Count; i++)
            {
                m_LoadedWidgets[i].Initalize(m_Instance);
            }

            // Sort them in order.
            m_LoadedWidgets.Sort();

            // Save them back to our instance.
            m_Instance.Widgets = m_LoadedWidgets;

            // Regenerate Widgets if they are set to auto build.
            for (int i = 0; i < m_LoadedWidgets.Count; i++)
            {
                m_LoadedWidgets[i].OnLoaded();
            }
        }

        /// <summary>
        /// Takes the ScriptableForge and all it's widgets and writes them to disk. 
        /// </summary>
        public void Save()
        {
            // Get our path
            string savePath = GetSavePath();
            // Get a handle to our widgets
            Widget[] widgets = m_Widgets.ToArray();
            // Create an array of objects to save. 
            Object[] savingObjects = new Object[widgets.Length + 1];
            // Set this first instance to our data.  
            savingObjects[0] = this;
            // Copy our widgets into our array.
            widgets.CopyTo(savingObjects, 1);
            // Save them to disk.
            InternalEditorUtility.SaveToSerializedFileAndForget(savingObjects, savePath, true);
        }

        [MenuItem("Edit/Project Settings/Script Forge")]
        public static void OpenSettings()
        {
            LoadOrCreateInstance();
            Selection.activeObject = Resources.FindObjectsOfTypeAll<ScriptableForge>()[0];
        }

        [System.NonSerialized]
        private bool m_AddForgeButtonSelected = false;

        [SerializeField]
        private List<Widget> m_Widgets = new List<Widget>();

        [SerializeField]
        private bool m_AnimateWidgets;

        /// <summary>
        /// Get or set our list of widgets.
        /// </summary>
        public List<Widget> Widgets
        {
            get { return m_Widgets; }
            set { m_Widgets = value; }
        }

        /// <summary>
        /// Gets or sets the setting if we should animate our widgets.
        /// </summary>
        public bool animateWidgets
        {
            get { return m_AnimateWidgets; }
            set { m_AnimateWidgets = value; }
        }


        /// <summary>
        /// Invoked when a new widget is added.
        /// </summary>
        public void OnWidgetAdded(object widgetType)
        {
            Type type = (Type)widgetType;
            Widget newWidget = (Widget)CreateInstance(type);
            newWidget.Initalize(this);
            m_Widgets.Add(newWidget);
            m_AddForgeButtonSelected = false;
            m_Widgets.Sort();
        }
    }
}
