using GA.Platformer.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Platformer
{
	public class Enemy : UnitBase
	{
		[SerializeField]
		private float fadeTime = 1;

		public override void Die()
		{
			Collider.enabled = false;
			Rigidbody.simulated = false;

			// Start the death effect coroutine
			StartCoroutine(DeathEffect());
		}

		// A coroutine method. This will execute over multiple frames.
		private IEnumerator DeathEffect()
		{
			Color color = Renderer.color;
			float timer = 0;
			// Fade the enemy away over some frames
			while(timer < fadeTime)
			{
				timer += Time.deltaTime;
				yield return null;

				color.a = (fadeTime - timer) / fadeTime;
				Renderer.color = color;
			}

			Destroy(gameObject);
		}
	}
}
