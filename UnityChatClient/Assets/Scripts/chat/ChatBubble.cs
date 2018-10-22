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

	private int mMaxCharacterInOneLineUsers = 20;
	private int mMaxCharacterInOneLineServer = 40;
    
    public void SetMessage(Message message)
	{
		this.bubble.color = message.color;

		if (message.IsMediaMessage)
			SetMediaMessage(message);
		else if (message.isServer)
			SetServerMessage(message);
        else
		    this.message.text = SpliceText(message.GetMessageContent(), mMaxCharacterInOneLineUsers);

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

        mediaComponent.InitiateComponent(message.GetMessageContent(), this);
    }

	private void SetServerMessage(Message message)
	{
		var t = "";
		if (message.message.Contains(Constants.PLAYER_DISCONECTED))
			t = message.GetMessageContent() + " disconected.";
        else if (message.message.Contains(Constants.PLAYER_CONNECTED))
			t = message.GetMessageContent() + " connected.";
        else if (message.message.Contains(Constants.ONLINE_CONNECTIONS))
			t = "Users online: \n" + string.Join(", ", message.GetMessageContent().Split(','));
		else if (message.message.Contains(Constants.DONATE_MESSAGE))
		{
			var contents = message.message.Replace(Constants.DONATE_MESSAGE, "").Split(',');
			t = string.Format("User <b>{0}</b> donated <b>{1}$</b> to user <b>{2}</b>", contents[0], contents[1], contents[2]);
		}

		this.message.text = SpliceText(t, mMaxCharacterInOneLineServer);
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
			if(preProcess[i].Length > lineLength){
				string[] nString = SplitInParts(preProcess[i], lineLength).ToArray();
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
