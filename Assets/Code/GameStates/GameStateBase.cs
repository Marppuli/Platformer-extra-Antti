using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Load/Unload functionality for scenes

namespace GA.Platformer.States
{
	// An abstract class. This means that we can't instantiate objects directly from this class.
	// Instead, we must derive at least one non-abstract class from this and use that instead.
	public abstract class GameStateBase
	{
		private int levelIndex = 0;

		// List of legal target state types for this state
		private List<StateType> targetStates = new List<StateType>();

		// An abstact property. The get accessor has to be implemented in a child class
		public abstract string SceneName { get; }

		public abstract StateType Type { get; }

		public virtual bool IsAdditive { get { return false; } }

		public virtual int LevelIndex 
		{ 
			get { return levelIndex; }
			protected set { levelIndex = value; }
		}

		/// <summary>
		/// A default constructor. This is called when the object is created without any parameters.
		/// Compiler will create this if we don't declare any constructors.
		/// This is safe since the class is not derived from MonoBehaviour.
		/// </summary>
		protected GameStateBase()
		{
		}

		protected void AddTargetState(StateType targetStateType)
		{
			if (!targetStates.Contains(targetStateType))
			{
				targetStates.Add(targetStateType);
			}
		}

		protected void RemoveTargetState(StateType targetStateType)
		{
			targetStates.Remove(targetStateType);
		}

		/// <summary>
		/// Activates the state. Loads the related scene as well.
		/// </summary>
		/// <param name="forceLoad">Forces the scene (re)load.</param>
		public virtual void Activate(int levelIndex = 0, bool forceLoad = false)
		{
			// The scene loading
			// Load the target scene if it's not loaded yet
			// The reference to currently loaded scene
			Scene currentScene = SceneManager.GetActiveScene();
			if (forceLoad || currentScene.name.ToLower() != SceneName.ToLower())
			{
				// The target scene is not loaded yet. Let's load it.
				// One line if-else statement + variable assignment.
				// Syntax: variable = condition ? true case value : false case value;
				LoadSceneMode mode = IsAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;

				// Handles the scene loading
				SceneManager.LoadScene(SceneName, mode);
			}
		}

		public virtual void Deactivate()
		{
			// Unload the level if necessary
			if (IsAdditive)
			{
				SceneManager.UnloadSceneAsync(SceneName);
			}
		}

		/// <summary>
		/// Checks if the targetStateType is a valid for this state.
		/// </summary>
		/// <param name="targetStateType">Type of the transition target.</param>
		/// <returns>True, if targetStateType is a valid target. False otherwise.</returns>
		public bool IsValidTarget(StateType targetStateType)
		{
			foreach(StateType stateType in targetStates)
			{
				if (stateType == targetStateType)
				{
					return true;
				}
			}

			return false;
		}
	}
}
