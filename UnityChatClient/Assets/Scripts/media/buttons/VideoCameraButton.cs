using System.Collections;
using UnityEngine;

public class VideoCameraButton : MediaButton
{
    protected override string buttonText => "Video Camera";
    protected override string buttonSpriteName => "camera";

    protected override void OnClick()
    {
        if (NativeGallery.IsMediaPickerBusy())
            return;
        
		RecordVideo();
    }

    private void RecordVideo()
    {
		NativeCamera.Permission permission = NativeCamera.RecordVideo((path) =>
        {
			Debug.Log("Video path: " + path);
            if (path != null)
            {
                StartCoroutine(LoadVideo(path));
            }
        });

        Debug.Log("Permission result: " + permission);
    }

	private IEnumerator LoadVideo(string path)
    {
        WWW www = new WWW(@"file://" + path);

        yield return www;

        MediaController.ME.SendVideo(www.bytes);
    }
}