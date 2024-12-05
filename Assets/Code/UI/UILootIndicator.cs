using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace GA.Platformer.UI
{
	public class UILootIndicator : MonoBehaviour
	{
		// The name of the animation controller parameter
		public const string IsLootedParameter = "IsLooted";

		[SerializeField]
		private Image iconImage;

		[SerializeField]
		private TMP_Text text;

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private float animationLength = 1;

		private float animationTimer = -1;

		public void ActivateItem(Sprite icon, int count)
		{
			// Set the correct sprite for the image
			iconImage.sprite = icon;
			// Set the text to indicate how many items we looted
			text.text = count.ToString();

			// By default we don't want to show the indicator. We should enable it just before the animation
			// is played.
			gameObject.SetActive(true);

			// Invoke the animation
			animator.SetBool(IsLootedParameter, true);

			// Set the animation timer
			animationTimer = animationLength;
		}

		private void Update()
		{
			// This makes sure that the animation is not played again by accident.
			if (animationTimer >= 0)
			{
				// Update the timer
				animationTimer -= Time.deltaTime;
			}
			else
			{
				// This is called just after the timer has run out of time.
				// It won't be called a second time after disabling the object because
				// Update isn't run for inactive objects.
				gameObject.SetActive(false);
			}
		}
	}
}
