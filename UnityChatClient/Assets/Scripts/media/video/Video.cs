using UnityEngine;
using UnityEngine.UI;

public class Video : PlayableMediaComponent<string>
{
	protected override Sprite IdleSprite => AssetController.GetSprite("VideoPlayButton");
    protected override Vector2 componentSize => new Vector2(384, 216);

	private Image mVideoThumbnail;

	public override void InitiateComponent(string param, ChatBubble chatBubble)
	{
		base.InitiateComponent(param, chatBubble);

		mVideoThumbnail = Instantiate(AssetController.GetGameObject("SimpleImage"), chatBubble.mediaParent).GetComponent<Image>();
		mVideoThumbnail.gameObject.AddComponent<Button>().onClick.AddListener(OnClick);

		mImage.gameObject.GetComponent<LayoutElement>().ignoreLayout = true;
		var rt = mImage.GetComponent<RectTransform>();
		rt.anchorMax = new Vector2(0.5f, 0.5f);
		rt.anchorMin = new Vector2(0.5f, 0.5f);
		rt.pivot = new Vector2(0.5f, 0.5f);
		rt.anchoredPosition = Vector2.zero;

		rt.SetAsLastSibling();
	}
       
	protected override void OnSucceedRetrieving(string file)
	{
		base.OnSucceedRetrieving(file);
		VideoController.ME.GetVideoThumbnail(file, SetVideoThumbnail);
	}

	private void SetVideoThumbnail(Texture2D thumbnail)
	{
		TextureScale.Bilinear(thumbnail, (int)componentSize.x, (int)componentSize.y);
        
		mVideoThumbnail.sprite = Sprite.Create(thumbnail, new Rect(0, 0, thumbnail.width, thumbnail.height), Vector2.zero);
	}
       
	protected override void OnClick()
    {
		VideoController.ME.PlayVideo(file);
    }
}