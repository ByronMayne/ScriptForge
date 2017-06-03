using System.IO;
using System.Collections.Generic;

namespace ScriptForge.Templates
{
    /// <summary>
    /// Represents a file in a resources path in Unity.  
    /// </summary>
    public class ResourceItem : ResourceNode
    {
        private string m_Extension; 

        public string safeName { get; protected set; }
        public string extension
        {
            get { return m_Extension; }
            protected set { m_Extension = value; }
        }

        public ResourceItem(string fileName)
        {
            extension = Path.GetExtension(fileName);
            name = Path.GetFileNameWithoutExtension(fileName); 
            safeName = name.Replace(" ", "_");
        }

        public override IEnumerator<ResourceNode> GetEnumerator()
        {
            yield return this; 
        }
    }
}
