using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;

public class ChatBubble : MonoBehaviour {
	public Image bubble;
	public TextMeshProUGUI message;
	public Text date;
	public Text userName;
	public Transform mediaParent;
    public MediaComponent mediaComponent;

	private int mMaxCharacterInOneLine = 20;
    
    public void SetMessage(Message message)
	{
		this.bubble.color = message.color;
        
        if (message.IsMediaMessage)
		    SetMediaMessage(message);
        else
		    this.message.text = SpliceText(message.GetMessage(), mMaxCharacterInOneLine);

        date.text = message.GetDate();
  
		if(!message.isServer)
			userName.text = message.userName;
	}

    private void SetMediaMessage(Message message)
    {
        if (message.IsStickerMessage)
        {
            mediaComponent = Instantiate(AssetController.GetGameObject("SimpleImage"), mediaParent).AddComponent<Sticker>();
            ((Sticker)mediaComponent).ToggleClick(false);
        }
        else if (message.IsAudioMessage)
        {
            mediaComponent = Instantiate(AssetController.GetGameObject("SimpleImage"), mediaParent).AddComponent<Sound>();
        }
        else if (message.IsPhotoMessage)
        {
            mediaComponent = Instantiate(AssetController.GetGameObject("SimpleImage"), mediaParent).AddComponent<Photo>();
        }
        else if (message.IsVideoMessage)
        {
            mediaComponent = Instantiate(AssetController.GetGameObject("SimpleImage"), mediaParent).AddComponent<Video>();
        }

        mediaComponent.InitiateComponent(message.GetMessage(), this);
    }

    public void ForceMessage(string message, Color textColor)
	{
		this.message.text = message;
		this.message.color = textColor;
	}

	private string SpliceText(string text, int lineLength)
	{
		string nText = string.Empty;;
		string[] preProcess = text.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
		for(int i = 0; i < preProcess.Length; i++){
			if(preProcess[i].Length > mMaxCharacterInOneLine){
				string[] nString = SplitInParts(preProcess[i], mMaxCharacterInOneLine).ToArray();
				for(int j = 0; j < nString.Length; j++){
					nText += nString[j]+"\n";
				}
			}else{
				nText += preProcess[i] + " ";
			}
		}

		var charCount = 0;
		var lines = nText.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries)
			.GroupBy(w => (charCount += w.Length + 1) / lineLength)
			.Select(g => string.Join(" ", g.ToArray()));
        
		return String.Join("\n", lines.ToArray());
	}

	private IEnumerable<String> SplitInParts(String s, Int32 partLength) {
		for (var i = 0; i < s.Length; i += partLength)
			yield return s.Substring(i, Math.Min(partLength, s.Length - i));
	}
}
