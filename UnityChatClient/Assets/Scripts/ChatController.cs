using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour {
	public static ChatController ME;

	public InputField messageInput;
	public ScrollRect scrollRect;

	public Transform bubblesParent;
	public GameObject myBubble;
	public GameObject othersBubble;
	public GameObject serverBubble;

	public void Awake()
	{
		ME = this;
		gameObject.SetActive(false);

		messageInput.onEndEdit.AddListener(OnEndEdit);
	}

	private void OnEndEdit(string arg0)
	{
		SendMessage();
	}

    public void ClearMessageInput()
    {
        messageInput.text = "";
    }

	public void SendMessage()
	{
		if (string.IsNullOrEmpty(messageInput.text)) return;
		string message = messageInput.text;
        ClearMessageInput();

		ChatClient.ME.SendMessageToServer(message);      
	}
       
	public void DisplayNewMessage(Message message)
	{
		var bubble = GameObject.Instantiate(message.isServer ? serverBubble : message.isMine ? myBubble : othersBubble, bubblesParent).GetComponent<ChatBubble>();
		bubble.SetMessage(message);

		Canvas.ForceUpdateCanvases();
		scrollRect.verticalNormalizedPosition = 0f;
		Canvas.ForceUpdateCanvases();
	}   
}
