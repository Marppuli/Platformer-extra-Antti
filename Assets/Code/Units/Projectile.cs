using GA.Platformer.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace GA.Platformer
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField]
		private int damage = 10;

		[SerializeField]
		private float speed = 1;

		[SerializeField]
		private float aliveTime = 5;

		[SerializeField]
		private float fadeTime = 1;

		private IMove mover;
		private SpriteRenderer renderer;
		private Collider2D collider;
		private Vector2 direction;
		private float aliveTimer;
		private bool isLaunched = false;

		private void Awake()
		{
			mover = GetComponent<IMove>();
			renderer = GetComponent<SpriteRenderer>();
			collider = GetComponent<Collider2D>();
		}

		private void Start()
		{
			mover.Setup(speed);
		}

		// Update is called once per frame
		void Update()
		{
			// Early exit if the projetile is not launched
			if (!isLaunched) return;

			aliveTimer -= Time.deltaTime;
			mover.Move(direction);

			if (aliveTimer <= 0)
			{
				// Timer ran out of time.
				// Fade the arrow away.
				StartCoroutine(FadeOut());
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			IHealth health = collision.GetComponentInChildren<IHealth>();
			UnitBase targetUnit = collision.GetComponentInChildren<UnitBase>();
			if (health != null)
			{
				if (!health.DecreaseHealth(damage))
				{
					// Target died
					if (targetUnit != null)
					{
						// TODO: Character should have a death effect and transitioning to GameOver should be delayed
						targetUnit.Die();
						GameStateManager.Instance.Go(StateType.GameOver);
					}
				}
				else
				{
					if (targetUnit != null)
					{
						targetUnit.ApplyDamage();
					}
				}

				Destroy(gameObject);
				// TODO: Play destroy effect
			}
		}

		private IEnumerator FadeOut()
		{
			isLaunched = false;
			collider.enabled = false;
			aliveTimer = fadeTime;

			Color color = renderer.color;
			while (aliveTimer > 0)
			{
				aliveTimer -= Time.deltaTime;
				color.a = aliveTimer / fadeTime;
				renderer.color = color;

				yield return null;
			}

			Destroy(gameObject);
		}

		public void Launch(Vector2 direction)
		{
			this.direction = direction;
			this.isLaunched = true;
			this.aliveTimer = this.aliveTime;

			// Projectile points right by default. If it is launched left, we should
			// flip the sprite
			if (direction.x < 0)
			{
				renderer.flipX = true;
			}
		}
	}
}
