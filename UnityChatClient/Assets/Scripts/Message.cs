using System;

public class Message
{
	public string userName;      
	public bool mine;
	public bool server;

	private string mRealMessage;
    private string message;
	private DateTime dateTime;

	public override string ToString()
	{
		var toReturn = "";
		if(mRealMessage.Contains(Constants.ONLINE_CONNECTIONS))
			toReturn = string.Format("People online: {0}", message);
		else
			toReturn = string.Format("[S:{0}, ME:{1}, {2}] {3}: {4}", server, mine, dateTime.ToString(), userName, message);
		      
		return toReturn;
	}

	public Message(string messageFromServer)
	{
		mRealMessage = messageFromServer;

		var splitted = messageFromServer.Split(Constants.MESSAGE_SEPARATOR);
		server = bool.Parse(splitted[0]);
		mine = bool.Parse(splitted[1]);
		dateTime = DateTime.Parse(splitted[2]);

		userName = splitted[3];
		message = "";
		for (int i = 4; i < splitted.Length; i++)
			message += splitted[i];
	}

    public string GetMessage()
	{
		var toReturn = "";
		if (mRealMessage.Contains(Constants.ONLINE_CONNECTIONS))
			toReturn = string.Format("People online: {0}", message);
		else
			toReturn = message;

        return toReturn;
	}

    public string GetDate()
	{
		return dateTime.ToString("HH:mm:ss");
	}
}
