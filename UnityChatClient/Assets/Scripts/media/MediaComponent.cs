using UnityEngine;
using UnityEngine.UI;

public abstract class MediaComponent : Button
{
    public enum MediaButtonState
    {
        IDLE,
        PLAYING,
        LOADING
    }

    protected abstract Vector2 componentSize { get; }   
	protected ChatBubble mChatBubble;
    protected LayoutElement mLayoutElement;

	protected float mScaler = 1f;
	protected Vector2 mComponentSize => componentSize * mScaler;

    protected override void Awake()
    {
        base.Awake();
        mLayoutElement = gameObject.GetComponent<LayoutElement>() ?? gameObject.AddComponent<LayoutElement>();

		ResizeComponent(mComponentSize);
        onClick.AddListener(OnClick);
    }

    protected virtual void OnClick() { }

	public virtual void InitiateComponent(string param, ChatBubble bubble)
    {
        mChatBubble = bubble;
    }

	public virtual void SetScaler(float scaler)
	{
		mScaler = scaler;
		ResizeComponent(mComponentSize);
	}

    protected virtual void ResizeComponent(Vector2 size)
    {
        mLayoutElement.minWidth = size.x;
        mLayoutElement.minHeight = size.y;
    }
}