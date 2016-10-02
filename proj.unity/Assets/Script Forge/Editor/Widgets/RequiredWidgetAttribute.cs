using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptForge
{
    /// <summary>
    /// Putting this attribute above a widget means that one instance of this must exists at all times.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiredWidgetAttribute : Attribute
    {
    }
}
