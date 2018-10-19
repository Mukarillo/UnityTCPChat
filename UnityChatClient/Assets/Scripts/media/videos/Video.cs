using UnityEngine;
using UnityEngine.Video;

public class Video : PlayableMediaComponent<MovieTexture>
{
    protected override Vector2 componentSize => new Vector2(400, 400);
    
    protected override void OnClick()
    {
        VideoController.ME.PlayMovie(file);
    }
}