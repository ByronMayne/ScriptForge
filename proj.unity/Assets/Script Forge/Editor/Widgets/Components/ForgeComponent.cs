using System;
using UnityEngine; 
using System.Text;
using System.Collections.Generic;

namespace ScriptForge.Widgets.Components
{
	[Serializable]
	public abstract class ForgeComponent : ScriptableObject 
	{
		/// <summary>
		/// Allows this component to add data to the session. 
		/// </summary>
		/// <param name="session">Session.</param>
		public abstract void PopulateSession(IDictionary<string, object> session);  

		/// <summary>
		/// Invoked when we are creating a hash for the parent Widget. 
		/// </summary>
		public abstract void PopulateHashBuilder(StringBuilder hashInput);

		/// <summary>
		/// Invoked when this component can draw it's content.
		/// </summary>
		/// <param name="style">Style.</param>
		public abstract void DrawContent(ScriptForgeStyles style);

		/// <summary>
		/// Invoked when the user has requested to reset the widget.
		/// </summary>
		public virtual void OnReset()
		{
            // Nothing to do here.
		}
	}
}
