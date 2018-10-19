using UnityEngine;
using UnityEngine.UI;

public abstract class MediaButton : Button
{
    protected abstract string buttonText { get; }
    protected abstract string buttonSpriteName { get; }

    protected override void Awake()
    {
        base.Awake();

        transform.Find("Icon").GetComponent<Image>().sprite = AssetController.GetSprite(buttonSpriteName);
        transform.Find("Text").GetComponent<Text>().text = buttonText;

        onClick.AddListener(OnClick);
    }

    protected abstract void OnClick();
}
