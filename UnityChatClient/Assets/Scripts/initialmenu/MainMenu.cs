using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public InputField userNameInputField;
	public ColorSelector colorSelector;

	public void Awake()
	{
		Screen.SetResolution(1000, 1600, false);
		Application.runInBackground = true;
	}

	public void Connect()
	{
		if (string.IsNullOrEmpty(userNameInputField.text)) return;

		gameObject.SetActive(false);      
		ChatClient.ME.Connect(userNameInputField.text, colorSelector.GetCurrentColor());      
	}   
}
