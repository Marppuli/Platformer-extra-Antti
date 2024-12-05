using GA.Platformer.States;
using UnityEngine;
using UnityEngine.Audio;

namespace GA.Platformer.UI
{
	public class UIOptions : MonoBehaviour
	{
		[SerializeField]
		private UIVolumeControl masterVolume;

		[SerializeField]
		private UIVolumeControl musicVolume;

		[SerializeField]
		private UIVolumeControl sfxVolume;

		[SerializeField]
		private AudioMixer mixer;

		[SerializeField]
		private string masterVolumeName;

		[SerializeField]
		private string musicVolumeName;

		[SerializeField]
		private string sfxVolumeName;

		private void Start()
		{
			masterVolume.Setup(mixer, masterVolumeName);
			musicVolume.Setup(mixer, musicVolumeName);
			sfxVolume.Setup(mixer, sfxVolumeName);
		}

		public void Save()
		{
			Debug.Log("Save options");
			masterVolume.Save();
			musicVolume.Save();
			sfxVolume.Save();
		}

		public void Close()
		{
			GameStateManager.Instance.GoBack();
		}

		public void ExitToMenu()
		{
			GameStateManager.Instance.Go(StateType.MainMenu);
		}
	}
}
