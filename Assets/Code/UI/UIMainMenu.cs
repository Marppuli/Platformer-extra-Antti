using GA.Platformer.States;
using UnityEngine;

namespace GA.Platformer.UI
{
	public class UIMainMenu : MonoBehaviour
	{
		public void StartGame()
		{
			GameStateManager.Instance.Go(StateType.InGame, levelIndex: 1);
		}

		public void OpenOptions()
		{
			GameStateManager.Instance.Go(StateType.Options);
		}

		public void Quit()
		{
			// Exits the game.
			// Won't do anything in the editor.
			Application.Quit();
		}
	}
}
