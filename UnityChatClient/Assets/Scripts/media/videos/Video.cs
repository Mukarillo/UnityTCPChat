using UnityEngine;

public class Video : PlayableMediaComponent<AudioClip>
{
    protected override Vector2 componentSize => new Vector2(400, 400);
    
    protected override void OnClick()
    {
        //VideoController.ME.PlayMovie(file);
    }
}