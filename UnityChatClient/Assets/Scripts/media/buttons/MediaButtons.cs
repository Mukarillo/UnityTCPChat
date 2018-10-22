using System;
using System.Collections.Generic;

public interface IMediaButtonInfo
{
    Type type { get; }
}
public class MediaButtonInfoInfo<T> : IMediaButtonInfo
{
    public Type type => typeof(T);
}

public class MediaButtons
{
    public static List<IMediaButtonInfo> Buttons = new List<IMediaButtonInfo>
    {
		new MediaButtonInfoInfo<DonationButton>(),
        new MediaButtonInfoInfo<StickersButton>(),
		new MediaButtonInfoInfo<PhotoCameraButton>(),
        new MediaButtonInfoInfo<PhotoGalleryButton>(),
		new MediaButtonInfoInfo<VideoCameraButton>(),
        new MediaButtonInfoInfo<VideoGalleryButton>()      
    };
}