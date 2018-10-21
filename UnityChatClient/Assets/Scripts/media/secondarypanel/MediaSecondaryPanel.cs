using UnityEngine;

public class MediaSecondaryPanel : MonoBehaviour
{   
    private MediaSecondaryPanelComponent mCurrentComponent;

    public void ShowPanel<T>(string prefabName) where T : MediaSecondaryPanelComponent
    {
        if (mCurrentComponent != null)
        {
            mCurrentComponent.Dispose();
			Destroy(mCurrentComponent.gameObject);
        }
        
		mCurrentComponent = (MediaSecondaryPanelComponent)Instantiate(AssetController.GetGameObject(prefabName), transform).GetComponent(typeof(T));
        mCurrentComponent.Initiate();
    }

    public void DisposeCurrentPanel()
    {
        if (mCurrentComponent == null)
            return;

        mCurrentComponent.Dispose();
		Destroy(mCurrentComponent.gameObject);
        mCurrentComponent = null;
    }
}
