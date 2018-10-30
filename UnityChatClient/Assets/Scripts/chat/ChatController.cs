using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ChatController : MonoBehaviour {
	public static ChatController ME;
    
	public TMP_InputField messageInput;
	public ScrollRect scrollRect;

	public GameObject microphonButton;
	public GameObject MediaButton;

	public Transform bubblesParent;
    
	public PlayerStatus onPlayerConnected = new PlayerStatus();
	public PlayerStatus onPlayerDisconected = new PlayerStatus();
	public PlayersOnline onPlayerOnlineChanged = new PlayersOnline();

	public float scaler = 1f;
    
	private List<string> mUsersOnline = new List<string>();
	public List<string> playersOnline => mUsersOnline;

	private List<ChatBubble> mBubbles = new List<ChatBubble>();

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
		if(message.isServer)
			UpdateUsers(message);

		var bubble = Instantiate(AssetController.GetGameObject(message.isServer ? "ServerChatBubble" : message.isMine ? "UserChatBubble" : "OtherChatBubble"), bubblesParent).GetComponent<ChatBubble>();
		bubble.SetMessage(message, scaler);

		mBubbles.Add(bubble);

		Canvas.ForceUpdateCanvases();
		scrollRect.verticalNormalizedPosition = 0f;
		Canvas.ForceUpdateCanvases();
	}  

    public void SetBubbleScaler(float scaler)
	{
		this.scaler = scaler;
		mBubbles.ForEach(x => x.SetScaler(scaler));
	}

	private void UpdateUsers(Message message)
	{
		var messageContent = message.GetMessageContent();
		if (message.message.Contains(Constants.PLAYER_DISCONECTED))
		{
			mUsersOnline.Remove(messageContent);
			onPlayerDisconected?.Invoke(messageContent);
		}
		else if (message.message.Contains(Constants.PLAYER_CONNECTED))
		{
			mUsersOnline.Add(messageContent);
			onPlayerConnected?.Invoke(messageContent);         
		}
		else if (message.message.Contains(Constants.ONLINE_CONNECTIONS))
		{
			mUsersOnline.AddRange(message.GetMessageContent().Split(',').ToList());         
		}

		onPlayerOnlineChanged.Invoke(mUsersOnline.Count);
	}

	private void OnDestroy()
	{
		onPlayerConnected.RemoveAllListeners();
		onPlayerConnected = null;

		onPlayerDisconected.RemoveAllListeners();
		onPlayerDisconected = null;

		onPlayerOnlineChanged.RemoveAllListeners();
		onPlayerOnlineChanged = null;
	}
}
