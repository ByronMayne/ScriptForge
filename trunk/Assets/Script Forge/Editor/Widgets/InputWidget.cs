using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngineInternal;
using System.Collections;
using System;
using System.Reflection;
using System.IO;

namespace ScriptForge
{
	public class InputWidget : ForgeWidget
	{
		/// <summary>
		/// The constructor we use to build our Forge items. 
		/// </summary>
		/// <param name="Name">This is the name that will showup on the foldout at the top.</param>
		/// <param name="Tooltip">This is the message that the user will see when they put their mouse over the foldout.</param>
		/// <param name="Height">How tall will the editor box be when fully opened?</param>
		public InputWidget() : base()
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


		}
		
		protected override void DrawForgeContent ()
		{

		}

		protected override string WidgetIcon ()
		{
			return sf_FontAwesome.fa_Gamepad.ToString();
		}

		protected override GUIContent Description ()
		{
			return sf_Descriptions.DESCRIPTION_INPUT_WIDGET;
		}
	}
}
