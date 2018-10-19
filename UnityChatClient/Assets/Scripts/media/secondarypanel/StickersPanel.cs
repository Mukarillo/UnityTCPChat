using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickersPanel : MediaSecondaryPanelComponent
{
    private readonly string[] STICKERS_NAMES =
    {
        //animated
        "cooking", "cuting", "driving", "eating", "kissing", "loving", "playing", "sobbing", "typing", "unicorning",
        //static
        "rukki_brb", "rukki_confused", "rukki_dancing", "rukki_drugged", "rukki_gogo", "rukki_hi", "rukki_hmm", "rukki_hug",
        "rukki_lol", "rukki_love", "rukki_mad", "rukki_missyou", "rukki_nap", "rukki_okay", "rukki_omg", "rukki_rest", "rukki_shy",
        "rukki_snuggle", "rukki_stalker", "rukki_thankyou", "rukki_thumbsup", "rukki_whoa", "rukki_xoxo", "rukki_yolo"
    };
    private GridLayoutGroup mGrid;

    private List<Sticker> mStickerList = new List<Sticker>();

    public override void Initiate(RectTransform content)
    {
        mGrid = content.gameObject.AddComponent<GridLayoutGroup>();
        mGrid.childAlignment = TextAnchor.MiddleCenter;
        mGrid.spacing = Vector2.one * 5;
        mGrid.padding = new RectOffset(0, 0, 10, 10);
        mGrid.cellSize = new Vector2(130, 130);

        foreach (var sticker in STICKERS_NAMES)
            mStickerList.Add(GameObject.Instantiate(AssetController.GetGameObject("SimpleImage"), content).AddComponent<Sticker>().SetupSticker(sticker, false));
    }

    public override void Dispose()
    {
        mStickerList.ForEach(x => Destroy(x.gameObject));
        Destroy(mGrid);
    }
}