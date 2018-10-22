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

    protected override void Awake()
    {
        base.Awake();
        mLayoutElement = gameObject.GetComponent<LayoutElement>() ?? gameObject.AddComponent<LayoutElement>();

        ResizeComponent(componentSize);
        onClick.AddListener(OnClick);
    }

    protected virtual void OnClick() { }

	public virtual void InitiateComponent(string param, ChatBubble bubble)
    {
        mChatBubble = bubble;
    }

    protected virtual void ResizeComponent(Vector2 size)
    {
        mLayoutElement.minWidth = size.x;
        mLayoutElement.minHeight = size.y;
    }
}