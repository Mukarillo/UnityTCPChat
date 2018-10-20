using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Generic;
using System.Text;

namespace chatserver
{
    public class ChatServer
    {
		static readonly object _lock = new object();
		static readonly Dictionary<int, Client> list_clients = new Dictionary<int, Client>();

        static void Main(string[] args)
        {
            int count = 1;

			TcpListener ServerSocket = new TcpListener(IPAddress.Any, 16005);
            ServerSocket.Start();
			Console.WriteLine("Server started");

            while (true)
            {
				Client client = new Client(ServerSocket.AcceptTcpClient());
                lock (_lock) list_clients.Add(count, client);

                Thread t = new Thread(HandleClient);
                t.Start(count);
                count++;
            }
        }

		public static void HandleClient(object o)
        {
            int id = (int)o;
			Client c;
			string message;
            lock (_lock) c = list_clients[id];

			while (true)
			{
				byte[] buffer = new byte[1024];
				int byte_count = 0;
            
				try
				{
					NetworkStream stream = c.client.GetStream();
					byte_count = stream.Read(buffer, 0, buffer.Length);
				}catch(Exception e)
				{
					Console.WriteLine(e);
					continue;
				}

				if (byte_count == 0)
				{
					break;
				}

				string data = Encoding.ASCII.GetString(buffer, 0, byte_count);
				var uMessage = Message.FromJson(data);
			    uMessage.dateTime = DateTime.Now;
				if (uMessage.message.Contains(Constants.SET_USER))
				{
					c.SetClient(uMessage.userName, uMessage.color);               

					var text = Constants.ONLINE_CONNECTIONS;
					foreach (var kvp in list_clients)
						text += (kvp.Key == id ? "You" : kvp.Value.userName) + ", ";
					text = text.Remove(text.Length - 2);

					message = new Message(c.userName, text, Color.ServerColor, false, true, DateTime.Now).ToJson();
					SendMessageToClient(c, message);

					message = new Message(c.userName, c.userName + " connected.", Color.ServerColor, false, true, DateTime.Now).ToJson();
					BroadcastExcept(message, id);
					Console.WriteLine(c.userName + " connected.");
				}
			    else
				{
					Broadcast(uMessage.message, id, false);
					Console.WriteLine(string.Format("{0}({1}): {2}", uMessage.userName, id, uMessage.message));
				}
            }
                     
            lock (_lock) list_clients.Remove(id);
			c.client.Client.Shutdown(SocketShutdown.Send);
			c.client.Close();
                     
			Broadcast(c.userName + " disconnected.", Constants.SERVER_ID, true);
			Console.WriteLine(c.userName + " disconnected.");
        }

		public static void SendMessageToClient(Client client, string message)
		{
			NetworkStream stream = client.client.GetStream();
			var buffer = Encoding.ASCII.GetBytes(message);

			stream.Write(buffer, 0, buffer.Length);
		}

        public static void BroadcastExcept(string data, int excluded)
		{
			byte[] buffer;

            lock (_lock)
            {
                foreach (var c in list_clients)
                {
					if (c.Key == excluded) continue;
                    NetworkStream stream = c.Value.client.GetStream();               
					buffer = Encoding.ASCII.GetBytes(data);               
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
		}

		public static void Broadcast(string data, int senderId, bool isServer)
        {
			byte[] buffer;

            lock (_lock)
            {
                foreach (var c in list_clients)
                {
					NetworkStream stream = c.Value.client.GetStream();
					var name = senderId == Constants.SERVER_ID ? Constants.SERVER_NAME : list_clients[senderId].userName;
					var color = senderId == Constants.SERVER_ID ? Color.ServerColor : list_clients[senderId].color;
					var message = new Message(name, data, color, c.Key == senderId, isServer, DateTime.Now).ToJson();
					buffer = Encoding.ASCII.GetBytes(message);

                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }     
    }   
}