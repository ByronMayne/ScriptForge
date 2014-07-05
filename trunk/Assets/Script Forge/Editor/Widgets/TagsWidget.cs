using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEditor;
using System.Linq;

namespace ScriptForge
{
    public class TagsWidget : ForgeWidget
    {
         /// <summary>
        /// The constructor we use to build our Forge items. 
        /// </summary>
        /// <param name="Name">This is the name that will showup on the foldout at the top.</param>
        /// <param name="Tooltip">This is the message that the user will see when they put their mouse over the foldout.</param>
        /// <param name="Height">How tall will the editor box be when fully opened?</param>
        public TagsWidget() : base()
        {
            _OnGenerateAll += OnGenerate;
        }

        /// <summary>
        /// This is the deconstructor. It's only used to unsubscribe our OnGenerate method from the static delegate. (It will cause errors if you don't);
        /// </summary>
		public override void Destroy()
        {
			base.Destroy();

            _OnGenerateAll -= OnGenerate;
        }

        public override void GenerateCode()
        {
			string[] info = UnityEditorInternal.InternalEditorUtility.tags;

			if( _lastSourceInfo != null )
			{
				if( _lastSourceInfo.SequenceEqual(info) )
					return; 
				else
					_lastSourceInfo = info; 
			}
			else
				_lastSourceInfo = info;


            // Build the generator with the class name and data source.
			TagsGenerator generator = new TagsGenerator(_scriptName, info, _namespace );

            // Generate output (class definition).
            var classDefintion = generator.TransformText();

            var outputPath = Path.Combine(Application.dataPath + _buildPath, _scriptName + ".cs");

            try
            {
                // Save new class to assets folder.
                File.WriteAllText(outputPath, classDefintion);

                // Refresh assets.
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.Log("An error occurred while saving file: " + e);
            }
        }

		protected override string WidgetIcon ()
		{
			return sf_FontAwesome.fa_Bookmark.ToString();
		}

		protected override GUIContent Description ()
		{
			return sf_Descriptions.DESCRIPTION_TAGS_WIDGET;
		}
    } 
}
