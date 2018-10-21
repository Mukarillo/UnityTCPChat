using UnityEngine;

public class StickersButton : MediaButton
{
    protected override string buttonText => "Stickers";
    protected override string buttonSpriteName => "stickers";

	protected override void OnClick()
    {
        MediaController.ME.OpenSecondaryPanel<StickersPanel>("StickerPanel");
    }
}