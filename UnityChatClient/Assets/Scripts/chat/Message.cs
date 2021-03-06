﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Message
{
    public string userName;
    public string message;
    public Color color;

    public bool isMine;
    public bool isServer;
    public DateTime dateTime;

    public bool IsAudioMessage => message.Contains(Constants.AUDIO_MESSAGE);
    public bool IsStickerMessage => message.Contains(Constants.STICKER_MESSAGE);
    public bool IsPhotoMessage => message.Contains(Constants.PHOTO_MESSAGE);
	public bool IsVideoMessage => message.Contains(Constants.VIDEO_MESSAGE);
	public bool IsMediaMessage => IsStickerMessage || IsAudioMessage || IsPhotoMessage || IsVideoMessage;

public Message(string userName, string message, Color color, bool isMine = true, bool isServer = false, DateTime dateTime = default(DateTime))
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
		var m = new Message(des["userName"].ToString(),
		                          des["message"].ToString(),
		                          ParseColor(des["color"].ToString()),
								  bool.Parse(des["isMine"].ToString()),
								  bool.Parse(des["isServer"].ToString()),
		                          DateTime.Parse(des["dateTime"].ToString()));
        return m;
    }

    public string GetMessageContent()
	{      
		foreach(var command in Constants.COMMANDS)
		{
			if(message.Contains(command))
				return message.Replace(command, "");
		}

		return message;
	}

    public string GetDate()
	{
		return dateTime.ToString("HH:mm:ss");
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
