using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

internal class ChatScales
{
	public Vector2 anchorMin { get; private set; }
    public Vector2 anchorMax { get; private set; }
	public float bubbleScaler { get; private set; }

	public ChatScales(Vector2 anchorMin, Vector2 anchorMax, float bubbleScaler)
	{      
        this.anchorMin = anchorMin;
		this.anchorMax = anchorMax;
		this.bubbleScaler = bubbleScaler;    
	}
}

public class ChatScaller : MonoBehaviour {

	private const float TIME_TO_TWEEN = 0.5f;

    public enum ChatSizes
	{
		Micro,
        Normal,
        FullScreen
	}

	private Dictionary<ChatSizes, ChatScales> mReferenceScales = new Dictionary<ChatSizes, ChatScales>()
	{
		{ChatSizes.Micro, new ChatScales(new Vector2(0.03f, 0.45f), new Vector2(0.35f, 0.63f), 0.3f)},
		{ChatSizes.Normal, new ChatScales(new Vector2(0f, 0f), new Vector2(0.37f, 1f), 0.4f)},
		{ChatSizes.FullScreen, new ChatScales(new Vector2(0f, 0f), new Vector2(1f, 1f), 1f)}
	};

	private ChatSizes mCurrentSize = ChatSizes.Micro;
	private RectTransform mRectTransform;

	private void Awake()
	{
		mRectTransform = GetComponent<RectTransform>();
		ChangeSizeType(ChatSizes.Micro);
	}

	public void ChangeSizeType(ChatSizes size)
	{
		ChatController.ME.MediaButton.SetActive(size == ChatSizes.FullScreen);
		ChatController.ME.microphonButton.SetActive(size == ChatSizes.FullScreen);

		mCurrentSize = size;

		mRectTransform.DOAnchorMin(mReferenceScales[mCurrentSize].anchorMin, TIME_TO_TWEEN).OnUpdate(() =>
		{
			mRectTransform.offsetMin = Vector2.zero;
		});
		mRectTransform.DOAnchorMax(mReferenceScales[mCurrentSize].anchorMax, TIME_TO_TWEEN).OnUpdate(() =>
        {
            mRectTransform.offsetMax = Vector2.zero;
        });

		var currentScale = ChatController.ME.scaler;      
		DOTween.To(() => currentScale, x => ChatController.ME.SetBubbleScaler(x), mReferenceScales[mCurrentSize].bubbleScaler, TIME_TO_TWEEN);
	}

    public void ToggleSize()
	{
		if (mCurrentSize == ChatSizes.FullScreen)
        {
            ChangeSizeType(ChatSizes.Micro);
        }
        else if (mCurrentSize == ChatSizes.Normal)
        {
            ChangeSizeType(ChatSizes.FullScreen);
        }
        else if (mCurrentSize == ChatSizes.Micro)
        {
            ChangeSizeType(ChatSizes.Normal);
        }
	}   
}
