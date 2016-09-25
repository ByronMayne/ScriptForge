using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Reflection;
using Type = System.Type;

namespace ScriptForge
{
    public class ScriptableForge : Editor
    {
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

        [System.NonSerialized]
        private bool m_AddForgeButtonSelected = false;

        [SerializeField]
        private List<Widget> m_Widgets = new List<Widget>();

        [SerializeField]
        private bool m_AnimateValues;

        /// <summary>
        /// Get or set our list of widgets.
        /// </summary>
        public List<Widget> Widgets
        {
            get { return m_Widgets; }
            set { m_Widgets = value; }
        }


        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {
   
        }

        /// <summary>
        /// Invoked when a new widget is added.
        /// </summary>
        public void OnWidgetAdded(object widgetType)
        {
            Type type = (Type)widgetType;
            Widget newWidget = (Widget)CreateInstance(type);
            m_Widgets.Add(newWidget);
            m_AddForgeButtonSelected = false;
            m_Widgets.Sort();
        }
    }
}
