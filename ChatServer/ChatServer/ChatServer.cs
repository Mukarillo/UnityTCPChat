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

            TcpListener ServerSocket = new TcpListener(IPAddress.Any, 8888);
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

            lock (_lock) c = list_clients[id];

			while (true)
			{
				NetworkStream stream = c.client.GetStream();
				byte[] buffer = new byte[1024];
				int byte_count = stream.Read(buffer, 0, buffer.Length);

				if (byte_count == 0)
				{
					break;
				}

				string data = Encoding.ASCII.GetString(buffer, 0, byte_count);
				if (data.Contains(Constants.SET_USER_NAME))
				{
					c.name = data.Split(Constants.MESSAGE_SEPARATOR)[1];
					Console.WriteLine(c.name + " connected.");

					var message = Constants.ONLINE_CONNECTIONS;
					foreach (var kvp in list_clients)
						message += (kvp.Key == id ? "You" : kvp.Value.name) + ", ";
					message = message.Remove(message.Length - 2);
                    SendMessageToClient(c, message);
					Console.WriteLine(message);

					BroadcastExcept(Constants.MESSAGE_SEPARATOR + c.name + " connected.", id);
				}
			    else
				{
					Broadcast(data, id, false);
					Console.WriteLine(data);
				}
            }
                     
            lock (_lock) list_clients.Remove(id);
			c.client.Client.Shutdown(SocketShutdown.Send);
			c.client.Close();

			Console.WriteLine(c.name + " disconnected.");
			Broadcast(Constants.MESSAGE_SEPARATOR + c.name + " disconnected.", -1, true);
        }

		public static void SendMessageToClient(Client client, string message)
		{
			NetworkStream stream = client.client.GetStream();         
			var buffer = GetParsedMessage(message, false, true);

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

                    buffer = GetParsedMessage(data, false, true);

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

					buffer = GetParsedMessage(data, c.Key == senderId, isServer);

                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        private static byte[] GetParsedMessage(string message, bool isSender, bool isServer)
		{
			return Encoding.ASCII.GetBytes(isServer.ToString() + Constants.MESSAGE_SEPARATOR +
				                           isSender.ToString() + Constants.MESSAGE_SEPARATOR + 
			                               DateTime.Now.ToString() + Constants.MESSAGE_SEPARATOR + 
			                               message);
		}      
    }   
}