using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class ChatBubble : MonoBehaviour {
	public Image bubble;
	public Text message;
	public Text date;
	public Text userName;
	public AudioPlayButton audioPlayButton;

	private int mMaxCharacterInOneLine = 20;
    
    public void SetMessage(Message message)
	{
		this.bubble.color = message.color;

		audioPlayButton?.gameObject?.SetActive(message.IsAudioMessage);

		if (!message.IsAudioMessage)
			this.message.text = SpliceText(message.GetMessage(), mMaxCharacterInOneLine);
		else
			audioPlayButton.Initiate(message.GetMessage(), this);

		date.text = message.GetDate();
  
		if(!message.isServer)
			userName.text = message.userName;
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
