using System;
using System.Collections.Generic;

namespace chatserver
{
    public class Message
    {
		public string userName;
		public string message;
		public Color color;

		public bool isMine;
		public bool isServer;
		public DateTime dateTime;
      
		public Message(string userName, string message, Color color, bool isMine, bool isServer, DateTime dateTime)
        {
			this.userName = userName;
			this.message = message;
			this.color = color;         
			this.isMine = isMine;
			this.isServer = isServer;
			this.dateTime = dateTime;
        }

		public string ToJson()
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("userName", userName);
            dictionary.Add("message", message);
            dictionary.Add("color", ColorToString(color));
            dictionary.Add("isMine", isMine.ToString());
            dictionary.Add("isServer", isServer.ToString());
            dictionary.Add("dateTime", dateTime.ToString());
            return MiniJSON.Json.Serialize(dictionary);
        }

        public static Message FromJson(string json)
        {
			IDictionary<string, object> des = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
            var message = new Message(des["userName"].ToString(),
                                      des["message"].ToString(),
                                      ParseColor(des["color"].ToString()),
                                      bool.Parse(des["isMine"].ToString()),
                                      bool.Parse(des["isServer"].ToString()),
                                      DateTime.Parse(des["dateTime"].ToString()));
            return message;
        }   

		private static Color ParseColor(string color)
        {
            var rgba = color.Split(',');
            return new Color(float.Parse(rgba[0]), float.Parse(rgba[1]), float.Parse(rgba[2]), float.Parse(rgba[3]));
        }

        private static string ColorToString(Color c)
        {
            return string.Format("{0},{1},{2},{3}", c.r, c.g, c.b, c.a);
        }
    }
}
