using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Reflection;
using Type = System.Type;
using Attribute = System.Attribute;
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
        /// We save this key in Editor Prefs to see if we have used script forge before. 
        /// </summary>
        public const string FIRST_LAUNCH_KEY = "ScriptForge.FirstLaunch";

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

            if(!EditorPrefs.HasKey(FIRST_LAUNCH_KEY))
            {
                EditorPrefs.SetBool(FIRST_LAUNCH_KEY, true);
                ScriptableForge.OpenDocumentation();
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
                    DestroyImmediate(instances[i], true);
                }
            }
            return instances[0];
        }

        /// <summary>
        /// Gets all the types of all widgets that are defined in this assembly. 
        /// </summary>
        public static List<Type> GetDefinedWidgetTypes(bool includeAbstractTypes)
        {
            // Get our current assembly
            Assembly assembly = Assembly.GetExecutingAssembly();
            // Get all the types
            Type[] types = assembly.GetTypes();
            // Create a holder
            List<Type> widgetTypes = new List<Type>();
            // Loop over all types and see if they are correct
            for(int i = 0; i < types.Length; i++)
            {
                if(!includeAbstractTypes && types[i].IsAbstract)
                {
                    // We don't want abstract types and this type is.
                    continue;
                }

                if(typeof(Widget).IsAssignableFrom(types[i]))
                {
                    widgetTypes.Add(types[i]);
                }
            }
            return widgetTypes;
        }

        /// <summary>
        /// Reads the yaml defined on disk if there is one for a Scriptable Forge or
        /// creates a new one.
        /// </summary>
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

            // Get all our widget types.
            List<Type> widgetTypes = GetDefinedWidgetTypes(includeAbstractTypes: false);
            // Remove all that are not required. 
            for (int i = widgetTypes.Count - 1; i >= 0; i--)
            {
                if (Attribute.GetCustomAttribute(widgetTypes[i], typeof(RequiredWidgetAttribute)) != null)
                {
                    bool foundWidget = false;
                    for (int x = 0; x < m_LoadedWidgets.Count; x++)
                    {

                        if( m_LoadedWidgets[x].GetType() == widgetTypes[i] )
                        {
                            foundWidget = true; 
                            // We found the type so we move on.
                            break;
                        }
                    }
                    if(!foundWidget)
                    {
                        m_Instance.OnWidgetAdded(widgetTypes[i]);
                    }
                }
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

            // Save them to disk. 
            InternalEditorUtility.SaveToSerializedFileAndForget(saveList.ToArray(), savePath, true);
        }

        [MenuItem("Edit/Project Settings/Script Forge")]
        private static void OpenSettings()
        {
            Selection.activeObject = GetInstance();
        }

        /// <summary>
        /// Opens the google doc that has the documentation for Script Forge. 
        /// </summary>
        public static void OpenDocumentation()
        {
            System.Diagnostics.Process.Start(ExtenalLinks.SCRIPT_FORGE_GOOLGE_DOC_URL);
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
