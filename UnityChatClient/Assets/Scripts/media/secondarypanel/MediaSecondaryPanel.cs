using UnityEngine;

public class MediaSecondaryPanel : MonoBehaviour
{   
    private MediaSecondaryPanelComponent mCurrentComponent;

	private void Awake()
	{
		var rt = GetComponent<RectTransform>();
		rt.anchoredPosition = new Vector2(rt.anchoredPosition.x + rt.rect.width, rt.anchoredPosition.y);
	}

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
