using UnityEngine;
using UnityEngine.UI;

public abstract class ImageMediaComponent : MediaComponent
{
	protected Image mImage;   

	protected override void Awake()
	{
		var temp = gameObject.GetComponent<Image>();
		mImage = temp != null ? temp : Instantiate(AssetController.GetGameObject("SimpleImage"), transform).GetComponent<Image>();
		mImage.preserveAspect = true;

		base.Awake();
	}

	public override void InitiateComponent(string param, ChatBubble bubble)
	{
		base.InitiateComponent(param, bubble);
		mImage.transform.SetParent(mChatBubble.mediaParent);
	}

	protected override void ResizeComponent(Vector2 size)
	{
		base.ResizeComponent(size);

		if (image.sprite == null)
			return;
		
		mLayoutElement.preferredWidth = mImage.sprite.texture.width * mScaler;
		mLayoutElement.preferredHeight = mImage.sprite.texture.height * mScaler;
	}
}