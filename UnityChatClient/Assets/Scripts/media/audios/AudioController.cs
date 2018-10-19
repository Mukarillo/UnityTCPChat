using System;
using UnityEngine;

public class AudioController : MonoBehaviour
{   
	private AudioRecorder recorder;
       
	public void StartRecording()
    {
		recorder = new AudioRecorder();
        recorder.StartRecording(OnFinishRecording);
    }

    public void EndRecording()
    {
		recorder.EndRecording();
    }

	private void OnFinishRecording(AudioClip clip)
    {
        Debug.Log("OnFinishRecording ");
		var uploader = new SaveToServer(this, clip, "audio_" + ChatClient.ME.userName + DateTime.Now.ToString("yyyyMMddHHmmss"), OnSucceedUploading, OnFailedUploading);
    }

    private void OnSucceedUploading(string fileName)
    {
		Debug.Log("OnSucceedUploading " + fileName);
		ChatClient.ME.SendMessageToServer(Constants.AUDIO_MESSAGE + FileServerInfo.AUDIO_FOLDER_URL + fileName);
    }

    private void OnFailedUploading(string error)
    {
        Debug.Log("OnFailedUploading " + error);
    }
}
