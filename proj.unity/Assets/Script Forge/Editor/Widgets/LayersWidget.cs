using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;
using System.IO;
using ScriptForge.Widgets.Components;

namespace ScriptForge
{
    [System.Serializable]
    [RequiredWidgetComponets(typeof(EnumComponent))]
    public class LayersWidget : ForgeWidget
    {
        [SerializeField]
        private bool m_CreateBitwise;

        /// <summary>
        /// The label that will be shown on the title of the widget.
        /// </summary>
        public override GUIContent label
        {
            get
            {
                return ScriptForgeLabels.layersWidgetTitle;
            }
        }

        /// <summary>
        /// The string icon for FontAwesome to be shown on the title bar.
        /// </summary>
        public override string iconString
        {
            get
            {
                return FontAwesomeIcons.BARS;
            }
        }

        /// <summary>
        /// Gets the sorting order for this widget.
        /// </summary>
        public override int sortOrder
        {
            get
            {
                return LayoutSettings.LAYERS_WIDGET_SORT_ORDER;
            }
        }

        /// <summary>
        /// The default name of this script
        /// </summary>
        protected override string defaultName
        {
            get
            {
                return "Layers";
            }
        }

        /// <summary>
        /// Invoked to allow us to draw are GUI content for this forge.
        /// </summary>
        protected override void DrawWidgetContent(ScriptForgeStyles style)
        {
            base.DrawWidgetContent(style);
            m_CreateBitwise = EditorGUILayout.Toggle(ScriptForgeLabels.createBitwiseLabel, m_CreateBitwise);
        }


        /// <summary>
        /// Returns an array of all the layer names, including unset ones.
        /// </summary>
        /// <returns></returns>
        private string[] GetLayerNames()
        {
            List<string> layers = new List<string>();

            for (int i = 0; i < 32; i++)
            {
                string layerName = InternalEditorUtility.GetLayerName(i);
                layerName = layerName.Replace(' ', '_');

                layers.Add(layerName);
            }
            return layers.ToArray();
        }

		/// <summary>
		/// Invoked when we are required to build a new hash code for our forge. All
		/// unique content should be converted to string and appending to the builder. 
		/// </summary>
		protected override void PopulateHashBuilder(System.Text.StringBuilder hashBuilder)
		{
			base.PopulateHashBuilder(hashBuilder);
			// Add our layer names 
			foreach (var layer in GetLayerNames())
			{
				hashBuilder.Append(layer);
			}
		}

        /// <summary>
        /// Invoked when the user adds this widget or resets it. 
        /// </summary>
        public override void OnReset()
        {
            base.OnReset();
			for(int i = 0; i < m_Components.Count; i++)
			{
				m_Components[i].OnReset();
			}
        }

        /// <summary>
        /// Invoked when this widget should generate it's content.
        /// </summary>
        public override void OnGenerate(bool forced)
        {
            if (ShouldRegnerate() || forced)
            {
                // Build the template
                LayersTemplate generator = new LayersTemplate();
                // Populate it's session
                CreateSession(generator);
                // Write it to disk. 
                WriteToDisk(generator);
                // Invoke the base.
                base.OnGenerate(forced);
            }
        }

        /// <summary>
        /// Used to send our paths for our session.
        /// </summary>
        /// <param name="session"></param>
        protected override void PopulateSession(IDictionary<string, object> session)
        {
            // Create our base session 
            base.PopulateSession(session);
            // Get our layers
            string[] layerNames = GetLayerNames();
            // Set our session
            session["m_Layers"] = layerNames;
            session["m_CreateEnum"] = true;
            session["m_EnumName"] = "Types";
            session["m_CreateBitwise"] = m_CreateBitwise;
        }
    }
}