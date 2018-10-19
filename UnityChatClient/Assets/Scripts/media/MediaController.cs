﻿using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MediaController : MonoBehaviour
{
    private const float TIME_TO_TWEEN = 0.2f;
    private const float TIME_TO_FADE = 0.05f;

    public static MediaController ME;

    public RectTransform footer;
    public RectTransform media;

    public MediaSecondaryPanel secondaryPanel;

    public GameObject mediaButtonPrefab;
    public RectTransform mediaButtonsParent;

    public Image backgroundImage;

    private bool mShowingSecondaryPanel;

    private void Awake()
    {
        ME = this;

        foreach (var btInfo in MediaButtons.Buttons)
            GameObject.Instantiate(mediaButtonPrefab, mediaButtonsParent).AddComponent(btInfo.type);

        backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 0f);
    }

    public void OpenMedia()
    {
        ChatController.ME.ClearMessageInput();

        footer.DOAnchorPosY(-footer.rect.height, TIME_TO_TWEEN);
        media.DOAnchorPosY(0f, TIME_TO_TWEEN).OnComplete(() => { backgroundImage.DOFade(0.4f, TIME_TO_FADE); });
    }

    public void BackMedia()
    {
        if (mShowingSecondaryPanel)
        {
            CloseSecondaryPanel();
            return;
        }

        ForceCloseMedia();
    }

    public void ForceCloseMedia()
    {
        backgroundImage.DOFade(0.0f, TIME_TO_TWEEN).OnComplete(() =>
        {
            CloseSecondaryPanel();

            footer.DOAnchorPosY(0f, TIME_TO_TWEEN);
            media.DOAnchorPosY(-media.rect.height, TIME_TO_TWEEN);
        });
    }
    
    public void OpenSecondaryPanel<T>() where T : MediaSecondaryPanelComponent
    {
        secondaryPanel.ShowPanel<T>();
        mediaButtonsParent.DOAnchorPosX(-mediaButtonsParent.rect.width, TIME_TO_TWEEN);
        mShowingSecondaryPanel = true;
    }

    private void CloseSecondaryPanel()
    {
        mediaButtonsParent.DOAnchorPosX(0f, TIME_TO_FADE).OnComplete(() =>
        {
            secondaryPanel.DisposeCurrentPanel();
            mShowingSecondaryPanel = false;
        });
    }

    public void SendSticker(string name)
    {
        ForceCloseMedia();

        ChatClient.ME.SendMessageToServer(Constants.STICKER_MESSAGE + name);
    }

    public void SendPhoto(Texture2D image)
    {
        ForceCloseMedia();
        var uploader = new SaveToServer(this, image, "photo_" + ChatClient.ME.userName + DateTime.Now.ToString("yyyyMMddHHmmss"), OnSucceedUploadingPhoto, OnFailedUploadingPhoto);
    }

    private void OnSucceedUploadingPhoto(string fileName)
    {
        ChatClient.ME.SendMessageToServer(Constants.PHOTO_MESSAGE + FileServerInfo.PHOTO_FOLDER_URL + fileName);
    }

    private void OnFailedUploadingPhoto(string error)
    {

    }

    public void SendVideo(byte[] video)
    {
        new SaveToServer(this, video, "video_" + ChatClient.ME.userName + DateTime.Now.ToString("yyyyMMddHHmmss"), OnSucceedUploadingVideo, OnFailedUploadingVideo);
    }

    private void OnSucceedUploadingVideo(string fileName)
    {
        ChatClient.ME.SendMessageToServer(Constants.VIDEO_MESSAGE + FileServerInfo.VIDEO_FOLDER_URL + fileName);
    }

    private void OnFailedUploadingVideo(string error)
    {
        throw new NotImplementedException();
    }
    
    private void OnDestroy()
    {
        ME = null;
    }
}
