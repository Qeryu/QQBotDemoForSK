using System;

[Serializable]
public class ConfigFile
{
    public string BotAppID;
    public string BotToken;
    public string BotKey;
    public string BotWssAddr;
    public string BotHttpsAddr;
    public string guild_id;
    public string channel_id;
    public string BotID;
    public string Authorization()
    {
        return "Bot " + BotAppID + "." + BotToken;
    }
}