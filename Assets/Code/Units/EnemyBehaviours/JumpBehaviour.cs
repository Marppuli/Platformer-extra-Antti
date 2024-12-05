using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace GA.Platformer
{
	public class JumpBehaviour : EnemyBehaviour
	{
		private enum State
		{
			None,
			Jumping,
			Waiting
		}

		[SerializeField]
		private float waitTime = 1;

		private State state;
		private Vector2 direction = Vector2.left;
		private float waitTimer = 0;
		private float jumpTimer = 0.1f;

		private void Update()
		{
			switch (state)
			{
				case State.None:
					// Start the jumping behaviour.
					if (Jump())
					{
						// Update the jump direction
						direction *= -1;
						jumpTimer = 0.1f; // HACK: Refactor this at some point
						// The enemy jumped.
						state = State.Jumping;
					}
					break;
				case State.Waiting:
					waitTimer -= Time.deltaTime;
					if (waitTimer <= 0)
					{
						// Wait timer ran out of time
						state = State.None;
					}
					break;
				case State.Jumping:
					if (jumpTimer > 0)
					{
						jumpTimer -= Time.deltaTime;
						return;
					}

					if(!Enemy.GroundSensor.IsActive)
					{
						// The enemy is in the air. Let's move them either left or right
						Enemy.Mover.Move(direction);
					}
					else
					{
						Enemy.Mover.Move(Vector2.zero);
						waitTimer = waitTime;
						state = State.Waiting;
					}
					break;
				default:
					// Illegal case! Let's reset state.
					state = State.None;
					break;
			}
		}

		private bool Jump()
		{
			if (Enemy.GroundSensor.IsActive)
			{
				// The enemy can jump since it is grounded.
				Enemy.Mover.Jump(Enemy.JumpHeight);
				return true;
			}
			return false;
		}
	}
}