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

	public GridLayoutGroup grid;
	public RectTransform stickersParent;

    private List<Sticker> mStickerList = new List<Sticker>();

    public override void Initiate()
    {      
        foreach (var sticker in STICKERS_NAMES)
			mStickerList.Add(Instantiate(AssetController.GetGameObject("SimpleImage"), stickersParent).AddComponent<Sticker>().SetupSticker(sticker, false));
    }

    public override void Dispose()
    {
        
    }
}