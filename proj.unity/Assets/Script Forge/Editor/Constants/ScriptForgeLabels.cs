

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

        public static readonly GUIContent generateForgeButton;
        public static readonly GUIContent resetForgeButton;
        public static readonly GUIContent removeForgeButton;

        // Forge Widget
        public static readonly GUIContent autoBuildContent;
        public static readonly GUIContent scriptLocation;
        public static readonly GUIContent namespaceContent;
        public static readonly GUIContent changePathContent;

        // Scene Forge
        public static readonly GUIContent classNameContent;
        public static readonly GUIContent enumNameContent;

        // Settings
        public static readonly GUIContent animateWidgetsContent;

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
            HEADER_SUB_TITLE = new GUIContent("v 2.1", "Last update July 2016/10/01.");
            defaultWidgetTitle = new GUIContent("Default", "This is a debugging default widget");
            aboutWidgetContent = new GUIContent("ScriptForge is a tool used to auto generate classes the are frequently used by developers who use Unity. If you have any questions, suggestions or issues please feel free to reach out to me at Byronmayne@gmail.com.");
            aboutWidgetTitle = new GUIContent("About");
            tagsWidgetTitle = new GUIContent("Tags");
            layersWidgetTitle = new GUIContent("Layers");
            scenesWidgetTitle = new GUIContent("Scenes");
            sortingLayersTitle = new GUIContent("Sorting Layers");
            settingsWidgetTitle = new GUIContent("Settings");
            generateForgeButton = new GUIContent("Generate");
            resetForgeButton = new GUIContent("Reset");
            removeForgeButton = new GUIContent("Remove");
            autoBuildContent = new GUIContent("Auto Build", "Should this forge run automatically in the background when it detects a change?");
            scriptLocation = new GUIContent("Script Location", "Where should the class this forge generates be exported to?");
            namespaceContent = new GUIContent("Namespace", "Which namespace (if any) should the generated class have?");
            changePathContent = new GUIContent("Change Path", "Click this button to allow you to pick a new path in the project to save the output too");
            classNameContent = new GUIContent("Class Name", "What should the name of the class that is generated be called? All invalid characters will be removed");
            enumNameContent = new GUIContent("Enum Name", "The name of the enum that is generated in the class");
            animateWidgetsContent = new GUIContent("Animate Widgets", "If true the widgets open and close with nice animations otherwise it will be instant.");
        }

    }
}
