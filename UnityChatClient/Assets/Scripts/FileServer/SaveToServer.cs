using System.Collections;
using UnityEngine;

public class SaveToServer
{   
	public delegate void OnFileSucceedUploading(string fileName);
	public delegate void OnFileFailedUploading(string error);

	private OnFileSucceedUploading mOnFileSucceedUploading;
	private OnFileFailedUploading mOnFileFailedUploading;

	public SaveToServer(MonoBehaviour monoref, AudioClip clip, string fileName, OnFileSucceedUploading onSucceedUploading, OnFileFailedUploading onFailedUploading)
    {
		mOnFileSucceedUploading = onSucceedUploading;
		mOnFileFailedUploading = onFailedUploading;

		monoref.StartCoroutine(InternalSaveToServer(
			                   WavUtility.FromAudioClip(clip),
		                       fileName + ".wav",
                               "audio/wav",
			                   "audioupload.php"));
    }

	private IEnumerator InternalSaveToServer(byte[] data, string fileName, string mimeType, string phpServerFile)
	{
		WWWForm form = new WWWForm();
		form.AddBinaryData("file", data, fileName, mimeType);

		WWW www = new WWW(FileServerInfo.BASE_URL + phpServerFile, form);
		yield return www;

		if (!string.IsNullOrEmpty(www.error))
			mOnFileFailedUploading(www.error);
		else
			mOnFileSucceedUploading(fileName);

		www.Dispose();
		www = null;
	}
}
