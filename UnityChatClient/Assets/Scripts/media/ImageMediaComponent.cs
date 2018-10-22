using UnityEngine.UI;

public abstract class ImageMediaComponent : MediaComponent
{
	protected Image mImage;

	protected override void Awake()
	{
		base.Awake();
		var temp = gameObject.GetComponent<Image>();
		mImage = temp != null ? temp : Instantiate(AssetController.GetGameObject("SimpleImage"), transform).GetComponent<Image>();
		mImage.preserveAspect = true;
	}

	public override void InitiateComponent(string param, ChatBubble bubble)
	{
		base.InitiateComponent(param, bubble);
		mImage.transform.SetParent(mChatBubble.mediaParent);
	}
}