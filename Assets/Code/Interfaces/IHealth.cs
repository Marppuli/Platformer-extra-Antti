using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Platformer
{
	public interface IHealth
	{
		/// <summary>
		/// Returns the current health.
		/// </summary>
		int CurrentHealth { get; }

		/// <summary>
		/// Maximum health.
		/// The CurrentHealth can never exceed this.
		/// </summary>
		int MaxHealth { get; }

		/// <summary>
		/// Minimum health.
		/// The CurrentHealth can never be below this.
		/// </summary>
		int MinHealth { get; }

		/// <summary>
		/// Increases the health by the amount.
		/// Note: CurrentHealth can never exceed the MaxHealth.
		/// </summary>
		/// <param name="amount">The amount the health is increased.</param>
		void IncreaseHealth(int amount);

		/// <summary>
		/// Decreases the health by the amount.
		/// Note: CurrentHealth can never be below the MinHealth.
		/// </summary>
		/// <param name="amount">The amount the health is decreased.</param>
		bool DecreaseHealth(int amount);

		/// <summary>
		/// Resets the component to its default values.
		/// </summary>
		void Reset();
	}
}
