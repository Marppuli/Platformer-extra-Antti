using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace GA.Platformer.UI
{
	public class UIVolumeControl : MonoBehaviour
	{
		// Slider value 0 reprecents the -80dB volume. Slider value 1 reprecents
		// volume 0dB.

		private AudioMixer mixer;
		private Slider slider;
		private string parameterName;

		[SerializeField]
		private TMP_Text volumeText;
			 
		private void Awake()
		{
			slider = GetComponentInChildren<Slider>();
		}

		private void OnDestroy()
		{
			if (slider != null)
			{
				slider.onValueChanged.RemoveListener(OnSliderChanged);
			}
		}

		public void Setup(AudioMixer mixer, string parameterName)
		{
			this.mixer = mixer;
			this.parameterName = parameterName;

			// Initialize the slider with the initial volume read from the mixer
			if (this.mixer.GetFloat(this.parameterName, out float decibel))
			{
				// read the volume from mixer, let's set it to the slider
				float linear = AudioManager.ToLinear(decibel);
				SetVolume(linear);
			}

			slider.onValueChanged.AddListener(OnSliderChanged);
		}

		// Reads the value from the slider and sends it to mixer
		public void Save()
		{
			mixer.SetFloat(this.parameterName, AudioManager.ToDB(slider.value));
		}

		private void OnSliderChanged(float sliderValue)
		{
			volumeText.text = Mathf.RoundToInt(sliderValue * 100).ToString();
		}

		private void SetVolume(float linear)
		{
			slider.value = linear;
			volumeText.text = Mathf.RoundToInt(linear * 100).ToString();
		}
	}
}
