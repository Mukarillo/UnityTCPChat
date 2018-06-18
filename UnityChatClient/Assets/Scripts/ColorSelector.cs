using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelector : MonoBehaviour {
	public Color[] possibilities;
	public GameObject colorOptionPrefab;
	public Transform colorOptionParent;

	public RectTransform selectedColorObject;

	private int mCurrentChoosen = 0;
	private List<Transform> mColors = new List<Transform>();

    public Color GetCurrentColor()
	{
		return possibilities[mCurrentChoosen];
	}

	void Start () {
		for (int i = 0; i < possibilities.Length; i++)
		{
			var index = i;
			var item = Instantiate(colorOptionPrefab, colorOptionParent);
			item.GetComponent<Image>().color = possibilities[i];
			item.GetComponent<Button>().onClick.AddListener(() =>
			{
				mCurrentChoosen = index;
				PlaceSelectedIcon(item.transform);
			});
			mColors.Add(item.transform);
		}

		PlaceSelectedIcon(mColors[0]);
	}

	private void PlaceSelectedIcon(Transform color)
	{
		selectedColorObject.transform.SetParent(color);
		selectedColorObject.anchoredPosition = Vector2.zero;
	}
}
