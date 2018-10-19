using System.Threading;
using System.Net.Sockets;
using UnityEngine;
using System;
using System.Net;
using System.Text;

public class ChatClient : MonoBehaviour
{
	public static ChatClient ME;
 
	public Sprite loadingIcon;
	public Sprite pauseAudioIcon;
	public Sprite playAudioIcon;   
	public string userName;

	private Color mUserColor = Color.black;
	private TcpClient mClient = new TcpClient();
	private Thread mClientReceiveThread;

	public void Awake()
	{
		ME = this;
		MainThread.initiate();
		MainThread.setMainThread();
	}

	public void Connect (string name, Color color) {
		userName = name;
		mUserColor = color;

        try {           
            mClientReceiveThread = new Thread (new ThreadStart(ListenForData));          
            mClientReceiveThread.IsBackground = true;            
            mClientReceiveThread.Start();
			ChatController.ME.gameObject.SetActive(true);
        }       
        catch (Exception e) {           
            Debug.Log("On client connect exception " + e);      
        }   
    }   

	private void ListenForData() {
        try {
			mClient = new TcpClient();
			mClient.Connect(IPAddress.Parse("192.168.0.6"), 16005);
			Debug.Log("Connected");         

			SendMessageToServer(Constants.SET_USER, true);

            Byte[] bytes = new Byte[1024];
            while (true) {
				using (NetworkStream stream = mClient.GetStream()) {
                    int length;
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);

						Message message = Message.FromJson(Encoding.ASCII.GetString(incommingData));
						MainThread.invoke(() => { ChatController.ME.DisplayNewMessage(message); });
                    }               
                }           
            }         
        }         
        catch (SocketException socketException) {             
            Debug.Log("Socket exception: " + socketException);         
        }     
    }

	public void SendMessageToServer(string text, bool command = false) {
		if (mClient == null)
            return;
		
        try {     
			NetworkStream stream = mClient.GetStream();
            if (stream.CanWrite) {
				Message message = new Message(userName, text, mUserColor);
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(message.ToJson());
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            }
        }       
        catch (SocketException socketException) {             
            Debug.Log("Socket exception: " + socketException);         
        }
    }

	private void OnDestroy()
	{
		if (mClient == null) return;
		mClient.Client.Shutdown(SocketShutdown.Send);
		mClientReceiveThread.Join();
		mClient.GetStream().Close();
		mClient.Close();
	}
}
