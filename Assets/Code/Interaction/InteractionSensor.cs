using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Platformer
{
	public class InteractionSensor : Sensor
	{
		public IInteractable IntersectingObject
		{
			get;
			private set;
		}

		protected override void OnTriggerEnter2D(Collider2D collision)
		{
			base.OnTriggerEnter2D(collision);

			IntersectingObject = collision.GetComponent<IInteractable>();
		}

		protected override void OnTriggerExit2D(Collider2D collision)
		{
			base.OnTriggerExit2D(collision);

			// Do this when the last interaction ends.
			// ! (not operator) inverts the bool value
			if (!IsActive)
			{
				IntersectingObject = null;
			}
		}
	}
}
