using UnityEngine;

public class Sound : PlayableMediaComponent<AudioClip>
{
    protected override Vector2 componentSize => new Vector2(100, 100);
    
    protected override void OnClick()
    {
        AudioPlayer.ME.PlayAudio(this);
    }
}
