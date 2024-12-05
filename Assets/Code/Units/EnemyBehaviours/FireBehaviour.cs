using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Platformer
{
	public class FireBehaviour : EnemyBehaviour
	{
		[SerializeField]
		private Projectile projectilePrefab;

		[SerializeField]
		private float fireRate = 1;

		private float timer;

		private void Start()
		{
			SetCooldown();
		}

		private void Update()
		{
			timer -= Time.deltaTime;
			if (timer <= 0)
			{
				SetCooldown();

 				int playerIndex = LayerMask.NameToLayer("Player");
				int mask = 1 << playerIndex;

				if (Physics2D.Raycast(
					transform.position - Vector3.up * 0.5f, Vector2.left, 15, mask))
				{
					// Ray hit the player. Fire an arrow to our left
					Fire(Vector2.left);
				}
				else if (Physics2D.Raycast(
					transform.position - Vector3.up * 0.5f, Vector2.right, 15, mask))
				{
					Fire(Vector2.right);
				}
			}
		}

		private void Fire(Vector2 direction)
		{
			Projectile projectile =
				Instantiate(projectilePrefab, transform.position, Quaternion.identity);
			projectile.Launch(direction);
		}

		private void SetCooldown()
		{
			timer = 1 / fireRate;
		}	
	}
}
