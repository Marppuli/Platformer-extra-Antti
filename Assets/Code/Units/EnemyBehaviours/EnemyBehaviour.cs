using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Platformer
{
	public abstract class EnemyBehaviour : MonoBehaviour
	{
		private Enemy enemy;

		public Enemy Enemy { get { return enemy; } }

		protected virtual void Awake()
		{
			enemy = GetComponent<Enemy>();
		}
	}
}
