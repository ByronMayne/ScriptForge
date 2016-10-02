using System;

namespace ScriptForge
{
    /// <summary>
    /// Provides a constructor for the T4 template to pass the data in.
    /// </summary>
	public partial class TagsGenerator
    {
        /// <summary>
        /// Stores the generated class name.
        /// </summary>
        private string className;

        /// <summary>
        /// A datasource for which concrete static fields will be created on the generated class.
        /// </summary>
        private string[] source;

		/// <summary>
		/// The namespace that this class we belong too.
		/// </summary>
		private string NameSpace;

        /// <summary>
        /// Initializes a new instance of the Generator class.
        /// </summary>
        /// <param name="generatedClassName"></param>
        /// <param name="source"></param>
		public TagsGenerator (string generatedClassName, string[] source, string @namespace = "" )
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
        }
    }
}