using UnityEngine;

public class PhotoCameraButton : MediaButton
{
    protected override string buttonText => "Photo Camera";
    protected override string buttonSpriteName => "camera";

    protected override void OnClick()
    {
        if (NativeGallery.IsMediaPickerBusy())
            return;

		TakePicture(512);
    }

	private void TakePicture(int maxSize)
    {
		NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path == null) return;

            var texture = NativeGallery.LoadImageAtPath(path, maxSize, false, false);
            if (texture == null)
            {
                Debug.Log("Couldn't load texture from " + path);
                return;
            }

            MediaController.ME.SendPhoto(texture);
        }, maxSize);

        Debug.Log("Permission result: " + permission);
    }
}