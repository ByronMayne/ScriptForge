using UnityEngine;
using System.Collections;
using System; 

namespace ScriptForge
{
	public partial class ScenesGenerator
	{
		/// <summary>
		/// Stores the generated class name.
		/// </summary>
		private string className;
		
		/// <summary>
		/// Stores the generated source path. 
		/// </summary>
		private string path;
		
		/// <summary>
		/// A datasource for which concrete static fields will be created on the generated class.
		/// </summary>
		private string[] source;
		
		/// <summary>
		/// The namespace that this class we belong too.
		/// </summary>
		private string NameSpace = "";

		private string Enumname = "";

		/// <summary>
		/// Initializes a new instance of the Generator class.
		/// </summary>
		/// <param name="generatedClassName"></param>
		/// <param name="source"></param>
		public ScenesGenerator(string generatedClassName, string path, string[] source, string aEnumName, string @namespace = "")
		{
			if (string.IsNullOrEmpty(generatedClassName))
			{
				throw new ArgumentException("generatedClassName");   
				
			}
			
			if (source == null)
			{
				throw new ArgumentNullException("source cannot be null!");    
			}
			
			this.className = generatedClassName;
			this.source = source;
			this.NameSpace = @namespace; 
			this.Enumname = aEnumName; 
		}
	}
}

