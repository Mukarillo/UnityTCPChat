using UnityEngine;
using UnityEngine.UI;

public abstract class PlayableMediaComponent<T> : LoadableMediaComponent<T>
{
    protected virtual Sprite IdleSprite => AssetController.GetSprite("PlayButton");
    protected virtual Sprite PlaySprite => AssetController.GetSprite("PauseButton");

    public override void ChangeState(MediaButtonState state)
    {
        base.ChangeState(state);
        interactable = state != MediaButtonState.LOADING;

        if (state == MediaButtonState.IDLE)
            ((Image) targetGraphic).sprite = IdleSprite;
        else if (state == MediaButtonState.PLAYING)
            ((Image) targetGraphic).sprite = PlaySprite;

        targetGraphic.transform.rotation = Quaternion.identity;
    }
}