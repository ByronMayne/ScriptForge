using System.Collections;
using System.Collections.Generic;

namespace ScriptForge.Templates
{
    public abstract class ResourceNode : IEnumerable<ResourceNode>
    {
        private string m_Name;

        public string name
        {
            get { return m_Name; }
            protected set { m_Name = value; }
        }

        public abstract IEnumerator<ResourceNode> GetEnumerator();

        public virtual ResourceNode GetOrCreateChild(string name)
        {
            return null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
