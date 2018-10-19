using System.Collections;
using UnityEngine;

public class VideoGalleryButton : MediaButton
{
    protected override string buttonText => "Video Gallery";
    protected override string buttonSpriteName => "attach";

    protected override void OnClick()
    {
        if (NativeGallery.IsMediaPickerBusy())
            return;

        PickVideo();
    }

    private void PickVideo()
    {
        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
        {
            Debug.Log("Video path: " + path);
            if (path != null)
            {
                StartCoroutine(LoadVideo(path));
            }
        }, "Select a video");

        Debug.Log("Permission result: " + permission);
    }

    private IEnumerator LoadVideo(string path)
    {
        WWW www = new WWW(@"file://" + path);

        yield return www;

        MediaController.ME.SendVideo(www.bytes);
    }
}