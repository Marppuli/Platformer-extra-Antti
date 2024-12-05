using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Platformer
{
	/// <summary>
	/// This interface should be implemented if an object can be interacted with.
	/// </summary>
    public interface IInteractable
    {
		/// <summary>
		/// Should be called when the object is interacted with.
		/// </summary>
		/// <returns>True, if interaction succeeded. False otherwise.</returns>
		bool Interact(out List<IItem> loot);
    }
}
