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
		}else if (mType == typeof(MovieTexture))
		{
		    var movie = www.GetMovieTexture();
			while (!movie.isReadyToPlay)
				yield return null;
			object omovie = movie;
		    mOnFileSucceedRetrieving((T) omovie);
		}

		www.Dispose();
		www = null;
	}
}
