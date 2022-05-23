using System;
using System.Collections.Generic;

[Serializable]
public class Message
{
    public string id;
    public string channel_id;
    public string guild_id;
    public string content;
    public string timestamp;
    public string edited_timestamp;
    public string mention_everyone;
    public User author;
    public List<MessageAttachment> attachments;
    public List<MessageEmbed> embeds;
    public List<User> mentions;
    public Member member;
    public int seq;
    public string seq_in_channel;
    public MessageReference message_reference;
    public string src_guild_id;
}
[Serializable]
public class BotMessage
{
    public string content;
}
[Serializable]
public class MessageEmbed
{
    public string title;
    public string prompt;
    public MessageEmbedThumbnail thumbnail = new MessageEmbedThumbnail();
    public List<MessageEmbedField> fields;
}
[Serializable]
public class MessageEmbedThumbnail
{
    public string url;
}
[Serializable]
public class MessageEmbedField
{
    public string url;
}
[Serializable]
public class MessageAttachment
{
    public string url;
}
public class MessageReference
{
    public string message_id;
    public bool ignore_get_message_error;
}