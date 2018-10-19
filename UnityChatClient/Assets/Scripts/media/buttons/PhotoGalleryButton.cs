using UnityEngine;

public class PhotoGalleryButton : MediaButton
{
    protected override string buttonText => "Photo Gallery";
    protected override string buttonSpriteName => "attach";

    protected override void OnClick()
    {
        if (NativeGallery.IsMediaPickerBusy())
            return;

        PickImage(512);
    }

    private void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
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
        }, "Select a PNG image", "image/png", maxSize);

        Debug.Log("Permission result: " + permission);
    }
}