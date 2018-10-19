using UnityEngine;
public abstract class MediaSecondaryPanelComponent : MonoBehaviour
{
    public abstract void Initiate(RectTransform content);
    public abstract void Dispose();
}