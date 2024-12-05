using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GA.Platformer.UI;

namespace GA.Platformer
{
	[RequireComponent(typeof(Animator))]
	public class Chest : MonoBehaviour, IInteractable
	{
		public const string ChestOpenParameter = "IsOpen";

		[SerializeField]
		private Item[] items;

		[SerializeField]
		private Sprite multipleItemsIcon;

		private UIChest ui;

		private Inventory inventory;

		// The Mecanim animation controller
		private Animator animator;

		// Chest open audio effect
		private AudioSource openAudio;

		private bool isLooted = false;

		private void Awake()
		{
			inventory = new Inventory(float.PositiveInfinity);

			foreach(Item item in items)
			{
				inventory.AddItem(item);
			}

			// Works when the animator is attached to the same GameObject than the Chest.
			animator = GetComponent<Animator>();

			openAudio = GetComponent<AudioSource>();

			ui = GetComponentInChildren<UIChest>(includeInactive: true);
		}

		public bool Interact(out List<IItem> loot)
		{
			if (!isLooted)
			{
				loot = inventory.GetItems();
				isLooted = true;

				// This runs chest's open animation
				animator.SetBool(ChestOpenParameter, true);

				// Play the sound
				if (openAudio != null)
				{
					AudioManager.PlayClip(openAudio, Config.SoundEffect.ChestOpen);
				}

				Sprite icon;
				int count = 0;
				if (loot.Count == 1)
				{
					icon = loot[0].Icon;
					count = loot[0].Count;
				}
				else if (loot.Count > 1)
				{
					icon = multipleItemsIcon;
					foreach(Item item in loot)
					{
						count += item.Count;
					}
				}
				else
				{
					// No loot!
					return false;
				}

				ui.Loot(icon, count);

				return true;
			}

			loot = null; // Nothing to loot, let's set it null
			return false;
		}
	}
}
