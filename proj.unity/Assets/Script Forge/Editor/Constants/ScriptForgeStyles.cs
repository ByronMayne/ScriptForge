

using UnityEditor;
using UnityEngine;

namespace ScriptForge
{
    /// <summary>
    /// This class contains all our GUI Content labels that we use in Script Forge
    /// </summary>
    public class ScriptForgeStyles
    {

        /// <summary>
        /// The search filter we use to find our font. 
        /// </summary>
        private const string FONT_AWESOME_SEARCH_FILTER = "t:Font fontawesome-webfont";
        private const string TITLE_FONT_NAME = "t:Font AlegreyaSansSC-Bold";
        private const string NORMAL_FONT_NAME = "t:Font Catamaran-Regular";
        // Fonts
        private Font m_FontAwesomeFont;
        private Font m_TitleFont;
        private Font m_NormalFont;

        public Font fontAwesomeFont { get { return m_FontAwesomeFont; } }
        public Font rustFont { get { return m_TitleFont; } }
        public Font thapkieMGFont { get { return m_NormalFont; } }

        public GUIStyle spacer { get; protected set; }
        public GUIStyle titleBarIcon { get; protected set; }
        public GUIStyle title { get; protected set; }
        public GUIStyle subTitle { get; protected set; }
        public GUIStyle button { get; protected set; }
        public GUIStyle changePathButton { get; protected set; }
        public GUIStyle miniButtonLeft { get; protected set; }
        public GUIStyle miniButtonRight { get; protected set; }
        public GUIStyle miniButtonMiddle { get; protected set; }
        public GUIStyle buttonLeft { get; protected set; }
        public GUIStyle buttonRight { get; protected set; }
        public GUIStyle buttonMiddle { get; protected set; }
        public GUIStyle widgetHeaderText { get; protected set; }
        public GUIStyle widgetHeaderIcon { get; protected set; }

        public GUIStyle scriptForgeIconSmall { get; protected set; }

