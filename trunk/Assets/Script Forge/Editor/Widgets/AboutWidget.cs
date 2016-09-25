using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ScriptForge
{
    public class AboutWidget : EditorWidget
    {
		private Vector2 _infoTextScrollArea = Vector2.zero;
		public static AboutWidget Instance { get; protected set; }

		public AboutWidget()
		{
			if( Instance != null )
			{
				Destroy();
				return;
			}

			Instance = this;
		}
		public override void Destroy() 
		{ 
			Instance = null ;

			base.Destroy();

			_OnGUI -= OnGUI; 
		}

		public GUIContent _infoContent = new GUIContent("This tool was developed by Byron Mayne on April 2014. If you have any questions or would like some custom modifcations please feel free to reach out to to me at Byronmayne@gmail.com or on LinkedIn at ca.linkedin.com/in/byronmayne/ \n\nRegards, \nByron M. (aspiring tools developer)" );

        protected override void DrawWindowContent()
        {
	
		

			GUILayout.Space(4);
			
			GUILayout.Box( GUIContent.none, GUILayout.Width(_widgetRect.width - EDITOR_WINDOW_INSET * 2), GUILayout.Height(4));

			_infoTextScrollArea = EditorGUILayout.BeginScrollView( _infoTextScrollArea, false, false, GUIStyle.none, GUI.skin.verticalScrollbar, GUI.skin.scrollView, GUILayout.MaxHeight(120));
				GUILayout.Label(" Script Forge v1.0", EditorStyles.boldLabel );
				GUILayout.Label(_infoContent, EditorStyles.wordWrappedLabel );
			GUILayout.EndScrollView();
			GUILayout.Space(10);
			GUILayout.Box( GUIContent.none, GUILayout.Width(_widgetRect.width - EDITOR_WINDOW_INSET * 2), GUILayout.Height(4));
			//if( Event.current.type == EventType.Layout )
				//_expandedHeight = GUILayoutUtility.GetRect(_infoContent, EditorStyles.label ).y;

			if( GUILayout.Button("Documentation", sf_Skins.Button) )
			{
				System.Diagnostics.Process.Start(ExtenalLinks.SCRIPT_FORGE_GOOLGE_DOC_URL);
			}
		
		}

		protected override string WidgetIcon ()
		{
			return FontAwesomeIcons.INFO_CIRCLE.ToString(); 
		}

		protected override GUIContent Description ()
		{
			return sf_Descriptions.DESCRIPTION_ABOUT_WIDGET;
		}
	}
}