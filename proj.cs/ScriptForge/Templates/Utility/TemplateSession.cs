using System;
using System.Collections.Generic;

namespace ScriptForge.Templates
{
    // Thrown when our session is missing a key.
    public class MissingSessionKeyException : Exception
    {
        public string key { get; protected set; }

        public MissingSessionKeyException(string key) : base()
        {
            this.key = key;
        }
    }

    /// <summary>
    /// This custom template session is used to allow us to override the default Contains Key function. The
    /// issue being that Mono has not implemented the <see cref="System.Runtime.Remoting.Messaging.CallContext"/> 
    /// class. This throws an exception as soon as it's accessed. That is fine if we have all our keys but these
    /// keys are all hidden under private variables that are created in the <see cref="BaseTemplate.Initialize"/>
    /// function. We want to be able to tell the user which key is missing to help debug faster. Pretty much what
    /// we do is just override the ContainsKey function and if it does not contain the key throw an planned exception
    /// with the name of the missing key.
    /// </summary>
    public class TemplateSession : Dictionary<string, object>, IDictionary<string, object>
    {
        /// <summary>
        /// We use this cu
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IDictionary<string, object>.ContainsKey(string key)
        {
            if(base.ContainsKey(key))
            {
                // We are fine
                return true;
            }
            // We want to throw an exception
            throw new MissingSessionKeyException(key);
        }
    }
}
