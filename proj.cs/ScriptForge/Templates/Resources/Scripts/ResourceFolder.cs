using System.IO;
using System.Collections.Generic;

namespace ScriptForge.Templates
{
    /// <summary>
    /// Represents a folder in a resources path in Unity.  
    /// </summary>
    public class ResourceFolder : ResourceNode
    {
        private List<ResourceNode> m_Children;

        public List<ResourceNode> children
        {
            get { return m_Children; }
        }

        public ResourceFolder(string name)
        {
            m_Children = new List<ResourceNode>();
            this.name = name.Replace(" ", "_");
        }

        public void Add(string path)
        {
            // Split our path
            string[] segments = path.Split('/');
            // Add it
            AddChild(segments);
        }

        public override IEnumerator<ResourceNode> GetEnumerator()
        {
            yield return this;
            for(int i = 0; i < m_Children.Count; i++)
            {
                yield return m_Children[i];
            }
        }

        public override ResourceNode GetOrCreateChild(string name)
        {
            for (int i = 0; i < m_Children.Count; i++)
            {
                if (m_Children[i].name.Equals(name, System.StringComparison.Ordinal))
                {
                    return m_Children[i];
                }
            }

            ResourceNode newNode = null;
            bool isFile = Path.HasExtension(name);

            if(isFile)
            {
                newNode = new ResourceItem(name);
            }
            else
            {
                newNode = new ResourceFolder(name);
            }
            m_Children.Add(newNode);
            return newNode;
        }

        private void AddChild(string[] segments)
        {
            ResourceNode current = this;
            for (int i = 0; i < segments.Length; i++)
            {
                current = current.GetOrCreateChild(segments[i]);
            }
        }
    }
}
