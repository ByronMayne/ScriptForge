using System; 
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ScriptForge
{
	[System.Serializable]
    public abstract class EditorWidget
    {
        #region -= STATIC ITEMS
        /// <summary>
        /// All EditorBox items are updated using one delegate. This keeps things simple. 
        /// </summary>
        public static Action _OnGUI;
        /// <summary>
        /// All EditorBox items are updated using one delegate. This keeps things simple. 
        /// </summary>
        public static Action _OnUpdate;
        /// <summary>
        /// If the user would rather not have the boxes slide open and fade in set this to false; 
        /// </summary>
        public static bool _animateBoxes = true;
        /// <summary>
        /// This function is used to put a nice looking spacer on the editor window. (Its just a bar with space above and below).
        /// </summary>
        public static void Spacer()
        {
			GUI.color = Color.grey;
            GUILayout.Space(EDITOR_WINDOW_SPACING / 2);
            GUILayout.Box(GUIContent.none, GUILayout.Width(Screen.width - 10), GUILayout.Height(4.0f));
            GUILayout.Space(EDITOR_WINDOW_SPACING / 2);
			GUI.color = Color.white;
        }

	

		/// <summary>
        /// This is the next idea that is used to label the windows. 
        /// </summary>
        private static int _nextID = 0;
        /// <summary>
        /// This is the editor id for this window. 
        /// </summary>
        public int _ID { get; private set; }
        #endregion 

        #region -= CONSTANTS =- 
			/// <summary>
			/// This is the hieght of a collapsed widget.
			/// </summary>
			protected const float EDITOR_WIDGET_HEADER_HEIGHT  = 25.0f; 
            /// <summary>
            /// This is the spacing used for 1) The offset of the box from the sides. 2) the contents offset from the parent box  
            /// example | (offset) [ (offset) [  CONTENT  ] (offset)  ] (offset) |
            /// </summary>
            protected const float EDITOR_WINDOW_INSET = 8.0f;
            /// <summary>
            /// Each editor box is spaced apart on the top and the bottom by this much.
            /// </summary>
            protected const float EDITOR_WINDOW_SPACING = 3.0f;
            /// <summary>
            /// This is how fast the editor boxes open and close. 1.5 is pretty fast but go slower if you want more fun
            /// </summary>
            protected const float EDITOR_WINDOW_RESIZE_SPEED = 1.5f;
			/// <summary>
			/// This is the path used for the default editor skin. 
		    /// </summary>
			private const string EDITOR_GUI_SKIN_PATH = "UnityForgeSkin";
        #endregion 

        #region -= Variables
            /// <summary>
            /// If the content box is currently open or closed. (This really does not need a comment)
            /// </summary>
            public bool _isOpen { get; protected set; }
            /// <summary>
            /// This is the label that we use to show the foldout at the top. It also comes with a sick tool tip. 
            /// </summary>
            public GUIContent _content { get; protected set; }
            /// <summary>
            /// This is the size of the whole box (the part you see) it is inset by <para>EDITOR_WINDOW_INSET</para> on each side.
            /// </summary>
            protected Rect _widgetRect;
            /// <summary>
            /// This is the height of the content box when its fully opened.
            /// </summary>
            public float _expandedHeight { get; protected set; }
            /// <summary>
            /// This is the CURRENT hight of the editor box. This grows and shrinks for the animations. 
            /// </summary>
            private float contentHeight;
            /// <summary>
            /// When a widget has an error it will flash red for this amount of time.
            /// </summary>
            private float errorFlashTime = 3.0f;
            /// <summary>
            /// At what time should we stop flashing?
            /// </summary>
            private float currentErrorFlashTimeLeft; 
			/// <summary>
			/// The build flash time.
			/// </summary>
			private float buildFlashTime = 2.0f;
			/// <summary>
			/// The current build flash time left.
			/// </summary>
			private float currentBuildFlashTimeLeft;
			/// <summary>
			/// This is used to display a message at the top of the Widget.
			/// </summary>
			private string flashMessage = "";
		 	/// <summary>
		 	/// This is the current background color of the widget.
			/// </summary>
			private Color  boxBackgroundColor = Color.white; 
        #endregion 

        #region -= Functions =- 
            /// <summary>
            /// The constructor we use to build our editor boxes. 
            /// </summary>
            /// <param name="Name">This is the name that will showup on the foldout at the top.</param>
            /// <param name="Tooltip">This is the message that the user will see when they put their mouse over the foldout.</param>
            /// <param name="Height">How tall will the editor box be when fully opened?</param>
            public EditorWidget()
            {
				_content = Description();

                _OnGUI += OnGUI;
                _OnUpdate += Update;

				contentHeight = EDITOR_WIDGET_HEADER_HEIGHT;

                _ID = _nextID++;

				LoadPrefValues();

				_expandedHeight = 200.0f;
            }

            /// <summary>
            /// This is the deconstructor. It's only used to unsubscribe our OnGUI method from the static delegate. (It will cause errors if you don't);
            /// </summary>
			public virtual void Destroy()
            {
                _OnGUI -= OnGUI;

				SavePrefValues();
            }

            protected virtual void LoadPrefValues()
            {
				_isOpen = sf_EditorPrefs.GetPref(sf_EditorPrefs.EP_IS_OPEN, this );

				if( _isOpen )
				contentHeight = _expandedHeight;
				else
				contentHeight = EDITOR_WIDGET_HEADER_HEIGHT;
            }

            protected virtual void SavePrefValues()
            {
				sf_EditorPrefs.SetPref(sf_EditorPrefs.EP_IS_OPEN, _isOpen, this );
            }

            /// <summary>
            /// This is the method that will draw on the elements in our inspector. 
            /// </summary>
            public void OnGUI()
            {
                //We need to get the current postion of the layout. We have to make sure we are drawing in
                //the correct postion. 
                Rect lastRect = GUILayoutUtility.GetRect(GUILayoutUtility.GetLastRect().width, contentHeight);

                //So it turns out if you don't do a check for the correct event this function will
                //screw up your code (it gets the wrong rect since OnGUI is called for every GUI event.
                if (Event.current.type == EventType.repaint)
                    _widgetRect = new Rect(lastRect.x + EDITOR_WINDOW_INSET,
                                             lastRect.y + EDITOR_WINDOW_SPACING * 2,
                                             Screen.width - EDITOR_WINDOW_INSET * 2 - (10),
                                             contentHeight);


                //The background for the foldout

                if (currentErrorFlashTimeLeft > Time.realtimeSinceStartup )
                {
					float precent = 0.3f; //((Mathf.Sin(Time.realtimeSinceStartup * 8) + 1) / 4) + 0.25f;
					boxBackgroundColor =  new Color(1, precent, precent);
					flashMessage = " ! Build Failed ! ";
                    ScriptForge.Instance.Repaint();
                }

				else if ( currentBuildFlashTimeLeft > Time.realtimeSinceStartup )
				{
					float precent = 0.3f; //((Mathf.Sin(Time.realtimeSinceStartup * 5) + 1) / 4) + 0.25f;
					boxBackgroundColor = new Color(precent, 1, precent);
					ScriptForge.Instance.Repaint();
					flashMessage = " Build Successful ";
				}
				else
					flashMessage = "";

				boxBackgroundColor = Color.Lerp( boxBackgroundColor,Color.white, 0.01f);
				GUI.color = boxBackgroundColor;
                GUI.Box(_widgetRect, GUIContent.none);

                GUI.color = Color.white;

                //The new group
                GUI.BeginGroup(_widgetRect);

                //The content with the inset
                GUILayout.BeginArea(new Rect(EDITOR_WINDOW_INSET, 0, _widgetRect.width - EDITOR_WINDOW_INSET * 2, _widgetRect.height));

                //This is used for fading the colors on closing the window
				float colorPrecent = (contentHeight - EDITOR_WIDGET_HEADER_HEIGHT) / (_expandedHeight - EDITOR_WIDGET_HEADER_HEIGHT);

                //This open an closes the window
                EditorGUI.BeginChangeCheck();
				GUILayout.BeginHorizontal();

				 EditorGUILayout.LabelField( WidgetIcon() , sf_Skins.FontAwesomeStyle, GUILayout.MaxWidth(20), GUILayout.Height(20) );
					
				 Rect headerRect = GUILayoutUtility.GetLastRect();
			  	 headerRect.width = Screen.width;
			     headerRect.height = EDITOR_WIDGET_HEADER_HEIGHT;

				 if( Event.current.type == EventType.mouseUp && headerRect.Contains(Event.current.mousePosition))
				 {
					_isOpen = !_isOpen;
					GUI.changed = true;
					Event.current.Use();
					SavePrefValues();
					boxBackgroundColor = new Color(0.7f, 0.7f, 0.7f );
				 }

			GUILayout.Label(_content, sf_Skins.WidgetTitleStyle );


			GUILayout.Label(flashMessage, EditorStyles.miniLabel );


			GUILayout.Space(-200);
					GUILayout.BeginVertical();
			GUILayout.EndVertical();
				GUILayout.EndHorizontal();
                if (EditorGUI.EndChangeCheck())
                    SavePrefValues();

                //The color the gui currently is
                if (_animateBoxes)
                    GUI.color = new Color(1, 1, 1, colorPrecent);
                else
                    GUI.color = Color.white;

                {
                    // WINDOW CONTENT START

                    DrawWindowContent();

                    // WINDOW CONTENT END
                }
                if (_animateBoxes)  GUI.color = Color.white;

				Rect currentRect = GUILayoutUtility.GetLastRect();
				
				if( Event.current.type == EventType.Repaint ) 
				{
					
					_expandedHeight = currentRect.y + EditorGUIUtility.singleLineHeight * 2.0f;
				}


                //End our layout group
                GUILayout.EndArea();

				

                //End our gui group
                GUI.EndGroup();



                //Lave a spacer at the bottom so the spacer does not overlap. 
                GUILayout.Space(10.0f);

                ///The footer for our box.
                Spacer();



            }

            /// <summary>
            /// We don't really need this update but we use it for animations of the editor. 
            /// pretty much just eye candy. 
            /// </summary>
            protected virtual void Update()
            {
                #region -= BOX ANIMATIONS =- 
                        if (_isOpen)
                        {


                            if (contentHeight < _expandedHeight)
                            {
                                if( _animateBoxes )
                                    contentHeight += EDITOR_WINDOW_RESIZE_SPEED;
                                else
                                    contentHeight = _expandedHeight ;

                                ScriptForge.Instance.Repaint();
                            }
                        }
                        else
							if (contentHeight > EDITOR_WIDGET_HEADER_HEIGHT)
                            {
                                if( _animateBoxes )
                                    contentHeight -= EDITOR_WINDOW_RESIZE_SPEED;
                                else
									contentHeight = EDITOR_WIDGET_HEADER_HEIGHT; 

                                ScriptForge.Instance.Repaint();
                            }
    
                #endregion 
            }

            /// <summary>
            /// I am not using this right now but its here to be overridden. 
            /// </summary>
            //TODO: Make this abstract.
            protected abstract void DrawWindowContent();

			/// <summary>
			/// This is used be each forget to return it's correct font awesome widget. 
			/// </summary>
			/// <returns>The icon.</returns>
			protected abstract string WidgetIcon();

			/// <summary>
			/// This is called whenever there is an error building. It flashes the widget red.
			/// </summary>
            protected void FlashError()
            {
                currentErrorFlashTimeLeft = Time.realtimeSinceStartup + errorFlashTime;
            }
			/// <summary>
			/// This is called whenever we build scripts. It flashs the widgets blue.
			/// </summary>
			protected void FlashBuild()
			{
				currentBuildFlashTimeLeft = Time.realtimeSinceStartup + buildFlashTime;
			}

			/// <summary>
			/// Description this instance.
			/// </summary>
			protected abstract GUIContent Description();
        #endregion 
    }

}