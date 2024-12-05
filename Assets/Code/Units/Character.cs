using GA.Platformer.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GA.Platformer
{
	public class Character : UnitBase
	{
		public const string AttackParam = "Attack";
		public const string AnimStateParam = "AnimState";
		public const string AirSpeedParam = "AirSpeedY";
		public const string GroundedParam = "Grounded";
		public const string JumpParam = "Jump";

		[SerializeField]
		private InteractionSensor interactionSensor;

		[SerializeField]
		private UnitSensor enemySensor;

		[SerializeField]
		private float weightLimit = 1;

		[SerializeField, Tooltip("The damage player causes to enemies")]
		private int damage = 10;

		// Member variable. It is owned by the object itself.
		// Can be used from any method in this class.
		// TODO: Decide wether to keep the input or not
		private Vector2 input;

		private bool isJumping = false; // For updating the animator

		private bool isAttacking = false; // For updating the animator

		public Inventory Inventory { get; private set; }

		#region Unity Messages
		protected override void Awake()
		{
			base.Awake();

			if (interactionSensor == null)
			{
				Debug.LogError("Interaction sensor is missing! Interactions won't work!");
			}

			// Player's inventory
			Inventory = new Inventory(weightLimit);
			Inventory.Load();
		}

		protected override void Update()
		{
			base.Update();
			UpdateAnimator();
		}

		private void OnApplicationQuit()
		{
			Inventory.ClearSave();
		}
		#endregion

		private void FlipSensor(Sensor sensor)
		{
			Vector3 position = sensor.transform.localPosition;
			position.x *= -1;
			sensor.transform.localPosition = position;
		}

		private void UpdateAnimator()
		{
			if (Rigidbody.velocity.x < -Mathf.Epsilon)
			{
				Renderer.flipX = true;
				if (interactionSensor.transform.localPosition.x > 0)
				{
					FlipSensor(interactionSensor);
					FlipSensor(enemySensor);
					FlipSensor(GroundSensor);
				}
			}
			else if (Rigidbody.velocity.x > Mathf.Epsilon)
			{
				Renderer.flipX = false;
				if (interactionSensor.transform.localPosition.x < 0)
				{
					FlipSensor(interactionSensor);
					FlipSensor(enemySensor);
					FlipSensor(GroundSensor);
				}
			}

			// Are we grounded?
			Animator.SetBool(GroundedParam, GroundSensor.IsActive);

			if (isJumping)
			{
				Animator.SetTrigger(JumpParam);
				isJumping = false;
			}

			// For jumping and falling
			Animator.SetFloat(AirSpeedParam, Rigidbody.velocity.y);

			// Run
			if (Mathf.Abs(Rigidbody.velocity.x) > Mathf.Epsilon)
			{
				Animator.SetInteger(AnimStateParam, 1);
			}
			else // Idle
			{
				Animator.SetInteger(AnimStateParam, 0);
			}

			// Attack
			if (isAttacking)
			{
				// Randomize the attack animation
				int attackIndex = Random.Range(0, 3) + 1;
				string attackString = AttackParam + attackIndex;
				Animator.SetTrigger(attackString);
				isAttacking = false;
			}
		}

		public void Move(InputAction.CallbackContext context)
		{
			// Read the user input using InputSystem's callback context
			input = context.ReadValue<Vector2>();
			Mover.Move(input);
		}

		public void Jump(InputAction.CallbackContext context)
		{
			// With and, both sides of the statement have to be true for the whole
			// statement to be true.
			if (GroundSensor.IsActive && context.phase == InputActionPhase.Performed)
			{
				Mover.Jump(JumpHeight);
				isJumping = true;
			}
		}

		public void Interact(InputAction.CallbackContext context)
		{
			if (interactionSensor.IsActive && context.phase == InputActionPhase.Performed)
			{
				// If this throws a NullReferenceException, there's a bug in our
				// interaction sensor's code.
				if (interactionSensor.IntersectingObject.Interact(out List<IItem> items))
				{
					foreach (IItem item in items)
					{
						if (Inventory.AddItem(item))
						{
							Debug.Log("Item " + item.Name + " added");
						}
					}
				}
			}
		}

		public void Attack(InputAction.CallbackContext context)
		{
			// Early exit conditions:
			// - Is the pointer on a UI object?
			// - Is the input phase something else than performed?
			if (EventSystem.current.IsPointerOverGameObject()) return;
			if (context.phase != InputActionPhase.Performed) return;

			isAttacking = true;
			if (enemySensor.IsActive)
			{
				Debug.Log($"Attacking {enemySensor.ActiveUnit}");
			}
		}

		public void Pause(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
			{
				GameStateManager.Instance.Go(StateType.Options);
			}
		}

		public void SaveInventory()
		{
			Inventory.Save();
		}
	}
}
