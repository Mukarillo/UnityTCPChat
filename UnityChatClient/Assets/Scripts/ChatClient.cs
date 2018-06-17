using System.Threading;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using System;  
using System.IO;
using System.Net;
using System.Text;

public class ChatClient : MonoBehaviour
{
	public static ChatClient ME;

	private string mUserName;
	private TcpClient mClient = new TcpClient();
	private Thread clientReceiveThread;

	public void Awake()
	{
		ME = this;
		MainThread.initiate();
		MainThread.setMainThread();
	}

	public void Connect (string name) {
		mUserName = name;

        try {           
            clientReceiveThread = new Thread (new ThreadStart(ListenForData));          
            clientReceiveThread.IsBackground = true;            
            clientReceiveThread.Start();
			ChatController.ME.gameObject.SetActive(true);
        }       
        catch (Exception e) {           
            Debug.Log("On client connect exception " + e);      
        }   
    }   

	private void ListenForData() {
        try {
			mClient = new TcpClient();
			mClient.Connect(IPAddress.Parse("127.0.0.1"), 8888);
			Debug.Log("Connected");         

			SendMessageToServer(Constants.SET_USER_NAME + mUserName, true);

            Byte[] bytes = new Byte[1024];
            while (true) {
                // Get a stream object for reading
				using (NetworkStream stream = mClient.GetStream()) {
                    int length;
                    // Read incomming stream into byte arrary.
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);

						Message message = new Message(Encoding.ASCII.GetString(incommingData));
						MainThread.invoke(() => { ChatController.ME.DisplayNewMessage(message); });
                    }               
                }           
            }         
        }         
        catch (SocketException socketException) {             
            Debug.Log("Socket exception: " + socketException);         
        }     
    }

	public void SendMessageToServer(string message, bool command = false) {
		if (mClient == null)
            return;
		
        try {     
			NetworkStream stream = mClient.GetStream();
            if (stream.CanWrite) {
				string clientMessage = command ? message : GetFormatedMessage(message);
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            }         
        }       
        catch (SocketException socketException) {             
            Debug.Log("Socket exception: " + socketException);         
        }
    }
   
    private string GetFormatedMessage(string message)
	{
		return mUserName + Constants.MESSAGE_SEPARATOR + message;
	}

	private void OnDestroy()
	{
		if (mClient == null) return;
		mClient.Client.Shutdown(SocketShutdown.Send);
		clientReceiveThread.Join();
		mClient.GetStream().Close();
		mClient.Close();
	}
}
