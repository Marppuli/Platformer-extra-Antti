using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Platformer
{
	/// <summary>
	/// A base class for any character in the game.
	/// All common functionality should be defined here.
	/// </summary>
	public abstract class UnitBase : MonoBehaviour
	{
		// TODO: Take damage
		// TODO: Die
		[SerializeField]
		private float speed = 1;

		[SerializeField]
		private float jumpHeight = 1;

		[SerializeField]
		private Sensor groundSensor;

		[SerializeField]
		private ParticleSystem damageEffect;

		// A mover reference. Used to move the character.
		private IMove mover;

		// Character's animator. It controls movement animations.
		private Animator animator;

		private new Rigidbody2D rigidbody;

		private new SpriteRenderer renderer;

		private new AudioSource audio;

		public float Speed { get { return speed; } }
		public float JumpHeight { get { return jumpHeight; } }
		public Sensor GroundSensor { get { return groundSensor; } }
		public IMove Mover { get { return mover; } }
		public Rigidbody2D Rigidbody { get { return rigidbody; } }
		public Animator Animator { get { return animator; } }
		public SpriteRenderer Renderer { get { return renderer; } }
		public Collider2D Collider { get; private set; }

		protected virtual void Awake()
		{
			mover = GetComponent<IMove>();
			if (mover == null)
			{
				Debug.LogError("Can't find a component which implements the IMove interface!");
			}

			Collider = GetComponent<Collider2D>();

			if (GroundSensor == null)
			{
				Debug.LogError("Ground sensor is missing! Grounding checks won't work!");
			}

			animator = GetComponent<Animator>();

			rigidbody = GetComponent<Rigidbody2D>();

			renderer = GetComponent<SpriteRenderer>();

			audio = GetComponent<AudioSource>();
		}

		protected virtual void Start()
		{
			Mover.Setup(Speed);
		}

		protected virtual void Update()
		{
		}

		public virtual void Die()
		{
			// The default implementation just destroys the gameObject.
			Destroy(gameObject);
		}

		/// <summary>
		/// Plays any damage effects, sounds etc.
		/// </summary>
		public void ApplyDamage()
		{
			if (damageEffect != null)
			{
				// With withChildren = true, all child particle systems will be played as well.
				damageEffect.Play(withChildren: true);
			}

			if (audio != null)
			{
				AudioManager.PlayClip(audio, Config.SoundEffect.EnemyHit);
			}
		}
	}
}
