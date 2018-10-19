using System;
using System.Collections;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
	public static AudioPlayer ME;
	public AudioSource audioSource;

	private AudioPlayButton mCurrentButton;

	private void Awake()
	{
		ME = this;      
	}

	public void PlayAudio(AudioPlayButton button)
	{
		if (button != mCurrentButton)
			InternalPlayAudio(button);
		else
			StopPlaying();
	}

	private void InternalPlayAudio(AudioPlayButton button)
	{
		mCurrentButton = button;

		mCurrentButton.ChangeState(AudioPlayButton.AudioPlayButtonState.PLAYING);
		audioSource.clip = mCurrentButton.audioClip;
		audioSource.Play();

		StartCoroutine(WaitAudioFile(mCurrentButton.audioClip.length));
	}

	private IEnumerator WaitAudioFile(float length)
	{
		yield return new WaitForSeconds(length);
		StopPlaying();
	}

	private void StopPlaying()
	{
		StopAllCoroutines();

		audioSource.Stop();
		mCurrentButton.ChangeState(AudioPlayButton.AudioPlayButtonState.IDLE);
		mCurrentButton = null;
	}

	private void OnDestroy()
	{
		ME = null;
	}
}
