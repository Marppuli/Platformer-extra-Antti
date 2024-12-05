using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Platformer.UI
{
	public class UIChest : MonoBehaviour
	{
		private UILootIndicator lootIndocator;

		private void Awake()
		{
			lootIndocator = GetComponentInChildren<UILootIndicator>(includeInactive: true);
		}

		/// <summary>
		/// Called when the chest is looted. Plays the open UI animation.
		/// </summary>
		/// <param name="icon">The icon which is rendered in the animation</param>
		/// <param name="count">How many objects we looted</param>
		public void Loot(Sprite icon, int count)
		{
			lootIndocator.ActivateItem(icon, count);
		}
	}
}
