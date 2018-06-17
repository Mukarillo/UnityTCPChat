using System.Net.Sockets;

namespace chatserver
{
    public class Client
    {
		public string name;
		public TcpClient client;

		public Client(TcpClient client)
        {
			this.client = client;
        }
    }
}
