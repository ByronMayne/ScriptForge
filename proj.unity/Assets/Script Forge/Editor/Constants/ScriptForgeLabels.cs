

using UnityEditor;
using UnityEngine;

namespace ScriptForge
{
    /// <summary>
    /// This class contains all our GUI Content labels that we use in Script Forge
    /// </summary>
    public static class ScriptForgeLabels
    {
        public static readonly GUIContent HEADER_TITLE;
        public static readonly GUIContent HEADER_SUB_TITLE;

        public static readonly GUIContent openAllWidgetsLabel;
        public static readonly GUIContent closeAllWidgetsLabel;
        public static readonly GUIContent addWidget;
        public static readonly GUIContent generateAllForgesLabel;
        public static readonly GUIContent setCommonPathLabel;
        public static readonly GUIContent RESET_ALL_FORGES_BUTTON_LABEL;

        public static readonly GUIContent DESCRIPTION_LAYERS_WIDGET;
        public static readonly GUIContent DESCRIPTION_TAGS_WIDGET;
        public static readonly GUIContent DESCRIPTION_SORTINGLAYERS_WIDGET;
        public static readonly GUIContent DESCRIPTION_SCENE_WIDGET;
        public static readonly GUIContent DESCRIPTION_INPUT_WIDGET;
        public static readonly GUIContent DESCRIPTION_ABOUT_WIDGET;
        public static readonly GUIContent DESCRIPTION_SETTINGS_WIDGET;
        public static readonly GUIContent aboutWidgetContent;

        // Widge Content
        public static readonly GUIContent defaultWidgetTitle;
        public static readonly GUIContent aboutWidgetTitle;
        public static readonly GUIContent tagsWidgetTitle;
        public static readonly GUIContent layersWidgetTitle;
        public static readonly GUIContent sortingLayersTitle;
        public static readonly GUIContent scenesWidgetTitle;
        public static readonly GUIContent settingsWidgetTitle;
        public static readonly GUIContent animationsWidgetTitle;
        public static readonly GUIContent resourcesWidgetTitle;

        public static readonly GUIContent generateForgeButton;
        public static readonly GUIContent forceGenerateForgeButton;
        public static readonly GUIContent resetForgeButton;
        public static readonly GUIContent removeForgeButton;

        // Forge Widget
        public static readonly GUIContent autoBuildContent;
        public static readonly GUIContent scriptLocation;
        public static readonly GUIContent namespaceContent;
        public static readonly GUIContent changePathContent;

        // Animations Widget
        public static readonly GUIContent generateClipNamesLabel;
        public static readonly GUIContent generateLayerNamesLabel;
        public static readonly GUIContent generateParamatersLabel;

        // Scene Forge
        public static readonly GUIContent classNameContent;
        public static readonly GUIContent enumNameContent;

        // Settings
        public static readonly GUIContent animateWidgetsContent;
        public static readonly GUIContent indentCountLabel;

        // Layers Widget
        public static readonly GUIContent createBitwiseLabel; 

        // Development
        public static readonly GUIContent inDevelopmentIcon;

        // About
        public static readonly GUIContent documentationButtonLabel;
        public static readonly GUIContent issuesButtonLabel;
        public static readonly GUIContent repoButtonLabel;
        public static readonly GUIContent twitterButtonLabel;
		public static readonly GUIContent licenseButtonLabel;

		// Forge Labels
		public static readonly GUIContent forgeErrorIcon;
		public static readonly GUIContent forgeUpToDateIcon;
		public static readonly GUIContent forgeOutOfDateIcon;

		// Script Save Location
		public static class ScriptSaveLocation
		{
			public static readonly string title;
			public static readonly string extension;
			public static readonly string message; 

			static ScriptSaveLocation()
			{
				title = "Script Save Location";
				extension = "cs";
				message = "Please choose a save location for your script.";
			}
		}

