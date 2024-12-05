using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GA.Platformer.States
{
	/// <summary>
	/// Controls the game's global state. There should be exactly one instance of this object
	/// at any given time.
	/// </summary>
	public class GameStateManager : MonoBehaviour
	{
		#region Statics
		// The static varialbe which stores the reference to the only instance that can be created
		// from this class.
		private static GameStateManager instance;

		public static GameStateManager Instance
		{
			get
			{
				// Lazy loading. The instance is created when we need it for the first time.
				if (instance == null)
				{
					// Create the instance that we can create from this class.
					// We use Resources.Load to access correct prefab runtime
					GameStateManager prefab =
						Resources.Load<GameStateManager>(typeof(GameStateManager).Name);

					instance = Instantiate(prefab);
				}

				return instance;
			}
		}
		#endregion

		#region Fields
		private List<GameStateBase> states = new List<GameStateBase>();
		#endregion

		#region Properties
		public GameStateBase CurrentState
		{
			get;
			private set;
		}

		public GameStateBase PreviousState
		{
			get;
			private set;
		}
		#endregion

		#region Unity messages
		private void Awake()
		{
			// We need to make sure there's only one instance available at any given time.
			if (instance == null)
			{
				// This should be the one and the only instance of this class.
				instance = this;
			}
			else if (instance != this)
			{
				// We have more than one instance from this class at the same time.
				// This is illegal based on Singleton pattern's definition.
				// Destroy the second instance!
				Debug.LogWarning($"Multiple {typeof(GameStateManager).Name} instances detected!" +
					$"\nDestroying excess ones.");

				Destroy(this);
				return;
			}

			// By calling this, Unity prevents destroying this GameObject during scene (un)load.
			DontDestroyOnLoad(gameObject);

			Initialize();
		}
		#endregion

		#region Private implementation
		private void Initialize()
		{
			// Create state objects
			MainMenuState mainMenu = new MainMenuState();
			InGameState ingame = new InGameState();
			OptionsState options = new OptionsState();
			GameOverState gameOver = new GameOverState();

			states.Add(mainMenu);
			states.Add(ingame);
			states.Add(options);
			states.Add(gameOver);

#if UNITY_EDITOR // A pre-processor directive. This code block is removed from the build
			foreach(GameStateBase state in states)
			{
				string activeSceneName = SceneManager.GetActiveScene().name.ToLower();
				int index = 0;
				if (activeSceneName.StartsWith("level"))
				{
					index = int.Parse(activeSceneName.Substring(5, 1));
					activeSceneName = "level";
				}

				string sceneName = state.SceneName.ToLower();
				if (sceneName.StartsWith("level"))
				{
					sceneName = "level";
				}

				if (sceneName == activeSceneName)
				{
					ActivateFirstScene(state, index);
					break; // Early exit from the loop
				}
			}
#endif

			if (CurrentState == null)
			{
				ActivateFirstScene(mainMenu);
			}
		}

		private void ActivateFirstScene(GameStateBase first, int index = 0)
		{
			CurrentState = first;
			CurrentState.Activate(index);
		}

		private GameStateBase GetState(StateType type)
		{
			foreach (GameStateBase state in states)
			{
				if (state.Type == type)
				{
					return state;
				}
			}

			return null;
		}
#endregion

#region Public API
		/// <summary>
		/// Transitions from current state to the target state.
		/// </summary>
		/// <param name="targetStateType">The type of the target state.</param>
		/// <returns>True, if transition is legal and can be done. False otherwise.</returns>
		public bool Go(StateType targetStateType, int levelIndex = 0)
		{
			Debug.Log($"Transitioning to the {targetStateType}");

			// Check the legality of the transition
			if (!CurrentState.IsValidTarget(targetStateType))
			{
				Debug.Log($"{targetStateType} is not valid target for {CurrentState.Type}");
				return false;
			}

			// Find the state that matches the targetStateType
			GameStateBase nextState = GetState(targetStateType);
			if (nextState == null)
			{
				Debug.Log($"No state exists that represents the {targetStateType}");
				return false;
			}

			// Transition from current state to the target state
			PreviousState = CurrentState;

			CurrentState.Deactivate();
			CurrentState = nextState;
			CurrentState.Activate(levelIndex);

			return true;
		}

		/// <summary>
		/// Transitions back to the previous state.
		/// </summary>
		/// <returns>True, if the transition succeeds. False otherwise.</returns>
		public bool GoBack()
		{
			return Go(PreviousState.Type, PreviousState.LevelIndex);
		}
#endregion
	}
}
