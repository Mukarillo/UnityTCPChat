using System.Collections;
using UnityEngine;

public class RetieveFromServer<T>
{
	public delegate void OnFileSucceedRetrieving(T file);
	public delegate void OnFileFailedRetrieving(string error);

	private OnFileSucceedRetrieving mOnFileSucceedRetrieving;
	private OnFileFailedRetrieving mOnFileFailedRetrieving;

	public RetieveFromServer(MonoBehaviour monoref, string url, OnFileSucceedRetrieving onSucceedRetrieving, OnFileFailedRetrieving onFailedRetrieving)
    {
		mOnFileSucceedRetrieving = onSucceedRetrieving;
		mOnFileFailedRetrieving = onFailedRetrieving;

		monoref.StartCoroutine(InternalRetieveFromServer(url));
    }

	private IEnumerator InternalRetieveFromServer(string url)
	{
		WWW www = new WWW(url);
        yield return www;
        
		if(!string.IsNullOrEmpty(www.error))
		{
			mOnFileFailedRetrieving(www.error);
			yield break;
		}

		var tType = typeof(T);     
		if(tType == typeof(AudioClip))
		{
			object clip = www.GetAudioClip();
			mOnFileSucceedRetrieving((T)clip);
		}

		www.Dispose();
		www = null;
	}
}