        /// <summary>
        /// Are content is only created when it's first used.
        /// </summary>
        static ScriptForgeLabels()
        {
            openAllWidgetsLabel = new GUIContent("Open All Widgets", "This will open all the widgets in  Script Forge");
            closeAllWidgetsLabel = new GUIContent("Close All Widgets", "This will close all open widgets in  Script Forge");
            addWidget = new GUIContent("Add Widget", "This will add a new widget that is not already part of Script Forge");
            generateAllForgesLabel = new GUIContent("Generate All Forges", "This will tell all forges to generate their scripts if they have changed since last time");
            setCommonPathLabel = new GUIContent("Set Common Path", "This is the path that all forges will build their scripts to.");
            RESET_ALL_FORGES_BUTTON_LABEL = new GUIContent("Reset Forges", "This will reset all forges to their default values.");
            DESCRIPTION_LAYERS_WIDGET = new GUIContent("Layers", "This forge is used to crate a static class for all Layers in your Unity Project. It makes both Bitwise value and Intagers.");
            DESCRIPTION_TAGS_WIDGET = new GUIContent("Tags", "This forge is used to crate a static class for all the tags in your Unity project.");
            DESCRIPTION_SORTINGLAYERS_WIDGET = new GUIContent("Sorting Layers", "This forge is used to create a static class for all the sorting layers in your Unity project.");
            DESCRIPTION_SCENE_WIDGET = new GUIContent("Scenes", "This forge is used to keep track of all your scenes in your project.");
            DESCRIPTION_INPUT_WIDGET = new GUIContent("Input", "This forge is used to keep track of all the Axis set up in the Unity Input Editor");
            DESCRIPTION_ABOUT_WIDGET = new GUIContent("About", "This is where you can adjust any settings for ScriptForge.");
            DESCRIPTION_SETTINGS_WIDGET = new GUIContent("Settings", "Who made ScriptForge and what does it do?");
            HEADER_TITLE = new GUIContent("cript Forge", "Making jobs simpler since 2014.");
            HEADER_SUB_TITLE = new GUIContent("v" + ProjectVersion.PROJECT_VERSION, "Last update July 2016/10/01.");
            defaultWidgetTitle = new GUIContent("Default", "This is a debugging default widget");
            aboutWidgetContent = new GUIContent("Script Forge was originally developed as a learning tool for myself to learn how to make more complex " +
                                                "editors and use T4 templates. I recently open sourced the full version to help others learn how to make " + 
                                                "awesome tools. Want to add some new features? Feel free to fork the repository and make a pull request. " + 
                                                "Keep in mind that every button and setting has a tooltip so if you are lost just hover over it.");
            aboutWidgetTitle = new GUIContent("About");
            tagsWidgetTitle = new GUIContent("Tags");
            layersWidgetTitle = new GUIContent("Layers");
            scenesWidgetTitle = new GUIContent("Scenes");
            sortingLayersTitle = new GUIContent("Sorting Layers");
            settingsWidgetTitle = new GUIContent("Settings");
            resourcesWidgetTitle = new GUIContent("Resources");
            animationsWidgetTitle = new GUIContent("Animations");
            generateForgeButton = new GUIContent("Generate", "This is regenerate the class only if it's not up to date");
            forceGenerateForgeButton = new GUIContent(FontAwesomeIcons.REPEAT, "This will force regenerate the template even if it's up to date"); 
            resetForgeButton = new GUIContent("Reset");
            removeForgeButton = new GUIContent("Remove");
            autoBuildContent = new GUIContent("Auto Build", "Should this forge run automatically in the background when it detects a change?");
            scriptLocation = new GUIContent("Script Location", "Where should the class this forge generates be exported to?");
            namespaceContent = new GUIContent("Namespace", "Which namespace (if any) should the generated class have?");
			changePathContent = new GUIContent(FontAwesomeIcons.PENCIL_SQUARE, "Click this button to allow you to pick a new path in the project to save the output too");
            classNameContent = new GUIContent("Class Name", "What should the name of the class that is generated be called? All invalid characters will be removed");
            enumNameContent = new GUIContent("Enum Name", "The name of the enum that is generated in the class");
            animateWidgetsContent = new GUIContent("Animate Widgets", "If true the widgets open and close with nice animations otherwise it will be instant.");
            indentCountLabel = new GUIContent("Indent Space Count", "This is used to tell use how many spaces we want to use for indenting when we generate our scripts. We don't use tabs because I am not adding that feature, you can if you want");
            inDevelopmentIcon = new GUIContent(FontAwesomeIcons.CODE, "This widget is currently in development and might not be fully featured.");
            documentationButtonLabel = new GUIContent(FontAwesomeIcons.BOOK, "Opens the documentation written on Google Docs");
            issuesButtonLabel = new GUIContent(FontAwesomeIcons.BUG, "Opens the issues page on GitHub where you can submit any bugs you might find. You can also log any feature requests here.");
            repoButtonLabel = new GUIContent(FontAwesomeIcons.GITHUB, "Opens the repository on GitHub.");
            twitterButtonLabel = new GUIContent(FontAwesomeIcons.TWITTER, "Opens up twitter to my page where you can follow me.");
            createBitwiseLabel = new GUIContent("Create Bitwise", "By default the layers will create int constants to use in code. If this is set to true it would also create bitwise version.");
			licenseButtonLabel = new GUIContent(FontAwesomeIcons.ID_CARD, "Opens up the link to the license that covers Script Forge.");
			// Forge icons
			forgeErrorIcon = new GUIContent(FontAwesomeIcons.WARNING, "This forge was encountered an error while trying to run the template. Fix the error by reading the error message below (open the widget) and hit generate to rebuild");  
			forgeUpToDateIcon = new GUIContent(FontAwesomeIcons.CHECKBOX, "This forge has already been run and is up to date with the current state of the project. There is no point to run this again");
			forgeOutOfDateIcon = new GUIContent(FontAwesomeIcons.REFRESH, "This forge is out of date with the current state of the project. You should hit Generate to get the updated output");

            // Animations Widget
            generateClipNamesLabel = new GUIContent("Generate Clips Names", "If true when the class is generated for this Animator Controller we will include fields for each clip");
            generateLayerNamesLabel = new GUIContent("Generate Layer Names", "If true when the class is generated for this Animator Controller we will include fields for each layer in the controller");
            generateParamatersLabel = new GUIContent("Generate Parameters", "If true when the class is generated for this Animator Controller we will include fields for the parameters");
        }

    }
}
