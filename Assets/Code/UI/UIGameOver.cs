using GA.Platformer.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Platformer
{
	public class UIGameOver : MonoBehaviour
	{
		public void Restart()
		{
			GameStateManager.Instance.Go(StateType.InGame);
		}

		public void BackToMenu()
		{
			GameStateManager.Instance.Go(StateType.MainMenu);
		}

		public void Quit()
		{
			Application.Quit();
		}
	}
}
