using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEditor;

namespace ScriptForge
{
	public abstract class FolderFilterWidget : ForgeWidget
	{
		private const string RESOURCES_FOLDER_NAME = "/Resources";
		private GUIContent m_FoldersTitle = new GUIContent("Folders To Include");
		
		[SerializeField]
		private List<string> m_Folders; 

		// Editor Drawing. 
		private ReorderableList m_FoldersDrawer; 

		/// <summary>
		/// Gets the list of folders that we use for this
		/// widget. 
		/// </summary>
		public List<string> folders 
		{
			get { return m_Folders; }
		}

		/// <summary>
		/// Gets or sets the title that appears above our list of
		/// folder filters. 
		/// </summary>
		public GUIContent foldersTitle
		{
			get { return m_FoldersTitle; }
			set { m_FoldersTitle = value; }
		}

		/// <summary>
		/// Invoked when our widget is created. 
		/// </summary>
		protected override void OnEnable()
		{
			// Call base. 
			base.OnEnable();
			// Create our drawer
			m_FoldersDrawer = new ReorderableList(m_Folders, typeof(string));
			// Subscribe our callbacks
			m_FoldersDrawer.drawHeaderCallback += OnDrawFoldersHeader;
			m_FoldersDrawer.drawElementCallback += OnDrawFolderContent; 
			m_FoldersDrawer.onAddCallback += OnAddFolderRequested;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			// Unsubscribe our callbacks
			m_FoldersDrawer.drawHeaderCallback -= OnDrawFoldersHeader;
			m_FoldersDrawer.drawElementCallback -= OnDrawFolderContent; 
			m_FoldersDrawer.onAddCallback -= OnAddFolderRequested;
		}
			
		/// <summary>
		/// Invoked when we draw the header for our list of folders we use as filters. 
		/// </summary>
		protected virtual void OnDrawFoldersHeader(Rect rect)
		{
			GUI.Label(rect, m_FoldersTitle);
		}

		/// <summary>
		/// Invoked when we draw each element  for our list of folders. 
		/// </summary>
		protected virtual void OnDrawFolderContent(Rect rect, int index, bool isActive, bool isFocused)
		{
			GUI.Label(rect, m_Folders[index]);
		}

		/// <summary>
		/// Invoked when ever the user presses the + button to add a 
		/// new folder.
		/// </summary>
		protected virtual void OnAddFolderRequested(ReorderableList list)
		{
			// Get the system path 
			string systemPath = EditorUtility.OpenFolderPanel("Folder To Include", Application.dataPath, "Resources");
			// Convert it to a Unity path
			string assetPath = FileUtil.GetProjectRelativePath(systemPath); 
			// Make sure it's not null
			if(!string.IsNullOrEmpty(assetPath))
			{
				// Make sure it's a resources path
				int resourcesIndex = assetPath.LastIndexOf(RESOURCES_FOLDER_NAME + "/");
				// Get our end index
				// Check to see if the index is great then -1
				if(resourcesIndex >= 0 || assetPath.EndsWith(RESOURCES_FOLDER_NAME))
				{
					if(!folders.Contains(assetPath))
					{
						folders.Add(assetPath);
					}
				} 
				else
				{
					EditorUtility.DisplayDialog("Invalid Resource Path", "The path '" + assetPath + "' does not contain a resources folder. Please try again", "Okay");
				}
			} 
			else
			{
				EditorUtility.DisplayDialog("Invalid Resource Path", "The path '" + systemPath + "' is not contained inside the current Unity project. The path must be.", "Okay");
			}
		}

		protected override void DrawWidgetContent(ScriptForgeStyles style)
		{
			base.DrawWidgetContent(style);
			// Draw our folders
			m_FoldersDrawer.DoLayoutList();
		}
	}
}
