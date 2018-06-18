using System.Net.Sockets;

namespace chatserver
{
    public class Client
    {
		public string userName;
		public Color color;
		public TcpClient client;

		public Client(TcpClient client)
        {
			this.client = client;
        }
    }
}
