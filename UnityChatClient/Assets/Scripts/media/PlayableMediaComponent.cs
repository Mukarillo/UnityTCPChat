using UnityEngine;

public abstract class PlayableMediaComponent<T> : LoadableMediaComponent<T>
{
	protected virtual Sprite IdleSprite => AssetController.GetSprite("PlayButton");
    protected virtual Sprite PlaySprite => AssetController.GetSprite("PauseButton");

	public override void ChangeState(MediaButtonState state)
    {
        base.ChangeState(state);
		interactable = state != MediaButtonState.LOADING;

        if (state == MediaButtonState.IDLE)
			mImage.sprite = IdleSprite;
        else if (state == MediaButtonState.PLAYING)
			mImage.sprite = PlaySprite;

		mImage.transform.rotation = Quaternion.identity;
    }
}