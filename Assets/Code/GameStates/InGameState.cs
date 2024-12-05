using UnityEngine;

namespace GA.Platformer.States
{
	public class InGameState : GameStateBase
	{
		public override string SceneName
		{
			get { return "Level" + LevelIndex; }
		}

		public override StateType Type
		{
			get { return StateType.InGame; }
		}

		public InGameState() : base()
		{
			AddTargetState(StateType.Options);
			AddTargetState(StateType.GameOver);
			AddTargetState(StateType.InGame);
		}

		public override void Activate(int levelIndex = 0, bool forceLoad = false)
		{
			LevelIndex = levelIndex;

			// Calls the base class's implementation.
			base.Activate(levelIndex, forceLoad);

			// Unpause the game
			Time.timeScale = 1;
		}

		public override void Deactivate()
		{
			base.Deactivate();

			// Pauses the game
			Time.timeScale = 0;
		}
	}
}
