using System;
using System.Collections;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
	public static AudioPlayer ME;
	public AudioSource audioSource;

	private Sound mCurrentButton;

	private void Awake()
	{
		ME = this;      
	}

	public void PlayAudio(Sound button)
	{
		if (button != mCurrentButton)
			InternalPlayAudio(button);
		else
			StopPlaying();
	}

	private void InternalPlayAudio(Sound button)
	{
		mCurrentButton = button;

		mCurrentButton.ChangeState(MediaComponent.MediaButtonState.PLAYING);
		audioSource.clip = mCurrentButton.file;
		audioSource.Play();

		StartCoroutine(WaitAudioFile(mCurrentButton.file.length));
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
		mCurrentButton.ChangeState(MediaComponent.MediaButtonState.IDLE);
		mCurrentButton = null;
	}

	private void OnDestroy()
	{
		ME = null;
	}
}
