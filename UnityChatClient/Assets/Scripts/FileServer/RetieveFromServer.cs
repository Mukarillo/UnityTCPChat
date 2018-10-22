using System;
using System.Collections;
using UnityEngine;

public class RetieveFromServer<T>
{
	public delegate void OnFileSucceedRetrieving(T file);
	public delegate void OnFileFailedRetrieving(string error);

	private OnFileSucceedRetrieving mOnFileSucceedRetrieving;
	private OnFileFailedRetrieving mOnFileFailedRetrieving;

    private Type mType => typeof(T);

	public RetieveFromServer(MonoBehaviour monoref, string url, OnFileSucceedRetrieving onSucceedRetrieving, OnFileFailedRetrieving onFailedRetrieving)
    {
		mOnFileSucceedRetrieving = onSucceedRetrieving;
		mOnFileFailedRetrieving = onFailedRetrieving;

		if(mType == typeof(string))
		{
			object ourl = url;
			mOnFileSucceedRetrieving((T)ourl);
			return;
		}

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
 
		if(mType == typeof(AudioClip))
		{
			object clip = www.GetAudioClip();
			mOnFileSucceedRetrieving((T)clip);
		}else if (mType == typeof(Texture2D))
		{
		    object texture = www.texture;
		    mOnFileSucceedRetrieving((T) texture);
		}

		www.Dispose();
		www = null;
	}
}
