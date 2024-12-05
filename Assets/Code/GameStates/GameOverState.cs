using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Platformer.States
{
	public class GameOverState : GameStateBase
	{
		public override string SceneName { get { return "GameOver"; } }

		public override StateType Type { get { return StateType.GameOver; } }

		public GameOverState() : base()
		{
			AddTargetState(StateType.MainMenu);
			AddTargetState(StateType.InGame);
		}
	}
}
