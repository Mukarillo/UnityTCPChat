using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlayButton : Button {

	public AudioClip audioClip;

	public enum AudioPlayButtonState
	{
		IDLE,
		PLAYING,
        LOADING
	}

	private ChatBubble mChatBubble;   
	private bool mRotate;

	public void Initiate(string url, ChatBubble chatBubble)
	{
		mChatBubble = chatBubble;

		ChangeState(AudioPlayButtonState.LOADING);
		mRotate = true;

		Debug.Log("Initiating download: " + url);
		var serverRetriever = new RetieveFromServer<AudioClip>(this, url, OnSucceedRetrieving, OnFailedRetrieving);
	}

	private void OnFailedRetrieving(string error)
	{
		Debug.Log("OnFailedRetrieving " + error);
		mChatBubble.ForceMessage("Failed to load audio.", new Color(255, 68, 68));
		gameObject.SetActive(false);
	}

	private void OnSucceedRetrieving(AudioClip file)
	{
		Debug.Log("OnSucceedRetrieving");
		mRotate = false;
		ChangeState(AudioPlayButtonState.IDLE);
		      
		audioClip = file;
	}

	public void ChangeState(AudioPlayButtonState state)
	{
		interactable = state != AudioPlayButtonState.LOADING;
		((Image)targetGraphic).sprite = 
			  state == AudioPlayButtonState.IDLE ? ChatClient.ME.playAudioIcon
			: state == AudioPlayButtonState.PLAYING ? ChatClient.ME.pauseAudioIcon
			: ChatClient.ME.loadingIcon;
		targetGraphic.transform.rotation = Quaternion.identity;
	}   
    
    public void ToggleAudio()
	{
		AudioPlayer.ME.PlayAudio(this);
	}

	private void Update()
	{
		if (!mRotate)
			return;

		targetGraphic.transform.Rotate(Vector3.forward * Time.deltaTime * 100);
	}
}
