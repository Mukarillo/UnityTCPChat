using UnityEngine;
using UnityEngine.UI;

public class Photo : LoadableMediaComponent<Texture2D>
{
    protected override Vector2 componentSize => new Vector2(400, 400);

	protected Image mPhoto;

    protected override void OnSucceedRetrieving(Texture2D file)
    { 
        base.OnSucceedRetrieving(file);
              
		mImage.sprite = Sprite.Create(file, new Rect(0, 0, file.width, file.height), Vector2.zero);

    }
}