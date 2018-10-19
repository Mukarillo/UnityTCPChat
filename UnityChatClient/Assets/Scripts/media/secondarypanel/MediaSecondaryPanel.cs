using UnityEngine;

public class MediaSecondaryPanel : MonoBehaviour
{
    public RectTransform content;

    private MediaSecondaryPanelComponent mCurrentComponent;

    public void ShowPanel<T>() where T : MediaSecondaryPanelComponent
    {
        if (mCurrentComponent != null)
        {
            mCurrentComponent.Dispose();
            Destroy(mCurrentComponent);
        }
        
        mCurrentComponent = (MediaSecondaryPanelComponent)gameObject.AddComponent(typeof(T));
        mCurrentComponent.Initiate(content);
    }

    public void DisposeCurrentPanel()
    {
        if (mCurrentComponent == null)
            return;

        mCurrentComponent.Dispose();
        Destroy(mCurrentComponent);
        mCurrentComponent = null;
    }
}
