using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour {
	public static VideoController ME;
    
	public VideoPlayer video;
	public RenderTexture renderTexture;
	public RawImage renderImage;

	private bool mToPlay;
	private UnityAction<Texture2D> mThumbnailCallback;

	private void Awake()
	{
		ME = this;

		var screenSize = renderImage.transform.parent.GetComponent<RectTransform>().rect.size;
        renderImage.rectTransform.sizeDelta = new Vector2(screenSize.y, screenSize.x);
        renderImage.gameObject.SetActive(false);

        video.prepareCompleted += PrepareCompleted;
        video.loopPointReached += LoopPointReached;
	}

    public void GetVideoThumbnail(string url, UnityAction<Texture2D> callback)
	{
		mThumbnailCallback = callback;
		mToPlay = false;
		video.url = url;
		video.Prepare();
	}

    public void PlayVideo(string url)
	{
		mToPlay = true;
		video.url = url;
        video.Prepare();
	}
    
	private void PrepareCompleted(VideoPlayer source)
	{
		renderImage.gameObject.SetActive(mToPlay);
		source.Play();
		if(!mToPlay)
		{
			RenderTexture.active = renderTexture;
			var texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
			texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
			texture2D.Apply();

			mThumbnailCallback.Invoke(texture2D);
		}
	}

	private void LoopPointReached(VideoPlayer source)
	{
		CloseVideo();
	}
 
    public void CloseVideo()
	{
		renderImage.gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		ME = null;
	}
}
