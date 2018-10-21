public class Constants
{   
    //COMMANDS
    public const string ONLINE_CONNECTIONS = "[ONLINE_CONNECTIONS]";
	public const string PLAYER_CONNECTED = "[NEW_PLAYER]";
    public const string PLAYER_DISCONECTED = "[DISCONECTED_PLAYER]";

	public const string AUDIO_MESSAGE = "[AUDIO_MESSAGE]";
    public const string STICKER_MESSAGE = "[STICKER_MESSAGE]";
    public const string PHOTO_MESSAGE = "[PHOTO_MESSAGE]";
	public const string DONATE_MESSAGE = "[DONATE_MESSAGE]";
    public const string SET_USER = "[SET_USER]";

	public static readonly string[] COMMANDS = new[]{
		ONLINE_CONNECTIONS,
		PLAYER_CONNECTED,
		PLAYER_DISCONECTED,
		AUDIO_MESSAGE,
		STICKER_MESSAGE,
		PHOTO_MESSAGE,
		DONATE_MESSAGE,
		SET_USER
	};
}