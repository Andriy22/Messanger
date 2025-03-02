#define TEST_MESSAGES

using System;
using System.Collections.Generic;

public class Attachment
{
    public enum Type
    {
        Image,
        Video,
        Audio,
        Text,
        File,
    }

    public string Url;
    public Type FileType;
}

public class Message
{
    public long Id;
    public long ChatId;
    public long? ReplyTo;
    public User Author;
    public string Text;
    public DateTime Sent;
    public List<Attachment> Attachments = new ();
    public List<long> ReadBy = new ();

#if TEST_MESSAGES
    private static long _idCounter = 0;
    private static Dictionary<long, Message> _messages = new ();
    public static long MAX_ID => _idCounter;
    public static Message GetMessage(long id) => _messages[id];

    public Message(long chatId)
    {
        Id = _idCounter++;
        _messages[Id] = this;
        ChatId = chatId;
    }
#endif
}