using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptForge
{
    /// <summary>
    /// Putting this attribute above a widget means that it will be displayed under the
    /// In Development/Widget context menu and will have a tag displayed on it. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class InDevelopmentAttribute : Attribute
    {
    }
}
