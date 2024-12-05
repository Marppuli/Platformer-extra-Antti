using GA.Platformer.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Platformer
{
	public class LevelChangeTrigger : MonoBehaviour
	{
		[SerializeField]
		private StateType targetState;

		[SerializeField]
		private int levelIndex;

		private void OnTriggerEnter2D(Collider2D collision)
		{
  			GameStateManager.Instance.Go(targetState, levelIndex);

			Character player = collision.GetComponent<Character>();
			if (player != null)
			{
				player.SaveInventory();
			}
		}
	}
}
