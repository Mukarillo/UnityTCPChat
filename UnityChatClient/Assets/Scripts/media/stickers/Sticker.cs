using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sticker : MediaComponent
{
    private string mStickerName;
    private List<Sprite> mSprites;
    private int mCurrentSpriteIndex;
    private int mFrameRate;
    private int mFrameCounter;
    private bool mLoop;

    private bool mIsPlaying;
    private bool mClickDisabled;

    protected override Vector2 componentSize => new Vector2(200, 200);

    protected override void Awake()
    {
        base.Awake();

        mIsPlaying = false;
    }

    protected override void OnClick()
    {
        if (mClickDisabled)
            return;

        MediaController.ME.SendSticker(mStickerName);
    }

    public override void InitiateComponent(string param, ChatBubble bubble)
    {
        SetupSticker(param);
    }

    public void ToggleClick(bool enabled)
    {
        mClickDisabled = !enabled;
    }

    public Sticker SetupSticker(string spritePrefix, bool autoPlay = true, bool loop = true, int frameRate = 5)
    {
        mStickerName = spritePrefix;

        Sprite sprite;
        mSprites = AssetController.TryGetSprite(spritePrefix, out sprite) ? new List<Sprite> {sprite} : AssetController.GetAnimation(spritePrefix);
        
        mFrameRate = frameRate;
        mLoop = loop;

        mIsPlaying = autoPlay;

        ChangeSpriteIndex(0);

        return this;
    }

    public override void OnPointerEnter(PointerEventData pointerEventData)
    {
        base.OnPointerEnter(pointerEventData);

        if (mClickDisabled)
            return;

        mIsPlaying = true;
    }

    public override void OnPointerExit(PointerEventData pointerEventData)
    {
        base.OnPointerExit(pointerEventData);

        if (mClickDisabled)
            return;

        mIsPlaying = false;
        ChangeSpriteIndex(0);
    }

    private void Update()
    {
        if (!mIsPlaying || mSprites == null || mSprites.Count <= 1 || (!mLoop && mCurrentSpriteIndex == mSprites.Count - 1))
            return;

        mFrameCounter++;
        if (mFrameCounter < mFrameRate)
            return;

        mFrameCounter = 0;
        ChangeSpriteIndex(mCurrentSpriteIndex + 1 >= mSprites.Count ? 0 : mCurrentSpriteIndex + 1);
    }

    private void ChangeSpriteIndex(int index)
    {
        mCurrentSpriteIndex = index;
        mImage.sprite = mSprites[mCurrentSpriteIndex];
    }
}