        public ScriptForgeStyles()
        {
            LoadFont(FONT_AWESOME_SEARCH_FILTER, ref m_FontAwesomeFont);
            LoadFont(TITLE_FONT_NAME, ref m_TitleFont);
            LoadFont(NORMAL_FONT_NAME, ref m_NormalFont);

            // Spacer
            spacer = new GUIStyle(GUI.skin.box);
            spacer.fixedHeight = 5f;
            spacer.fixedWidth = 0f;
            spacer.stretchWidth = true;

            // Title Bar Icon
            titleBarIcon = new GUIStyle(GUI.skin.label);
            titleBarIcon.fontSize = 35;
            titleBarIcon.fixedWidth = 50f;
            titleBarIcon.fontStyle = FontStyle.Normal;
            titleBarIcon.alignment = TextAnchor.MiddleCenter;
            titleBarIcon.wordWrap = false;
            titleBarIcon.clipping = TextClipping.Overflow;
            titleBarIcon.imagePosition = ImagePosition.TextOnly;
            titleBarIcon.font = fontAwesomeFont;
            titleBarIcon.normal.textColor = Color.white;

            // Title
            title = new GUIStyle(GUI.skin.label);
            title.fontSize = 30;
            title.fixedWidth = 50f;
            title.fixedHeight = 50f;
            title.fontStyle = FontStyle.Normal;
            title.alignment = TextAnchor.LowerLeft;
            title.wordWrap = false;
            title.contentOffset = new Vector2(-10f, 10f);
            title.clipping = TextClipping.Overflow;
            title.imagePosition = ImagePosition.TextOnly;
            title.font = rustFont;
            title.normal.textColor = Color.white;

            // Sub Title
            subTitle = new GUIStyle(GUI.skin.label);
            subTitle.fontSize = 15;
            subTitle.fixedWidth = 50f;
            subTitle.fontStyle = FontStyle.Normal;
            subTitle.alignment = TextAnchor.MiddleCenter;
            subTitle.wordWrap = false;
            subTitle.contentOffset = new Vector2(80, 40f);
            subTitle.clipping = TextClipping.Overflow;
            subTitle.imagePosition = ImagePosition.TextOnly;
            subTitle.font = rustFont;
            subTitle.normal.textColor = Color.white;

            // Button
            button = new GUIStyle(GUI.skin.button);
            button.fontSize = 20;
            button.fontStyle = FontStyle.Normal;
            button.alignment = TextAnchor.MiddleCenter;
            button.font = thapkieMGFont;
            button.fixedHeight = EditorGUIUtility.singleLineHeight * 2f;

            // Change Button Path
            changePathButton = new GUIStyle(button);
            changePathButton.fixedHeight = EditorGUIUtility.singleLineHeight;
            changePathButton.stretchWidth = false;
            changePathButton.fixedWidth = 100f;
            changePathButton.fontSize = 12;
            changePathButton.contentOffset = new Vector2(0, 1f);
            changePathButton.margin = new RectOffset(0, 0, 1, 1);

            // Mini Button Left
            miniButtonLeft = new GUIStyle(EditorStyles.miniButtonLeft);
            miniButtonLeft.fontSize = 12;
            miniButtonLeft.fontStyle = FontStyle.Normal;
            miniButtonLeft.alignment = TextAnchor.MiddleCenter;
            miniButtonLeft.font = thapkieMGFont;
            buttonLeft = new GUIStyle(miniButtonLeft);
            buttonLeft.fontSize = 15;
            buttonLeft.fixedHeight = EditorGUIUtility.singleLineHeight * 2f;

            // Mini Button Middle
            miniButtonMiddle = new GUIStyle(EditorStyles.miniButtonMid);
            miniButtonMiddle.fontSize = 12;
            miniButtonMiddle.fontStyle = FontStyle.Normal;
            miniButtonMiddle.alignment = TextAnchor.MiddleCenter;
            miniButtonMiddle.font = thapkieMGFont;
            buttonMiddle = new GUIStyle(miniButtonMiddle);
            buttonMiddle.fontSize = 15;
            buttonMiddle.fixedHeight = EditorGUIUtility.singleLineHeight * 2f;

            // Mini Button Right
            miniButtonRight = new GUIStyle(EditorStyles.miniButtonRight);
            miniButtonRight.fontSize = 12;
            miniButtonRight.fontStyle = FontStyle.Normal;
            miniButtonRight.alignment = TextAnchor.MiddleCenter;
            miniButtonRight.font = thapkieMGFont;
            buttonRight = new GUIStyle(miniButtonRight);
            buttonRight.fontSize = 15;
            buttonRight.fixedHeight = EditorGUIUtility.singleLineHeight * 2f;

            // Widget Header Text
            widgetHeaderText = new GUIStyle(GUI.skin.label);
            widgetHeaderText.fontSize = 22;
            widgetHeaderText.alignment = TextAnchor.UpperLeft;
            widgetHeaderText.wordWrap = true;
            widgetHeaderText.richText = true;
            widgetHeaderText.contentOffset = new Vector2(8f, 1f);
            widgetHeaderText.font = rustFont;
            widgetHeaderIcon = new GUIStyle(widgetHeaderText);
            widgetHeaderIcon.fixedHeight = 28f;
            widgetHeaderIcon.fixedWidth = 28f;
            widgetHeaderIcon.font = fontAwesomeFont;
            widgetHeaderIcon.contentOffset = new Vector2(5, 2);

            scriptForgeIconSmall = new GUIStyle();
            scriptForgeIconSmall.stretchWidth = true;
            scriptForgeIconSmall.stretchHeight = true;
            scriptForgeIconSmall.fixedHeight = 50;
            scriptForgeIconSmall.fixedWidth = 50;
            scriptForgeIconSmall.margin = new RectOffset(0, 0, 5, 5);
            HierarchyProperty serach = new HierarchyProperty(HierarchyType.Assets);
            serach.SetSearchFilter("t:Texture ScriptForgeIcon", 0);
            serach.Next(null);
            if(serach.pptrValue != null)
            {
                scriptForgeIconSmall.normal.background = (Texture2D)serach.pptrValue;
            }
        }

        /// <summary>
        /// Finds Font Awesome on disk and loads it.
        /// </summary>
        private void LoadFont(string search, ref Font result)
        {
            // Look in assets folder
            HierarchyProperty fontSearch = new HierarchyProperty(HierarchyType.Assets);
            // Set our filter
            fontSearch.SetSearchFilter(search, 0);
     
            // Loop over all results
            while(fontSearch.Next(null))
            {
                if(fontSearch.pptrValue != null && fontSearch.pptrValue is Font)
                {
                    // Cast our font and load it.
                    result = (Font)fontSearch.pptrValue;
                }
            }
        }
    }
}
