

namespace ScriptForge
{
    public static class LayoutSettings
    {
        /// <summary>
        /// This is the height of a collapsed widget.
        /// </summary>
        public const float EDITOR_WIDGET_HEADER_HEIGHT = 25.0f;
        /// <summary>
        /// This is the spacing used for 1) The offset of the box from the sides. 2) the contents offset from the parent box  
        /// example | (offset) [ (offset) [  CONTENT  ] (offset)  ] (offset) |
        /// </summary>
        public const float EDITOR_WINDOW_INSET = 8.0f;
        /// <summary>
        /// Each editor box is spaced apart on the top and the bottom by this much.
        /// </summary>
        public const float EDITOR_WINDOW_SPACING = 3.0f;
        /// <summary>
        /// This is how fast the editor boxes open and close. 1.5 is pretty fast but go slower if you want more fun
        /// </summary>
        public const float EDITOR_WINDOW_RESIZE_SPEED = 1.5f;
        /// <summary>
        /// This is the path used for the default editor skin. 
        /// </summary>
        public const string EDITOR_GUI_SKIN_PATH = "UnityForgeSkin";

        public const int DEFAULT_WIDGET_SORT_ORDER = 0;
        public const int TAG_WIDGET_SORT_ORDER = 1;
        public const int LAYERS_WIDGET_SORT_ORDER = 2;
        public const int SORTING_LAYERS_WIDGET_SORT_ORDER = 3;
        public const int SCENES_WIDGET_SORT_ORDER = 4;
        public const int SETTINGS_WIDGET_SORT_ORDER = 5;
        public const int ABOUT_WIDGET_SORT_ORDER = 6;
    }
}
