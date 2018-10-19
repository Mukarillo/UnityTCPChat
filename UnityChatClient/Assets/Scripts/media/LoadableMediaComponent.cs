
using UnityEngine;
using UnityEngine.UI;

public abstract class LoadableMediaComponent<T> : MediaComponent
{
    protected virtual Sprite LoadingSprite => AssetController.GetSprite("Loading");

    public T file;
    protected ChatBubble mChatBubble;
    protected bool mRotate;
    
    public override void InitiateComponent(string param, ChatBubble chatBubble)
    {
        mChatBubble = chatBubble;
        ChangeState(MediaButtonState.LOADING);

        mRotate = true;
        Debug.Log("Initiating download: " + param);
        var serverRetriever = new RetieveFromServer<T>(this, param, OnSucceedRetrieving, OnFailedRetrieving);
    }

    protected virtual void OnSucceedRetrieving(T file)
    {
        Debug.Log("OnSucceedRetrieving");
        mRotate = false;
        ChangeState(MediaButtonState.IDLE);

        this.file = file;
    }

    protected virtual void OnFailedRetrieving(string error)
    {
        Debug.Log("OnFailedRetrieving " + error);
        if (mChatBubble != null)
            mChatBubble.ForceMessage("Failed to file.", new Color(255, 68, 68));
        gameObject.SetActive(false);
    }

    public virtual void ChangeState(MediaButtonState state)
    {
        if (state == MediaButtonState.LOADING)
            ((Image) targetGraphic).sprite = LoadingSprite;
        mRotate = state == MediaButtonState.LOADING;

        targetGraphic.transform.rotation = Quaternion.identity;
    }

    protected virtual void Update()
    {
        if (!mRotate)
            return;

        targetGraphic.transform.Rotate(Vector3.forward * Time.deltaTime * 100);
    }
}