using System;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using AnimatorController = UnityEditor.Animations.AnimatorController;
using AnimatorControllerParameter = UnityEngine.AnimatorControllerParameter;
using AnimatorControllerLayer = UnityEditor.Animations.AnimatorControllerLayer;

namespace ScriptForge
{
    [Serializable]
    public struct AnimatorData
    {
        [SerializeField]
        public string className;

        [SerializeField]
        public AnimatorController controller;
    }

    [Serializable, InDevelopment]
    public class AnimationsWidget : ForgeWidget
    {
        [SerializeField]
        private bool m_GenerateClipNames;

        [SerializeField]
        private bool m_GenerateLayerNames;

        [SerializeField]
        private bool m_GenerateParamaters;

        [SerializeField]
        private List<AnimatorData> m_AnimatorsData;



        // Editor Drawing. 
        private ReorderableList m_DataReorderableList;

        public override GUIContent label
        {
            get
            {
                return ScriptForgeLabels.animationsWidgetTitle;
            }
        }

        public override string iconString
        {
            get
            {
                return FontAwesomeIcons.MOVIE;
            }
        }

        /// <summary>
        /// Gets the sorting order for this widget.
        /// </summary>
        public override int sortOrder
        {
            get
            {
                return LayoutSettings.ANIMATION_WIDGET_SORT_ORDER;
            }
        }

        /// <summary>
        /// The default name of this script
        /// </summary>
        protected override string defaultName
        {
            get
            {
                return "Animations";
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (m_AnimatorsData == null)
            {
                m_AnimatorsData = new List<AnimatorData>();
            }

            m_DataReorderableList = new ReorderableList(m_AnimatorsData, typeof(AnimatorData));
            m_DataReorderableList.drawHeaderCallback += OnDrawHeader;
            m_DataReorderableList.drawElementCallback += OnDrawElement;
            m_DataReorderableList.onAddCallback += OnAnimatorAdded;
            m_DataReorderableList.elementHeight = EditorGUIUtility.singleLineHeight;
        }



        private void OnDrawHeader(Rect rect)
        {
            rect.width /= 2.0f;
            GUI.Label(rect, "Class Name");
            rect.x += rect.width;
            GUI.Label(rect, "Animator");
        }

        private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            Rect classNameRect = rect;
            classNameRect.width /= 2.0f;
            Rect animatorRect = classNameRect;
            animatorRect.x += animatorRect.width;
            AnimatorData data = m_AnimatorsData[index];
            data.className = EditorGUILayoutEx.ClassNameTextField(classNameRect, data.className, "ClassName");
            EditorGUI.BeginChangeCheck();
            {
                data.controller = EditorGUI.ObjectField(animatorRect, data.controller, typeof(AnimatorController), false) as AnimatorController;
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (string.IsNullOrEmpty(data.className) && data.controller != null)
                {
                    data.className = data.controller.name;
                }
            }
            m_AnimatorsData[index] = data;
        }

        private void OnAnimatorAdded(ReorderableList list)
        {
            AnimatorData data = new AnimatorData();
            m_AnimatorsData.Add(data);
        }

        /// <summary>
        /// Invoked to allow us to draw are GUI content for this forge.
        /// </summary>
        protected override void DrawWidgetContent(ScriptForgeStyles style)
        {
            base.DrawWidgetContent(style);
            m_GenerateClipNames = EditorGUILayout.Toggle(ScriptForgeLabels.generateClipNamesLabel, m_GenerateClipNames);
            m_GenerateLayerNames = EditorGUILayout.Toggle(ScriptForgeLabels.generateLayerNamesLabel, m_GenerateLayerNames);
            m_GenerateParamaters = EditorGUILayout.Toggle(ScriptForgeLabels.generateParamatersLabel, m_GenerateParamaters);
            m_DataReorderableList.DoLayoutList();
        }

		/// <summary>
		/// Invoked when we are required to build a new hash code for our forge. All
		/// unique content should be converted to string and appending to the builder. 
		/// </summary>
		protected override void PopulateHashBuilder(System.Text.StringBuilder hashBuilder)
		{
			base.PopulateHashBuilder(hashBuilder);
			hashBuilder.Append(m_GenerateClipNames);
			hashBuilder.Append(m_GenerateLayerNames);
			hashBuilder.Append(m_GenerateParamaters);
		}

        /// <summary>
        /// Invoked when this widget should generate it's content. 
        /// </summary>
        public override void OnGenerate(bool forced)
        {
            if (ShouldRegnerate() || forced)
            {
                // Invoke the base.
                base.OnGenerate(forced);
                // Build the template
                AnimationsTemplate generator = new AnimationsTemplate();
                // Populate it's session
                CreateSession(generator);
                // Write it to disk. 
                WriteToDisk(generator);
            }
        }

        protected override void PopulateSession(IDictionary<string, object> session)
        {
            base.PopulateSession(session);
            session["m_GenerateClipNames"] = m_GenerateClipNames;
            session["m_GenerateLayerNames"] = m_GenerateLayerNames;
            session["m_GenerateParamaters"] = m_GenerateParamaters;
            session["m_EnumName"] = string.Empty;

            Dictionary<string, List<AnimatorController>> animatorControllerMap = new Dictionary<string, List<AnimatorController>>();

            for (int i = 0; i < m_AnimatorsData.Count; i++)
            {
                AnimatorData data = m_AnimatorsData[i];
                if (!animatorControllerMap.ContainsKey(data.className))
                {
                    animatorControllerMap[data.className] = new List<AnimatorController>();
                }

                if (data.controller != null)
                {
                    if (animatorControllerMap[data.className].Contains(data.controller))
                    {
                        Debug.LogError("The Animator Controller '" + data.controller.name + "' is already defined.");
                    }
                    else
                    {
                        animatorControllerMap[data.className].Add(data.controller);
                    }
                }
            }
            session["m_AnimatorControllerMap"] = animatorControllerMap;
        }
    }
}