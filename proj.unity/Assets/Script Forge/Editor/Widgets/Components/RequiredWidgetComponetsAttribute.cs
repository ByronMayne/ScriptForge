using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptForge.Widgets.Components
{
    [AttributeUsage(AttributeTargets.Class, Inherited=true)]
    public class RequiredWidgetComponetsAttribute : Attribute
    {
        private Type[] m_RequiredTypes;

        public Type[] requiredTypes
        {
            get { return m_RequiredTypes; }
        }

        public RequiredWidgetComponetsAttribute(params Type[] requiredTypes)
        {
            m_RequiredTypes = requiredTypes;
        }
    }
}
