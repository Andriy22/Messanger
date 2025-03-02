using System;
using System.Collections.Generic;

public class Chat
{
    public long Id;
    public string Title;
    public string AvatarUrl;
    public List<User> Participants;
    public List<Message> Messages;
    public DateTime CreatedAt;

    public DateTime LastUpdatedAt => Messages.Count > 0 ? Messages[^1].Sent : CreatedAt;
}