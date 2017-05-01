using UnityEditor;
using UnityEngine;

namespace ScriptForge
{
    /// <summary>
    /// This class is a made up of two parts. The first part being the output
    /// from the .tt or t4 template file. This part note the keyword 'partial' 
    /// is used as an unchanging version. In this class we are going to set up all our variables
    /// that we defined in our template.
    /// </summary>
    public partial class ScenesTemplateConstructor : ScenesTemplate
    {
        // Worlds longest constructor but all of this information
        // is required to process our T4 templates.
        public ScenesTemplateConstructor(string className,
                                         string @namespace, 
                                         string enumName,
                                         string[] scenes,
                                         bool createEnum,
                                         bool isPartialClass,
                                         bool isStaticClass,
                                         string indent,
                                         string saveLocation,
                                         string assetHash,
                                         bool enumedDefinedInClass
                                         )
        {
        }
    }
}
