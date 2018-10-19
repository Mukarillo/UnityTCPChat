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

    protected LayoutElement mLayoutElement;
    protected Image mImage;

    protected override void Awake()
    {
        base.Awake();
        mLayoutElement = gameObject.GetComponent<LayoutElement>() ?? gameObject.AddComponent<LayoutElement>();
        mImage = gameObject.GetComponent<Image>();
        mImage.preserveAspect = true;

        ResizeComponent(componentSize);
        onClick.AddListener(OnClick);
    }

    protected abstract void OnClick();

    public abstract void InitiateComponent(string param, ChatBubble bubble);

    protected virtual void ResizeComponent(Vector2 size)
    {
        mLayoutElement.minWidth = size.x;
        mLayoutElement.minHeight = size.y;
    }
}