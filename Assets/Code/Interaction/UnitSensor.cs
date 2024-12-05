using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Platformer
{
	public class UnitSensor : Sensor
	{
		// TODO: Refactor me! This should be able to report anything that has health
		// as valid target.
		public UnitBase ActiveUnit
		{
			get;
			private set;
		}

		protected override void OnTriggerEnter2D(Collider2D collision)
		{
			base.OnTriggerEnter2D(collision);

			ActiveUnit = collision.GetComponentInChildren<UnitBase>();
		}

		protected override void OnTriggerExit2D(Collider2D collision)
		{
			base.OnTriggerExit2D(collision);

			if (!IsActive)
			{
				ActiveUnit = null;
			}
		}
	}
}
