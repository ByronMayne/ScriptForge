using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Reflection;
using Type = System.Type;
using UnityEditor.Callbacks;

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
        private static void Initialize()
        {
            var instance = GetInstance();

            if(instance == null)
            {
                Debug.Log("Unable to load or create Scriptable Forge instance");
            }
        }

        /// <summary>
        /// Loads the instance from disk.
        /// </summary>
        private static ScriptableForge GetInstance()
        {
            ScriptableForge[] instances = Resources.FindObjectsOfTypeAll<ScriptableForge>();

            if (instances.Length == 0)
            {
                ReadInstanceFromDiskOrCreateNew();
                return GetInstance();
            }
            else if (instances.Length > 1)
            {
                for (int i = 1; i < instances.Length; i++)
                {
                    Debug.Log("Cleaning up extra instances of Scriptable Forge");
                    DestroyImmediate(instances[i], true);
                }
            }
            return instances[0];
        }

        private static void ReadInstanceFromDiskOrCreateNew()
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
                        if (loadedObject[i] is Widget)
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
            for (int i = 0; i < m_LoadedWidgets.Count; i++)
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
            List<Object> saveList = new List<Object>();
            // Set this first instance to our data.  
            saveList.Add(this);
            // Copy our widgets into our array.
            for(int i = 0; i < m_Widgets.Count; i++)
            {
                if(m_Widgets[i] != null)
                {
                    saveList.Add(m_Widgets[i]);
                }
            }
            ScriptableForge[] instances = Resources.FindObjectsOfTypeAll<ScriptableForge>();

            Debug.Log("Savinging INstances: " + instances.Length + " Widget Count: " + m_Widgets.Count.ToString());
            // Save them to disk. 
            InternalEditorUtility.SaveToSerializedFileAndForget(saveList.ToArray(), savePath, true);
        }

        [MenuItem("Edit/Project Settings/Script Forge")]
        private static void OpenSettings()
        {
            Selection.activeObject = GetInstance();
        }

        [System.NonSerialized]
        private bool m_AddForgeButtonSelected = false;

        [SerializeField]
        private List<Widget> m_Widgets = new List<Widget>();

        [SerializeField]
        private bool m_AnimateWidgets = true;

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
