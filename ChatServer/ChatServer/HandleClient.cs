using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;

namespace ChatServer
{
	public class HandleClinet
    {
        TcpClient clientSocket;
        string clNo;
        Hashtable clientsList;

		public void StartClient(TcpClient clientSocket, string clNo, Hashtable clientsList)
        {
			this.clientSocket = clientSocket;
			this.clNo = clNo;
			this.clientsList = clientsList;
            Thread ctThread = new Thread(Chat);
            ctThread.Start();
        }

		private void Chat()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[10025];
            string dataFromClient = null;
            string rCount = null;
            requestCount = 0;

            while (true)
            {
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$", StringComparison.Ordinal));
                    Console.WriteLine("From client - " + clNo + " : " + dataFromClient);
                    rCount = Convert.ToString(requestCount);

                    ChatServer.broadcast(dataFromClient, clNo, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
